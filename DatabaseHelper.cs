using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace ChatBotApp
{
    public static class DatabaseHelper
    {
        // Replace with your actual MySQL Server connection parameters
        private static string connectionString = "server=localhost;database=cyber_tasks;user=root;password=yourpassword;";

        public static void InitializeDatabase()
        {
            // Connect to the server without specifying a database first
            string setupConnectionString = "server=127.0.0.1;port=3306;user=root;password=;";

            using (var conn = new MySqlConnection(setupConnectionString))
            {
                conn.Open();

                // 1. Create the database if it doesn't exist yet
                string createDbQuery = "CREATE DATABASE IF NOT EXISTS cyber_tasks;";
                using (var cmd = new MySqlCommand(createDbQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Now that we are sure the DB exists, run your original table creation code
            using (var conn = new MySqlConnection("server=127.0.0.1;port=3306;database=cyber_tasks;user=root;password=;"))
            {
                conn.Open();
                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS tasks (
                id INT AUTO_INCREMENT PRIMARY KEY,
                title VARCHAR(255) NOT NULL,
                description TEXT,
                reminder_days INT DEFAULT NULL,
                is_completed BOOLEAN DEFAULT FALSE
            );";
                using (var cmd = new MySqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static string AddTask(string title, string description, int? reminderDays)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO tasks (title, description, reminder_days) VALUES (@title, @desc, @reminder);";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@desc", description);
                        cmd.Parameters.AddWithValue("@reminder", (object)reminderDays ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                return $"✅ Task added successfully: '{title}'";
            }
            catch (Exception ex)
            {
                return $"❌ Database error: {ex.Message}";
            }
        }

        public static string ViewTasks()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, title, description, reminder_days, is_completed FROM tasks WHERE is_completed = FALSE;";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        sb.AppendLine("📋 Here are your active cybersecurity tasks:");
                        bool hasTasks = false;
                        while (reader.Read())
                        {
                            hasTasks = true;
                            string reminderInfo = reader.IsDBNull(3) ? "No reminder set" : $"Remind me in {reader.GetInt32(3)} days";
                            sb.AppendLine($"\n🔹 [ID: {reader.GetInt32(0)}] {reader.GetString(1)}");
                            sb.AppendLine($"   Description: {reader.GetString(2)}");
                            sb.AppendLine($"   Reminder: {reminderInfo}");
                        }
                        if (!hasTasks) return "🎉 You have no pending cybersecurity tasks!";
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"❌ Database error: {ex.Message}";
            }
        }

        public static string DeleteTask(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tasks WHERE id = @id;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0) return $"✅ Task ID {id} deleted successfully.";
                        else return $"⚠️ No task found with ID {id}.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"❌ Database error: {ex.Message}";
            }
        }
    }
}