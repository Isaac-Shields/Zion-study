using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Mono.Data.Sqlite;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class DatabaseHelper : MonoBehaviour
{
    private string DatabaseName = "database.db";
    private string dbPath;
    private static string usersTable = "users";
    private static string notesTable = "notes";
    private static string cardsTable = "cards";
    private static string problemsTable = "problems";
    private static string commentsTable = "comments";

    void Start ()
    {
        dbPath = Path.Combine(Application.persistentDataPath, DatabaseName);
        CreateDB();
    }


    private void CreateDB()
    {
        if(!File.Exists(dbPath))
        {
            SqliteConnection.CreateFile(dbPath);
        }
        string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
        using (var connection = new SqliteConnection(connectionString))
        {
            Debug.Log("connected");
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"CREATE TABLE IF NOT EXISTS {usersTable} (userId INTEGER PRIMARY KEY AUTOINCREMENT, username VARCHAR(25) NOT NULL, password VARCHAR(20), isAdmin BOOLEAN);";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {notesTable} (title VARCHAR(50), body VARCHAR(720), userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {cardsTable} (setId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(20), isPublic BOOLEAN, commentsId INTEGER, userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {problemsTable} (problem VARCHAR(30), answer VARCHAR(30), setId INTEGER, FOREIGN KEY (setId) REFERENCES {cardsTable}(setId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {commentsTable} (commentsId INTEGER, title VARCHAR(20), body VARCHAR(250), FOREIGN KEY (commentsId) REFERENCES {problemsTable}(setId));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }


    public bool checkUsername(string username)
    {
        bool validUsername = false;
        string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT count(*) FROM {usersTable} WHERE username = @username";

            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);

                validUsername = Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        return validUsername;
    }


}
