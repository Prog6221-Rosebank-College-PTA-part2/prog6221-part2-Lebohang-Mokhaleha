using System;
using System.Collections.Generic;

namespace CyberSecurityChatBotWithUI
{
    //holds a single quiz question with four answer choices and the index of the correct one
    public class QuizQuestion
    {
        public string   Question      { get; set; } = "";
        public string[] Options       { get; set; } = Array.Empty<string>(); //always exactly 4 options
        public int      CorrectIndex  { get; set; }                          //0-based index into Options
        public string   Explanation   { get; set; } = "";                    //shown after the user answers
    }

    //contains the full question bank and helper methods used by the quiz feature
    static class QuizData
    {
        //all questions are stored here - the quiz picks a random subset each time it runs
        public static readonly List<QuizQuestion> Questions = new List<QuizQuestion>
        {
            new QuizQuestion
            {
                Question     = "What does 'HTTPS' stand for?",
                Options      = new[]
                {
                    "A) HyperText Transfer Protocol Secure",
                    "B) High-Traffic Transmission Protocol System",
                    "C) HyperText Transfer Protocol Standard",
                    "D) Home Transfer Protocol Secure"
                },
                CorrectIndex = 0,
                Explanation  = "HTTPS uses TLS encryption to protect data between your browser and the server."
            },
            new QuizQuestion
            {
                Question     = "Which of these is the SAFEST type of password?",
                Options      = new[]
                {
                    "A) password123",
                    "B) Your date of birth",
                    "C) A short random word like 'cat'",
                    "D) A long passphrase like 'Coffee@Sunrise7Rings!'"
                },
                CorrectIndex = 3,
                Explanation  = "Long passphrases are both strong and memorable. Length beats complexity."
            },
            new QuizQuestion
            {
                Question     = "What is phishing?",
                Options      = new[]
                {
                    "A) A water sport involving nets",
                    "B) Tricking people into revealing sensitive info via fake messages",
                    "C) A type of antivirus scan",
                    "D) Encrypting your files for ransom"
                },
                CorrectIndex = 1,
                Explanation  = "Phishing uses fake emails or websites to steal passwords, card numbers, and more."
            },
            new QuizQuestion
            {
                Question     = "What does Two-Factor Authentication (2FA) add to your login?",
                Options      = new[]
                {
                    "A) A second, longer password",
                    "B) A firewall around your account",
                    "C) A second verification step beyond your password",
                    "D) An automatic password reset"
                },
                CorrectIndex = 2,
                Explanation  = "2FA means even if your password is stolen, the attacker still can't get in."
            },
            new QuizQuestion
            {
                Question     = "What should you do if you receive an unexpected email with a link?",
                Options      = new[]
                {
                    "A) Click it immediately to see what it is",
                    "B) Forward it to all your contacts",
                    "C) Hover over the link to check the real URL before clicking",
                    "D) Reply asking what the link is for"
                },
                CorrectIndex = 2,
                Explanation  = "Hovering reveals the true destination URL without actually visiting it."
            },
            new QuizQuestion
            {
                Question     = "What is ransomware?",
                Options      = new[]
                {
                    "A) Software that speeds up your PC",
                    "B) Malware that encrypts your files and demands payment to restore them",
                    "C) A type of VPN",
                    "D) A browser extension that blocks ads"
                },
                CorrectIndex = 1,
                Explanation  = "Never pay the ransom — there is no guarantee your files will be restored."
            },
            new QuizQuestion
            {
                Question     = "Why should you avoid reusing the same password across multiple sites?",
                Options      = new[]
                {
                    "A) It makes it harder to remember",
                    "B) It slows down your browser",
                    "C) One breach exposes ALL accounts that share that password",
                    "D) Websites don't allow duplicate passwords"
                },
                CorrectIndex = 2,
                Explanation  = "This attack is called credential stuffing — hackers try leaked passwords on other sites."
            },
            new QuizQuestion
            {
                Question     = "Which of these is a sign that a website is NOT secure?",
                Options      = new[]
                {
                    "A) It starts with 'https://'",
                    "B) It has a padlock icon in the browser bar",
                    "C) The browser shows a 'Not Secure' warning",
                    "D) It loads quickly"
                },
                CorrectIndex = 2,
                Explanation  = "HTTP (no 'S') means the connection is unencrypted — never enter sensitive info."
            },
            new QuizQuestion
            {
                Question     = "What is a VPN mainly used for?",
                Options      = new[]
                {
                    "A) Making your internet faster",
                    "B) Blocking all ads and pop-ups",
                    "C) Encrypting your traffic and masking your IP address",
                    "D) Scanning for viruses"
                },
                CorrectIndex = 2,
                Explanation  = "A VPN is especially useful on public Wi-Fi where traffic can be intercepted."
            },
            new QuizQuestion
            {
                Question     = "What is social engineering?",
                Options      = new[]
                {
                    "A) Building apps for social media",
                    "B) Manipulating people into revealing confidential information",
                    "C) A type of network firewall",
                    "D) Encrypting your social media messages"
                },
                CorrectIndex = 1,
                Explanation  = "Social engineers exploit human psychology rather than technical vulnerabilities."
            }
        };

        //returns a shuffled subset of questions for one quiz session
        //using 5 questions keeps the quiz quick but still meaningful
        public static List<QuizQuestion> GetRandomQuestions(int count = 5)
        {
            var shuffled = new List<QuizQuestion>(Questions);

            //fisher-yates shuffle - fair random order every time
            var rng = new Random();
            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }

            //clamp count so we never ask for more questions than exist
            return shuffled.GetRange(0, Math.Min(count, shuffled.Count));
        }
    }
}
