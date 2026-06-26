using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace CyberSecurityChatBotWithUI
{
    //represents a single cybersecurity task the user has added
    public class CyberTask
    {
        public int     Id          { get; set; }
        public string  Title       { get; set; } = "";
        public string  Description { get; set; } = "";
        public string? Reminder    { get; set; }   //optional - e.g. "in 3 days" or a specific date
        public bool    IsComplete  { get; set; }
        public string  AddedOn     { get; set; } = "";
    }

    //handles all database reads and writes for the task assistant feature
    //uses sqlite so no external server is needed - the db lives as a single file next to the app
    static class TaskManager
    {
        //path to the sqlite database file - created automatically on first run
        private const string DbPath = "cybertasks.db";

        //connection string sqlite uses to find and open the database file
        private static string ConnectionString => $"Data Source={DbPath}";

        //called once at startup to make sure the tasks table exists
        //if the file is brand new, this creates it; if it already exists, nothing changes
        public static void Initialise()
        {
            using var con = new SqliteConnection(ConnectionString);
            con.Open();

            //create the table only if it doesn't already exist
            using var cmd = con.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Tasks (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title       TEXT    NOT NULL,
                    Description TEXT    NOT NULL,
                    Reminder    TEXT,
                    IsComplete  INTEGER NOT NULL DEFAULT 0,
                    AddedOn     TEXT    NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        //saves a new task to the database and returns the auto-assigned id
        public static int AddTask(string title, string description, string? reminder)
        {
            using var con = new SqliteConnection(ConnectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText =
                @"INSERT INTO Tasks (Title, Description, Reminder, IsComplete, AddedOn)
                  VALUES ($title, $desc, $rem, 0, $date);
                  SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$title", title);
            cmd.Parameters.AddWithValue("$desc",  description);
            cmd.Parameters.AddWithValue("$rem",   reminder ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$date",  DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            //last_insert_rowid() returns the id sqlite assigned to the new row
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        //loads every task from the database and returns them as a list
        public static List<CyberTask> GetAllTasks()
        {
            var tasks = new List<CyberTask>();

            using var con = new SqliteConnection(ConnectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Description, Reminder, IsComplete, AddedOn FROM Tasks ORDER BY Id;";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    Id          = reader.GetInt32(0),
                    Title       = reader.GetString(1),
                    Description = reader.GetString(2),
                    Reminder    = reader.IsDBNull(3) ? null : reader.GetString(3),
                    IsComplete  = reader.GetInt32(4) == 1,
                    AddedOn     = reader.GetString(5)
                });
            }

            return tasks;
        }

        //flips a task's IsComplete flag to true (1) in the database
        public static bool MarkComplete(int id)
        {
            using var con = new SqliteConnection(ConnectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE Tasks SET IsComplete = 1 WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);

            //ExecuteNonQuery returns how many rows were changed - 0 means the id wasn't found
            return cmd.ExecuteNonQuery() > 0;
        }

        //permanently removes a task row from the database by its id
        public static bool DeleteTask(int id)
        {
            using var con = new SqliteConnection(ConnectionString);
            con.Open();

            using var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM Tasks WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
