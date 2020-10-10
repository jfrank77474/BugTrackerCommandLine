using System;
using System.Data;
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

        public string getResource(string key)
        {
            ResourceManager rm = new ResourceManager("BugTrackerCommandLine.Properties.serverInfo", Assembly.GetExecutingAssembly());

            return rm.GetString(key);
        }

        private void Initialize()
        {

            string cs = $"server={getResource("server")};port={getResource("port")};database={getResource("db")};uid={getResource("uid")};password={getResource("pw")};";
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
