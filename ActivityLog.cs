using System;
using System.IO;

namespace CyberSecurityChatBotWithUI
{
    //writes a record of every action the bot takes to a plain-text log file
    //this gives users (and developers) a full audit trail of each session
    static class ActivityLog
    {
        //the log file sits next to the app executable
        //each run appends to the same file so history is preserved across sessions
        private const string LogFile = "activity_log.txt";

        //writes a single timestamped entry to the log
        //called every time the bot or user does something meaningful
        public static void Write(string entry)
        {
            try
            {
                //format: [2025-06-01 14:32:05]  entry text
                string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]  {entry}";
                File.AppendAllText(LogFile, line + Environment.NewLine);
            }
            catch
            {
                //silently ignore write errors - the chat should never crash because of logging
            }
        }

        //writes a blank divider line between sessions so the log is easy to read
        public static void WriteSessionStart()
        {
            Write("══════════════════════  SESSION STARTED  ══════════════════════");
        }

        //marks the end of a session in the log file
        public static void WriteSessionEnd(string username)
        {
            Write($"session ended for user: {username}");
            Write("══════════════════════  SESSION ENDED    ══════════════════════");
            Write(""); //blank line for spacing between sessions
        }

        //returns the full path to the log file so the UI can tell the user where to find it
        public static string GetLogPath() => Path.GetFullPath(LogFile);
    }
}
