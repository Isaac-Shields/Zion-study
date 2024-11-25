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
    public static string calculatorTable = "calculators";
    public MasterScript master;

    void Start ()
    {
        dbPath = Path.Combine(Application.persistentDataPath, DatabaseName);
        CreateDB();
    }


    //Create the database
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

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {notesTable} (noteId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(128), body VARCHAR(512), userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {cardsTable} (setId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(20), isPublic INTEGER, commentsId INTEGER, userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {problemsTable} (problemId INTEGER PRIMARY KEY AUTOINCREMENT, problem VARCHAR(30), answer VARCHAR(30), setId INTEGER, FOREIGN KEY (setId) REFERENCES {cardsTable}(setId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {commentsTable} (commentsId INTEGER, title VARCHAR(20), body VARCHAR(250), FOREIGN KEY (commentsId) REFERENCES {problemsTable}(setId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {calculatorTable} (calcId INTEGER PRIMARY KEY AUTOINCREMENT, calcName VARCHAR(120), isVisible INTEGER, calcUid INTEGER NOT NULL, calcDesc VARCHAR(512));";
                command.ExecuteNonQuery();

            }

            connection.Close();
        }
    }


    //Checks if the username entered is valid (unique)
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

            connection.Close();
        }

        return validUsername;
    }

    //Adds a new user to the database
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

    //Get userId
    public int getSessionData(string username, string password)
    {
        int userId = -1;
        if(validCredentials(username, password))
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT userId FROM {usersTable} WHERE username = '{username}' AND password = '{password}';";
                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        userId = Int32.Parse(reader["userId"].ToString());
                    }
                }

                connection.Close();
            }
        }
        return userId;
    }

    //Checks if the username and password are correct
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

            connection.Close();
        }

        return validLoginDetails;

    }

    //Adds a note to the database
    public bool addNoteToDatabase(string title, string body, int userId)
    {
        bool validOperation = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"INSERT INTO {notesTable} (title, body, userId) VALUES ('{title}', '{body}', '{userId}');";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

            validOperation = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Error: " + " Line 186, " + ex.Message);
        }

        return validOperation;
    }

    //Returns a list of all the notes belonging to a user
    public List<notesObj> getAllNotesFromDatabase(int uid)
    {
        List<notesObj> notes = new List<notesObj>();
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                    connection.Open();

                    using(SqliteCommand cmd = new SqliteCommand(connection))
                    {
                        cmd.CommandText = $"SELECT * FROM {notesTable} WHERE userId = '{uid}' ;";

                        using(SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notesObj curnote = new notesObj();
                                curnote.setNoteTitle(reader["title"].ToString());
                                curnote.setNoteBody(reader["body"].ToString());
                                curnote.setNoteId(Convert.ToInt32(reader["noteId"].ToString()));
                                notes.Add(curnote);
                            }

                            reader.Close();
                        }


                    }

                    connection.Close();
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Error: " + " Line 186, " + ex.Message);
        }

        return notes;
    }

    //Updates an existing note
    public bool updateNotes(string t, string b, int nid)
    {
        bool success = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"UPDATE {notesTable} SET title = '{t}', body = '{b}' WHERE noteId = {nid};";
                    cmd.ExecuteNonQuery();
                    success = true;
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error updating note on line 241: " + ex.Message);
        }

        return success;
    }

    //Deletes a not from the database
    public bool deleteNote(int nid)
    {
        bool success = false;
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"DELETE FROM {notesTable} WHERE noteId = '{nid}';";
                    cmd.ExecuteNonQuery();
                    success = true;
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error updating note on line 241: " + ex.Message);
        }

        return success;
    }

    public bool createCardset(string title, int uid)
    {
        bool status = false;
        //create a cardset

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"INSERT INTO {cardsTable} (title, userId, isPublic) VALUES ('{title}', '{uid}', '0');";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                status = true;
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error adding card: " + ex.Message);
        }

        return status;
    }

    public bool addCardToCardset(string problem, string answer, int cid)
    {
        bool status = false;
        //add a card to the cardset

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"INSERT INTO {problemsTable} (problem, answer, setId) VALUES ('{problem}', '{answer}', '{cid}');";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                status = true;
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error adding card: " + ex.Message);
        }

        return status;
    }

    public int getCardsetId(string title)
    {
        int setId = -1;
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {cardsTable} WHERE title = '{title}' ;";
                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            setId = Int32.Parse(reader["setId"].ToString());
                        }

                        reader.Close();
                    }
                }
                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error adding card: " + ex.Message);
        }

        if(setId != -1)
        {
            master.curCard.setId(setId);
        }
        else
        {
            Debug.Log("Error reading set ID");
        }

        return setId;
    }

    public List<string> getPrivateCardsets(int uid)
    {
        List<string> allTitles = new List<string>();

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {cardsTable} WHERE userId = '{uid}';";

                        using(SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allTitles.Add(reader["title"].ToString());
                            }

                            reader.Close();
                        }
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error getting cardsets: " + ex.Message);
        }

        return allTitles;
    }

    public List<string> getPublicCardsets()
    {
        List<string> allTitles = new List<string>();

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {cardsTable} WHERE isPublic = '1';";

                        using(SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allTitles.Add(reader["title"].ToString());
                            }

                            reader.Close();
                        }
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error getting cardsets: " + ex.Message);
        }

        return allTitles;
    }

    public List<problemObj> getAllProblems(string title)
    {
        List<problemObj> allProblems = new List<problemObj>();

        try
        {
            int setId = getCardsetId(title);
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {problemsTable} WHERE setId = '{setId}' ;";

                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            problemObj problem = new problemObj();

                            problem.setProblem(reader["problem"].ToString());
                            problem.setAnswer(reader["answer"].ToString());
                            problem.setId(Int32.Parse(reader["problemId"].ToString()));

                            allProblems.Add(problem);
                        }

                        reader.Close();
                    }
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error getting problems: " + ex.Message);
        }

        return allProblems;
    }

    public bool updateProblemSet(int pid, string p, string a, string t, int cid)
    {
        bool allGood = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"UPDATE {problemsTable} SET problem = '{p}', answer = '{a}' WHERE problemId = {pid};";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"UPDATE {cardsTable} SET title = '{t}' WHERE setId = {cid};";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                    
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error updating problem: " + ex.Message);
        }

        return allGood;
    }

    public bool dropProblemsTable()
    {
        bool allGood = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"DROP TABLE {problemsTable};";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"DROP TABLE {cardsTable};";
                    cmd.ExecuteNonQuery();

                    allGood = true;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error deleting: " + ex.Message);
        }


        return allGood;
    }

    public bool deleteProblem(int pid)
    {
        bool allGood = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"DELETE FROM {problemsTable} WHERE problemId = '{pid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error deleting problem: " + ex.Message);
        }

        return allGood;
    }

    public bool deleteCardSet(int csid)
    {
        bool allGood = false;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"DELETE FROM {cardsTable} WHERE setId = '{csid}';";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"DELETE FROM {problemsTable} WHERE setId = '{csid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error deleting problem: " + ex.Message);
        }

        return allGood;
    }
}
