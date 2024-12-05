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
        //get the database path and create the database
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
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {usersTable} (userId INTEGER PRIMARY KEY AUTOINCREMENT, username VARCHAR(25) NOT NULL, password VARCHAR(20), isAdmin INTEGER);";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {notesTable} (noteId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(128), body VARCHAR(512), userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {cardsTable} (setId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(20), isPublic INTEGER, userId INTEGER, FOREIGN KEY (userId) REFERENCES {usersTable}(userId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {problemsTable} (problemId INTEGER PRIMARY KEY AUTOINCREMENT, problem VARCHAR(30), answer VARCHAR(30), setId INTEGER, FOREIGN KEY (setId) REFERENCES {cardsTable}(setId));";
                command.ExecuteNonQuery();

                command.CommandText = $"CREATE TABLE IF NOT EXISTS {commentsTable} (commentsId INTEGER PRIMARY KEY AUTOINCREMENT, title VARCHAR(20), body VARCHAR(250), setId INTEGER, FOREIGN KEY (setId) REFERENCES {problemsTable}(setId));";
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
                    cmd.CommandText = $"INSERT INTO {usersTable} (username, password, isAdmin) VALUES ('{usernameFS}', '{password}', '0');";
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
    //Create a cardset
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
    //Add a card (problem) to a cardset
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
    //Retrieve the cardset ID from the title and user ID
    public int getCardsetId(string title, int uid)
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
                    cmd.CommandText = $"SELECT * FROM {cardsTable} WHERE title = '{title}' AND userId = '{uid}';";
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
    //Get a users personal cardsets
    public List<cardsetObj> getPrivateCardsets(int uid)
    {
        List<cardsetObj> allPrivateProbs = new List<cardsetObj>();

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
                                cardsetObj curCard = new cardsetObj();
                                curCard.setCardsetTitle(reader["title"].ToString());
                                curCard.setId(Int32.Parse(reader["setId"].ToString()));
                                allPrivateProbs.Add(curCard);
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

        return allPrivateProbs;
    }
    //Get all the public cardsets
    public List<cardsetObj> getPublicCardsets()
    {
        List<cardsetObj> allCards = new List<cardsetObj>();

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {cardsTable} WHERE isPublic = '{2}';";

                        using(SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cardsetObj curCard = new cardsetObj();
                                curCard.setCardsetTitle(reader["title"].ToString());
                                curCard.setId(Int32.Parse(reader["setId"].ToString()));
                                allCards.Add(curCard);
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

        return allCards;
    }
   //Get all the problems in a cardset
    public List<problemObj> getAllProblems(int sid)
    {
        List<problemObj> allProblems = new List<problemObj>();

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";

            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {problemsTable} WHERE setId = '{sid}';";

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
    //Update cardset title and specific problem in said cardset
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
   //Drop the problems table (Used in development)
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
    //Delete a problem
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
    //Delete a cardset
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
    //Update a users password
    public bool updatePassword(int uid, string pw)
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
                    cmd.CommandText = $"UPDATE {usersTable} SET password = '{pw}' WHERE userId = '{uid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error updating password: " + ex.Message);
        }

        return allGood;
    }
    //Update users level (Make them an admin)
    public bool updateAdminLevel(int uid, int level)
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
                    cmd.CommandText = $"UPDATE {usersTable} SET isAdmin = '{level}' WHERE userId = '{uid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }

        }
        catch(Exception ex)
        {
            Debug.Log("Error updating user level: " + ex.Message);
        }

        return allGood;
    }
    //Retrieve a users level (Check if they're an admin)
    public int getUserLevel(int uid)
    {
        int level = -1;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT isAdmin FROM {usersTable} WHERE userId = {uid};";
                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        level = Int32.Parse(reader["isAdmin"].ToString());
                    }
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error getting user level: " + ex.Message);
        }

        return level;
    }
   //Delete a user
    public bool deleteUser(int uid)
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
                    bool subOperation;
                    cmd.CommandText = $"DELETE FROM {notesTable} where userId = '{uid}';";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"DELETE FROM {cardsTable} WHERE userId = '{uid}';";
                    cmd.ExecuteNonQuery();
                    subOperation = deleteOrphanProblems();
                    cmd.CommandText = $"DELETE FROM {usersTable} WHERE userId = '{uid}';";
                    cmd.ExecuteNonQuery();
                    if(subOperation)
                    {
                        allGood = true;
                    }
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error deleting user: " + ex.Message);
        }

        return allGood;
    }
    //Delete orphan problems (happens when a cardset is deleted but the problems aren't)
    private bool deleteOrphanProblems()
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
                    cmd.CommandText = $"DELETE FROM {problemsTable} where setId NOT IN (SELECT setId FROM {cardsTable});";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error deleting problems: " + ex.Message);
        }

        return allGood;
    }
    //Retrieve all cardsets that need to be approved before being made public (admin only tool)
    public List<cardsetObj> getCardsetsForApproval()
    {
        List<cardsetObj> cards = new List<cardsetObj>();
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {cardsTable} WHERE isPublic = '{1}' ;";
                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cardsetObj curCardset = new cardsetObj();
                            curCardset.setCardsetTitle(reader["title"].ToString());
                            curCardset.setId(Convert.ToInt32(reader["setId"].ToString()));
                            cards.Add(curCardset);
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Error: " + " Line 810, " + ex.Message);
        }

        return cards;
    }
    //Change a cardset to public
    public bool changeCardsetToPublic(int sid)
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
                    cmd.CommandText = $"UPDATE {cardsTable} SET isPublic = '{2}' WHERE setId = '{sid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error making cardset public: " + ex.Message);
        }

        return allGood;
    }
   //Checks if a cardset is public or not
    public bool getCardsetPublicState(int sid)
    {
        bool isPublic = false;
        int level = -1;

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT isPublic FROM {cardsTable} WHERE setId = '{sid}';";
                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        level = Int32.Parse(reader["isPublic"].ToString());
                        if(level != 0)
                        {
                            isPublic = true;
                        }
                    }
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error getting public level: " + ex.Message);
        }

        return isPublic;
    }
    //Drop the cards table and the problems table (used in development)
    public void dropCardsTable()
    {
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"DROP TABLE {cardsTable}";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"DROP TABLE {problemsTable}";
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error deleting problems: " + ex.Message);
        }
    }
    //Add a comment to a cardset
    public bool addComment(string title, string body, int sid)
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
                    cmd.CommandText = $"INSERT INTO {commentsTable} (title, body, setId) VALUES ('{title}', '{body}', '{sid}');";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }
                
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error adding comment: " + ex.Message);
        }

        return allGood;
    }
   //Retrieve all comments tied to a public cardset
   public List<commentObj> getComments(int sid)
    {
        List<commentObj> comments = new List<commentObj>();
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {commentsTable} WHERE setId = '{sid}';";

                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            commentObj commentData = new commentObj();
                            commentData.setTitle(reader["title"].ToString());
                            commentData.setBody(reader["body"].ToString());
                            commentData.setSetId(sid);
                            commentData.setCommentId(Int32.Parse(reader["commentsId"].ToString()));
                            comments.Add(commentData);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error adding comment: " + ex.Message);
        }

        return comments;

    }
    //Drop comments table (This is used in development)
    public void dropCommentsTable()
    {
        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"DROP TABLE {commentsTable};";
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error dropping comments table: " + ex.Message);
        }

    }
    //Get all users and their information
    public List<userObject> getAllUsers()
    {
        List<userObject> users = new List<userObject>();

        try
        {
            string connectionString = $"URI=file:{dbPath.Replace("\\", "/")}";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using(SqliteCommand cmd = new SqliteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {usersTable};";

                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userObject curUser = new userObject();
                            curUser.setUid(Int32.Parse(reader["userId"].ToString()));
                            curUser.setUserName(reader["username"].ToString());
                            curUser.setPassword(reader["password"].ToString());
                            curUser.setUserLevel(Int32.Parse(reader["isAdmin"].ToString()));
                            users.Add(curUser);

                        }
                    }
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error getting users: " + ex.Message);
        }

        return users;
    }
    public bool updateUser(userObject curUser)
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
                    string uName = curUser.getUserName();
                    string pWord = curUser.getUserPassword();
                    int uid = curUser.getUid();
                    int adminLevel = curUser.getUserLevel();
                    cmd.CommandText = $"UPDATE {usersTable} SET username = '{uName}', password = '{pWord}', isAdmin = '{adminLevel}' WHERE userId = '{uid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error updating user: " + ex.Message);
        }

        return allGood;
    }
    //Change cardset to pending aproval state
    public bool updateToPending(int sid)
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
                    cmd.CommandText = $"UPDATE {cardsTable} SET isPublic = '{1}' WHERE setId = '{sid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error changing cardset level, line 1116: " + ex.Message);
        }


        return allGood;
    }
    //Revert cardset to private
    public bool revertCardset(int sid)
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
                    cmd.CommandText = $"UPDATE {cardsTable} SET isPublic = '{0}' WHERE setId = '{sid}';";
                    cmd.ExecuteNonQuery();
                    allGood = true;
                }

                connection.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error changing cardset level, line 1146: " + ex.Message);
        }


        return allGood;
    }
}
