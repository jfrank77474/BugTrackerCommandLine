using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Resources;
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
            ResourceManager rm = new ResourceManager("BugTrackerCommandLine.Properties.serverInfo", Assembly.GetExecutingAssembly());

            string cs = $"server={rm.GetString("server")};port={rm.GetString("port")};database={rm.GetString("db")};uid={rm.GetString("uid")};password={rm.GetString("pw")};";
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
