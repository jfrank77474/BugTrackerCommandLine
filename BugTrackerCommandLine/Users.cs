using System;
using System.Data;

namespace BugTrackerCommandLine
{
    public class Users
    {
        public static void ListUsers()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("List Users");

            var sql = "select user_name, display_name, email, password, is_active from Users;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("User Name: " + row.Field<string>("user_name"));
                    Console.WriteLine("Display Name: " + row.Field<string>("display_name"));
                    Console.WriteLine("Email: " + row.Field<string>("email"));
                    Console.WriteLine("Password: " + row.Field<string>("password"));
                    Console.WriteLine("Is Active: " + row.Field<bool>("is_active"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }
            }
            else
                Console.WriteLine("No Users Found");

            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        public static void CreateUser()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Create a new User");

            string name, display_name, email, password;

            Console.WriteLine("Enter User Name:");
            name = Console.ReadLine();

            Console.WriteLine("Enter User Display Name:");
            display_name = Console.ReadLine();

            Console.WriteLine("Enter Email:");
            email = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            password = Console.ReadLine();

            DataEntry data = new DataEntry();
            string encryptedPw = EncryptionDecryptionService.Encrypt(data.getResource("key"), password);

            string sql = $"INSERT INTO Users (user_name, display_name, email, password, is_active) VALUES  ('{name}', '{display_name}', '{email}', '{encryptedPw}', 1)";

            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        public static void ModifyUser()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Modify a User");

            var sql = "select id, user_name, display_name, email from Users;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("ID: " + row.Field<int>("id"));
                    Console.WriteLine("User Name: " + row.Field<string>("user_name"));
                    Console.WriteLine("Display Name: " + row.Field<string>("display_name"));
                    Console.WriteLine("Email: " + row.Field<string>("email"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }
            }
            else
                Console.WriteLine("No Users Found");

            Console.WriteLine("Enter the ID you want to modify:");
            string id = Console.ReadLine();

            string name, display_name, email, password;
            bool is_active;
            Console.WriteLine($"Enter new User Name for ID {id}:");
            name = Console.ReadLine();

            Console.WriteLine($"Enter new User Display Namefor ID {id}:");
            display_name = Console.ReadLine();

            Console.WriteLine($"Enter new Email for ID {id}:");
            email = Console.ReadLine();

            Console.WriteLine($"Enter new Password for ID {id}:");
            password = Console.ReadLine();

            string encryptedPw = EncryptionDecryptionService.Encrypt(data.getResource("key"), password);

            Console.WriteLine($"Is {id} still Active? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
                is_active = true;
            else
                is_active = false;

            sql = $"update Users set user_name='{name}', display_name='{display_name}', email='{email}', password='{encryptedPw}', is_active={is_active}";
            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        public static void DeleteUser()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Delete a User");

            var sql = "select id, user_name, display_name from Users;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("ID: " + row.Field<int>("id"));
                    Console.WriteLine("User Name: " + row.Field<string>("user_name"));
                    Console.WriteLine("Display Name: " + row.Field<string>("display_name"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }

                Console.WriteLine("Enter the ID you want to delete:");
                string id = Console.ReadLine();

                Console.WriteLine($"Are you sure you would like to delete ID {id}? (y/n)");
                string answer = Console.ReadLine();

                if (answer.ToLower() == "y")
                {
                    sql = $"DELETE FROM Users WHERE id = {id}";
                    data.ConnectToDatabase();
                    data.RunSQL(sql);
                }
            }
            else
            {
                Console.WriteLine("No Users Found");
                Console.ReadKey();
            }

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }
    }
}
