using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace BugTrackerCommandLine
{
    public class DataEntry
    {
        private MySqlConnection connection;
        public void ConnectToDatabase()
        {
            Initialize();
        }

        private void Initialize()
        {
            string cs = "server=10.0.0.161;port=3306;database=BugTracker;uid=bugtracker;password=bugtracker;";
            connection = new MySqlConnection(cs);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException ex)
            {
                switch(ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server. Contact administrator");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public DataTable RunSQL(string sql)
        {
            DataTable results = new DataTable("Results");
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                using (MySqlDataReader dataReader = cmd.ExecuteReader())
                    results.Load(dataReader);

                CloseConnection();

                return results;
            }
            else
                return results;
        }
    }
}
