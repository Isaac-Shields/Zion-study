using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using Mono.Data.Sqlite;
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
                command.CommandText = $@"CREATE TABLE IF NOT EXISTS {usersTable} (userId INTEGER PRIMARY KEY AUTOINCREMENT, username VARCHAR(25) NOT NULL, password VARCHAR(20), isAdmin INTEGER);";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {notesTable} (title VARCHAR(50), body VARCHAR(720), userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {cardsTable} (setId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(20), isPublic INTEGER, commentsId INTEGER, userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {problemsTable} (problem VARCHAR(30), answer VARCHAR(30), setId INTEGER, FOREIGN KEY (setId) REFERENCES {cardsTable}(setId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {commentsTable} (commentsId INTEGER, title VARCHAR(20), body VARCHAR(250), FOREIGN KEY (commentsId) REFERENCES {problemsTable}(setId));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }


    public bool newUserName(string username)
    {
        bool validUsername = true;
        string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (SqliteCommand command = new SqliteCommand(connection))
            {
                command.CommandText = $"SELECT count(*) FROM {usersTable} WHERE username = '{username}'";
                int count = Convert.ToInt32(command.ExecuteScalar());
                if(count > 0)
                {
                    validUsername = false;
                }
            }
        }

        return validUsername;
    }

    public bool addNewUser(string usernameFS, string password)
    {
        bool userAdded = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"INSERT INTO {usersTable} (username, password, isAdmin) VALUES ('{usernameFS}', '{password}', 'FALSE');";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            userAdded = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Error inserting user: " + ex.Message);
        }

        return userAdded;
    }

    public bool validCredentials(string usernameSent, string passwordSent)
    {
        bool validLoginDetails = false;
        string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using(SqliteCommand cmd = new SqliteCommand(connection))
            {
                cmd.CommandText = $"SELECT COUNT(*) FROM {usersTable} WHERE username = '{usernameSent}' AND password = '{passwordSent}';";

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if(count > 0)
                {
                    validLoginDetails = true;
                }

            }
        }

        return validLoginDetails;

    }


}
