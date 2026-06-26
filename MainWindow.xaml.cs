using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberSecurityChatBotWithUI
{
    public partial class MainWindow : Window
    {
        // ── colour palette ────────────────────────────────────────────────────
        //these match the colours defined in the xaml resource dictionary
        private static readonly SolidColorBrush ColourBot     = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x41)); //matrix green  - bot name tag
        private static readonly SolidColorBrush ColourUser    = new SolidColorBrush(Color.FromRgb(0x00, 0xBF, 0xFF)); //electric cyan - user name tag
        private static readonly SolidColorBrush ColourDivider = new SolidColorBrush(Color.FromRgb(0xFF, 0x45, 0x00)); //alert orange  - section dividers
        private static readonly SolidColorBrush ColourBody    = new SolidColorBrush(Color.FromRgb(0xC8, 0xC8, 0xC8)); //light grey    - all body text
        private static readonly SolidColorBrush ColourGold    = new SolidColorBrush(Color.FromRgb(0xFF, 0xD7, 0x00)); //gold          - quiz wrong-answer feedback

        // ── conversation states ───────────────────────────────────────────────
        //the bot works like a flowchart - each state controls what happens when
        //the user next sends a message
        private enum BotState
        {
            AwaitingName,       //waiting for the user to type their name
            AwaitingMenu,       //main menu is on screen, waiting for option 1-9

            AwaitingKeyword,    //user picked "other" (option 6), need a keyword

            //task assistant states - fill these in one step at a time
            AwaitingTaskMenu,   //task sub-menu is on screen (A/V/B)
            AwaitingTaskTitle,  //waiting for the new task's title
            AwaitingTaskDesc,   //waiting for the new task's description
            AwaitingTaskRem,    //asking if the user wants a reminder (Y/N)
            AwaitingTaskRemVal, //user said yes - waiting for the reminder text
            AwaitingTaskMgmt,   //task list is showing, waiting for DONE/DEL/B

            //quiz states
            AwaitingQuizAnswer, //a quiz question is displayed, waiting for A/B/C/D

            AwaitingContinue,   //bot answered something, asking Y/N to continue
            Done                //session over, input is disabled
        }

        //tracks who we're talking to and which stage the conversation is in
        private string   _username = "";
        private BotState _state    = BotState.AwaitingName;

        //── task assistant ────────────────────────────────────────────────────
        //hold onto the title and description between steps while the user fills
        //in the task details one message at a time
        private string _pendingTitle = "";
        private string _pendingDesc  = "";

        //── quiz ──────────────────────────────────────────────────────────────
        private List<QuizQuestion> _quizQuestions = new List<QuizQuestion>();
        private int _quizIndex = 0; //which question we're currently on (0-based)
        private int _quizScore = 0; //number of correct answers this session

        // ── constructor ───────────────────────────────────────────────────────
        public MainWindow()
        {
            InitializeComponent();

            //set up the sqlite database on first run (creates file + table if missing)
            TaskManager.Initialise();

            //record that a new session has started in the activity log
            ActivityLog.WriteSessionStart();

            //let the user press enter to send instead of clicking the button
            txtMessage.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                    SendMessage();
            };

            //opening messages - same as the original
            BotSay("Welcome to the CyberSecurity Awareness ChatBot.");
            BotSay("Please enter your name:");
        }

        // ── button click handlers ─────────────────────────────────────────────
        private void BtnSend_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            //log the exit before shutting down
            ActivityLog.WriteSessionEnd(_username.Length > 0 ? _username : "unknown");
            Application.Current.Shutdown();
        }

        // ── main input router ─────────────────────────────────────────────────
        //every message lands here first - we run nlp on it then route to the
        //correct handler based on the current state
        private void SendMessage()
        {
            string input = txtMessage.Text.Trim();
            txtMessage.Clear();

            if (string.IsNullOrEmpty(input))
                return;

            //run nlp analysis to detect the user's tone and intent
            var nlp = NlpHelper.Analyse(input);

            //handle greetings mid-session (outside the normal name-entry state)
            if (nlp.Intent == "greeting" && _state != BotState.AwaitingName)
            {
                UserSay(input);
                BotSay($"Hey again, {_username}! What can I help you with?");
                ActivityLog.Write($"nlp: greeting detected from {_username}");
                return;
            }

            //handle thanks mid-session when we're waiting for a Y/N continue
            if (nlp.Intent == "thanks" && _state == BotState.AwaitingContinue)
            {
                UserSay(input);
                BotSay($"You're welcome, {_username}! Type Y to go back to the menu or N to exit.");
                ActivityLog.Write($"nlp: thanks detected from {_username}");
                return;
            }

            //route to the handler that matches the current conversation stage
            switch (_state)
            {
                case BotState.AwaitingName:       HandleName(input);              break;
                case BotState.AwaitingMenu:       HandleMenuChoice(input);        break;
                case BotState.AwaitingKeyword:    HandleKeyword(input, nlp);      break;
                case BotState.AwaitingTaskMenu:   HandleTaskMenu(input);          break;
                case BotState.AwaitingTaskTitle:  HandleTaskTitle(input);         break;
                case BotState.AwaitingTaskDesc:   HandleTaskDesc(input);          break;
                case BotState.AwaitingTaskRem:    HandleTaskRem(input);           break;
                case BotState.AwaitingTaskRemVal: HandleTaskRemVal(input);        break;
                case BotState.AwaitingTaskMgmt:   HandleTaskManagement(input);    break;
                case BotState.AwaitingQuizAnswer: HandleQuizAnswer(input);        break;
                case BotState.AwaitingContinue:   HandleContinue(input);          break;
                //BotState.Done is intentionally absent - input is disabled there
            }
        }

        // ── name handler ──────────────────────────────────────────────────────
        private void HandleName(string input)
        {
            _username = input;
            UserSay(input);
            BotSay($"Hello, {_username}! {ChatBotData.GetRandomGreeting()}");
            ActivityLog.Write($"session started - user: {_username}");
            Divider();
            ShowMenu();
        }

        // ── main menu handler ─────────────────────────────────────────────────
        private void HandleMenuChoice(string input)
        {
            UserSay(input);

            //reject anything that isn't a number in the valid range
            if (!int.TryParse(input, out int choice) || choice < 1 || choice > 4)
            {
                BotSay("Please enter a number between 1 and 4.");
                return;
            }

            ActivityLog.Write($"{_username} selected menu option {choice}");

            switch (choice)
            {
                case 1: //search by keyword - ask the user what they want to know about
                    BotSay("Type a keyword or question and I'll find information on it.\n" +
                           "Try: vpn, malware, firewall, backup, phishing, scam,\n" +
                           "two factor, encryption, identity theft, social engineering...");
                    _state = BotState.AwaitingKeyword;
                    return;

                case 2: //open the task assistant / reminder feature
                    ShowTaskMenu();
                    return;

                case 3: //start the mini quiz
                    StartQuiz();
                    return;

                case 4: //exit the application
                    BotSay($"Goodbye, {_username}! Stay safe online. 🔒");
                    ActivityLog.WriteSessionEnd(_username);
                    Divider();
                    EndSession();
                    return;
            }
        }

        // ── keyword handler (option 1, with nlp tone detection) ───────────────
        private void HandleKeyword(string input, NlpResult nlp)
        {
            UserSay(input);

            //if the nlp detected worry or urgency, show a supportive prefix first
            if (nlp.Prefix != null)
            {
                BotSay(nlp.Prefix);
                ActivityLog.Write($"nlp: sentiment '{nlp.Sentiment}' detected in: {input}");
            }

            string? result = ChatBotData.FindKeywordResponse(input);

            if (result != null)
            {
                BotSay(result);
                ActivityLog.Write($"keyword match: {input}");
            }
            else
            {
                BotSay($"Sorry {_username}, I don't have a specific answer for that yet.");
                BotSay("Try keywords like: vpn, malware, firewall, backup, phishing, scam,\n" +
                       "two factor, encryption, identity theft, social engineering, and more.");
                ActivityLog.Write($"no keyword match for: {input}");
            }

            Divider();
            AskContinue();
        }

        // ═════════════════════════════════════════════════════════════════════
        //  FEATURE 1 — TASK ASSISTANT
        //  users can add tasks with a title, description, and optional reminder.
        //  tasks are saved to a sqlite database file (cybertasks.db) and
        //  persist between sessions. tasks can be marked done or deleted.
        // ═════════════════════════════════════════════════════════════════════

        //shows the task assistant sub-menu and waits for A/V/B
        private void ShowTaskMenu()
        {
            ActivityLog.Write($"{_username} opened task assistant");
            BotSay("── TASK ASSISTANT ──────────────────────────────────────\n" +
                   "  A) Add a new cybersecurity task\n" +
                   "  V) View all my tasks\n" +
                   "  B) Back to main menu\n\n" +
                   "Choose an option (A / V / B):");
            _state = BotState.AwaitingTaskMenu;
        }

        //handles the A/V/B choices in the task sub-menu
        private void HandleTaskMenu(string input)
        {
            UserSay(input);

            switch (input.Trim().ToUpper())
            {
                case "A": //start the add-task flow
                    BotSay("What is the title of your task?\n" +
                           "Example: 'Enable two-factor authentication'");
                    _state = BotState.AwaitingTaskTitle;
                    break;

                case "V": //show the task list
                    ShowTaskList();
                    break;

                case "B": //go back to the main menu
                    Divider();
                    ShowMenu();
                    break;

                default:
                    BotSay("Please type A to add a task, V to view tasks, or B to go back.");
                    break;
            }
        }

        //step 1 of adding a task: save the title and ask for the description
        private void HandleTaskTitle(string input)
        {
            UserSay(input);
            _pendingTitle = input;
            BotSay("Got it! Now add a short description of what needs to be done.\n" +
                   "Example: 'Go to Settings → Security and turn on 2FA for all accounts'");
            _state = BotState.AwaitingTaskDesc;
        }

        //step 2: save the description and ask if they want a reminder
        private void HandleTaskDesc(string input)
        {
            UserSay(input);
            _pendingDesc = input;
            BotSay("Would you like to set a reminder for this task? (Y / N)\n" +
                   "Reminder examples: 'in 3 days', 'by Friday', '2025-07-01'");
            _state = BotState.AwaitingTaskRem;
        }

        //step 3: if yes, ask for the reminder text; if no, save straight away
        private void HandleTaskRem(string input)
        {
            UserSay(input);

            if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                BotSay("When would you like to be reminded?\n" +
                       "Examples: 'in 3 days', 'by end of week', '2025-07-01'");
                _state = BotState.AwaitingTaskRemVal;
            }
            else
            {
                //no reminder - save the task immediately with no reminder value
                FinaliseAndSaveTask(null);
            }
        }

        //step 4 (optional): capture the reminder text and save
        private void HandleTaskRemVal(string input)
        {
            UserSay(input);
            FinaliseAndSaveTask(input); //pass reminder text through to the save method
        }

        //writes the completed task to the database and confirms to the user
        private void FinaliseAndSaveTask(string? reminder)
        {
            int id = TaskManager.AddTask(_pendingTitle, _pendingDesc, reminder);

            string msg = $"Task saved! (ID: {id})\n" +
                         $"  Title:       {_pendingTitle}\n" +
                         $"  Description: {_pendingDesc}";
            if (reminder != null)
                msg += $"\n  Reminder:    {reminder}";

            BotSay(msg);
            ActivityLog.Write($"task added [{id}] '{_pendingTitle}' by {_username}" +
                              (reminder != null ? $" - reminder: {reminder}" : ""));

            //clear the temporary fields so they're ready for the next task
            _pendingTitle = "";
            _pendingDesc  = "";

            Divider();
            ShowTaskMenu(); //go back to the task menu so they can add more or view
        }

        //displays all saved tasks and waits for a DONE/DEL/B command
        private void ShowTaskList()
        {
            var tasks = TaskManager.GetAllTasks();
            ActivityLog.Write($"{_username} viewed task list ({tasks.Count} task(s))");

            if (tasks.Count == 0)
            {
                BotSay("You have no tasks yet. Use option A to add one!");
                Divider();
                ShowTaskMenu();
                return;
            }

            //build one big string so the chat doesn't get flooded with separate messages
            string list = "── YOUR CYBERSECURITY TASKS ────────────────────────────\n";
            foreach (var t in tasks)
            {
                string status = t.IsComplete ? "[DONE] " : "[TODO] ";
                list += $"\n  {status}ID {t.Id}: {t.Title}\n";
                list += $"         {t.Description}\n";
                if (t.Reminder != null)
                    list += $"         Reminder: {t.Reminder}\n";
                list += $"         Added: {t.AddedOn}\n";
            }

            BotSay(list);
            BotSay("Commands:\n" +
                   "  DONE <id>  — mark a task as complete  (example: DONE 2)\n" +
                   "  DEL  <id>  — permanently delete a task (example: DEL 3)\n" +
                   "  B          — back to task menu");

            _state = BotState.AwaitingTaskMgmt;
        }

        //handles DONE/DEL/B commands when the task list is visible
        private void HandleTaskManagement(string input)
        {
            UserSay(input);
            string upper = input.Trim().ToUpper();

            if (upper == "B")
            {
                //go back to the task sub-menu
                Divider();
                ShowTaskMenu();
                return;
            }

            if (upper.StartsWith("DONE "))
            {
                //extract the number after "DONE "
                string idStr = input.Substring(5).Trim();
                if (int.TryParse(idStr, out int id))
                {
                    if (TaskManager.MarkComplete(id))
                    {
                        BotSay($"Task {id} marked as complete! Well done, {_username}. 🔒");
                        ActivityLog.Write($"{_username} marked task {id} as complete");
                    }
                    else
                    {
                        BotSay($"No task found with ID {id}. Type V to see your tasks.");
                    }
                }
                else
                {
                    BotSay("Usage: DONE <number>  — example: DONE 2");
                }
                return;
            }

            if (upper.StartsWith("DEL "))
            {
                //extract the number after "DEL "
                string idStr = input.Substring(4).Trim();
                if (int.TryParse(idStr, out int id))
                {
                    if (TaskManager.DeleteTask(id))
                    {
                        BotSay($"Task {id} deleted.");
                        ActivityLog.Write($"{_username} deleted task {id}");
                    }
                    else
                    {
                        BotSay($"No task found with ID {id}. Type V to see your tasks.");
                    }
                }
                else
                {
                    BotSay("Usage: DEL <number>  — example: DEL 3");
                }
                return;
            }

            //if we get here the input was not recognised
            BotSay("Type DONE <id>, DEL <id>, or B to go back.\n" +
                   "Example: DONE 2  or  DEL 3");
        }

        // ═════════════════════════════════════════════════════════════════════
        //  FEATURE 2 — MINI QUIZ
        //  picks 5 random cybersecurity questions each session.
        //  the user types A/B/C/D and gets immediate feedback with an
        //  explanation after each answer. a score is shown at the end.
        // ═════════════════════════════════════════════════════════════════════

        //sets up a fresh quiz session with 5 random questions
        private void StartQuiz()
        {
            ActivityLog.Write($"{_username} started the mini quiz");
            _quizQuestions = QuizData.GetRandomQuestions(5);
            _quizIndex     = 0;
            _quizScore     = 0;

            BotSay($"── CYBERSECURITY MINI QUIZ ──────────────────────────────\n" +
                   $"5 questions. Type A, B, C, or D to answer each one.\n" +
                   $"Good luck, {_username}!");
            Divider();
            ShowCurrentQuestion();
        }

        //prints the question and all four options for the current question index
        private void ShowCurrentQuestion()
        {
            var q = _quizQuestions[_quizIndex];
            BotSay($"Question {_quizIndex + 1} of {_quizQuestions.Count}:\n\n" +
                   $"  {q.Question}\n\n" +
                   $"  {q.Options[0]}\n" +
                   $"  {q.Options[1]}\n" +
                   $"  {q.Options[2]}\n" +
                   $"  {q.Options[3]}");
            _state = BotState.AwaitingQuizAnswer;
        }

        //called when the user types their answer letter
        private void HandleQuizAnswer(string input)
        {
            UserSay(input);

            //map the letter to a 0-based index; -1 means invalid input
            int answer = input.Trim().ToUpper() switch
            {
                "A" => 0, "B" => 1, "C" => 2, "D" => 3,
                _   => -1
            };

            if (answer == -1)
            {
                BotSay("Please type A, B, C, or D.");
                return; //stay on the same question and wait again
            }

            var q = _quizQuestions[_quizIndex];

            if (answer == q.CorrectIndex)
            {
                //correct - show green-ish label and the explanation
                AppendColouredLine("✔  Correct!", ColourBot);
                BotSay(q.Explanation);
                _quizScore++;
                ActivityLog.Write($"quiz Q{_quizIndex + 1}: {_username} correct");
            }
            else
            {
                //wrong - show which option was right and the explanation
                string correctLetter = q.CorrectIndex switch { 0 => "A", 1 => "B", 2 => "C", _ => "D" };
                AppendColouredLine($"✘  Incorrect. The correct answer was {correctLetter}.", ColourGold);
                BotSay(q.Explanation);
                ActivityLog.Write($"quiz Q{_quizIndex + 1}: {_username} incorrect");
            }

            _quizIndex++;
            Divider();

            if (_quizIndex < _quizQuestions.Count)
            {
                ShowCurrentQuestion(); //move to the next question
            }
            else
            {
                ShowQuizResults(); //all questions answered - show the final score
            }
        }

        //shows the final score with a verdict message based on how well they did
        private void ShowQuizResults()
        {
            int total = _quizQuestions.Count;

            string verdict = _quizScore switch
            {
                5     => $"Perfect score! You're a cybersecurity expert, {_username}! 🏆",
                4     => $"Excellent work! {_quizScore}/{total} — you clearly know your stuff.",
                3     => $"Good effort! {_quizScore}/{total} — review the topics you missed.",
                2     => $"Keep learning! {_quizScore}/{total} — use the menu to brush up.",
                1     => $"Don't worry, {_username}! {_quizScore}/{total} — practice makes perfect.",
                _     => $"Keep trying! {_quizScore}/{total} — the menu topics can help you improve."
            };

            BotSay($"── QUIZ COMPLETE ────────────────────────────────────────\n" +
                   $"Final score: {_quizScore} / {total}\n\n{verdict}");

            ActivityLog.Write($"{_username} finished quiz: {_quizScore}/{total}");
            AskContinue();
        }

        // ═════════════════════════════════════════════════════════════════════
        //  FEATURE 3 — NLP SIMULATION
        //  nlp runs on every message in SendMessage() via NlpHelper.Analyse().
        //  the result is passed into HandleKeyword() to prepend a tone-aware
        //  prefix, and also intercepts greetings and thanks mid-session above.
        //  see NlpHelper.cs for the full keyword lists and logic.
        // ═════════════════════════════════════════════════════════════════════

        // ═════════════════════════════════════════════════════════════════════
        //  FEATURE 4 — ACTIVITY LOG
        //  every meaningful action calls ActivityLog.Write() with a short
        //  description. the log is appended to activity_log.txt next to the
        //  app. session boundaries are marked with header/footer lines.
        //  see ActivityLog.cs for the implementation.
        // ═════════════════════════════════════════════════════════════════════

        // ── continue / end session ────────────────────────────────────────────
        private void HandleContinue(string input)
        {
            UserSay(input);

            if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Divider();
                ShowMenu();
            }
            else
            {
                BotSay($"Goodbye, {_username}! Stay safe online. 🔒");
                BotSay($"Your activity log is saved at:\n  {ActivityLog.GetLogPath()}");
                ActivityLog.WriteSessionEnd(_username);
                Divider();
                EndSession();
            }
        }

        // ── shared helpers ────────────────────────────────────────────────────
        private void ShowMenu()
        {
            BotSay("==== MENU ====\n" +
                   "  1. Search by keyword\n" +
                   "  2. Task Assistant  ← manage your cybersecurity to-do list\n" +
                   "  3. Mini Quiz       ← test your cybersecurity knowledge\n" +
                   "  4. Exit\n\n" +
                   "Choose an option (1–4):");
            _state = BotState.AwaitingMenu;
        }

        private void AskContinue()
        {
            BotSay($"Do you have more questions, {_username}? (Y / N)");
            _state = BotState.AwaitingContinue;
        }

        private void EndSession()
        {
            _state               = BotState.Done;
            txtMessage.IsEnabled = false;
            btnSend.IsEnabled    = false;
        }

        // ── chat rendering ────────────────────────────────────────────────────
        private void BotSay(string text)
        {
            AppendMessage("[CyberBot] ", ColourBot, text, ColourBody);
        }

        private void UserSay(string text)
        {
            string label = string.IsNullOrEmpty(_username) ? "You" : _username;
            AppendMessage($"[{label}] ", ColourUser, text, ColourBody);
        }

        private void Divider()
        {
            var para = new Paragraph(new Run("──────────────────────────────────────────"))
            {
                Foreground = ColourDivider,
                FontFamily = new FontFamily("Consolas"),
                FontSize   = 13,
                Margin     = new Thickness(0, 2, 0, 2)
            };
            txtChat.Document.Blocks.Add(para);
            txtChat.ScrollToEnd();
        }

        //renders one chat line with a coloured bold label and a grey body
        private void AppendMessage(string label, Brush labelBrush, string body, Brush bodyBrush)
        {
            var para = new Paragraph
            {
                FontFamily = new FontFamily("Consolas"),
                FontSize   = 13,
                Margin     = new Thickness(0, 4, 0, 0)
            };
            para.Inlines.Add(new Run(label) { Foreground = labelBrush, FontWeight = FontWeights.Bold });
            para.Inlines.Add(new Run(body)  { Foreground = bodyBrush });
            txtChat.Document.Blocks.Add(para);
            txtChat.ScrollToEnd();
        }

        //renders a short standalone line in a specific colour (used for quiz feedback)
        private void AppendColouredLine(string text, Brush colour)
        {
            var para = new Paragraph(new Run(text))
            {
                Foreground = colour,
                FontFamily = new FontFamily("Consolas"),
                FontSize   = 13,
                FontWeight = FontWeights.Bold,
                Margin     = new Thickness(0, 4, 0, 0)
            };
            txtChat.Document.Blocks.Add(para);
            txtChat.ScrollToEnd();
        }
    }
}
