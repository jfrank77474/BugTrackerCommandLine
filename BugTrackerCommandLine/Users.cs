using System;
using System.Data;

namespace BugTrackerCommandLine
{
    public class Users
    {
        public static void UserMenu()
        {
            string userInput = "";
            while (userInput.Trim(' ') != "0")
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("Manage Users.");
                Console.WriteLine("1) List Users");
                Console.WriteLine("2) Create a new User");
                Console.WriteLine("3) Modify a User");
                Console.WriteLine("4) Delete a User");
                Console.WriteLine("0) Previous Menu");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ListUsers();
                        break;
                    case "2":
                        CreateUser();
                        break;
                    case "3":
                        ModifyUser();
                        break;
                    case "4":
                        DeleteUser();
                        break;
                    case "0":
                        CurrentMenu.currentMenu.Pop();
                        break;
                }
            }
        }
        
        public static void NewUser()
        {
            CreateUser();
            CurrentMenu.currentMenu.Pop();
        }

        private static void ListUsers()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("List Users");

            var sql = "select id, user_name, display_name, email, is_active from Users;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("Id: " + row.Field<int>("id"));
                    Console.WriteLine("User Name: " + row.Field<string>("user_name"));
                    Console.WriteLine("Display Name: " + row.Field<string>("display_name"));
                    Console.WriteLine("Email: " + row.Field<string>("email"));
                    Console.WriteLine("Is Active: " + row.Field<bool>("is_active"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("No Users Found");
                UserMenu();
            }
        }

        private static void CreateUser()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Create a new User");

            string name = "", display_name, email, password;

            DataEntry data = new DataEntry();
            do
            {
                Console.WriteLine("Enter User Name:");
                name = Console.ReadLine();
            } while (DoesUserExist(name));

            Console.WriteLine("Enter User Display Name:");
            display_name = Console.ReadLine();

            Console.WriteLine("Enter Email:");
            email = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            password = Console.ReadLine();

            string encryptedPw = EncryptionDecryptionService.Encrypt(data.getResource("key"), password);

            string sql = $"INSERT INTO Users (user_name, display_name, email, password, is_active) VALUES  ('{name}', '{display_name}', '{email}', '{encryptedPw}', 1)";

            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void ModifyUser()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Modify a User");

            ListUsers();

            Console.WriteLine("Enter the ID you want to modify:");
            string id = Console.ReadLine();

            string sql = $"select id, user_name, display_name, email, is_active from Users WHERE id = {id}";
            Console.WriteLine(sql);

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            string old_name = "", old_display_name = "", old_email = "";
            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    old_name = row.Field<string>("user_name");
                    old_display_name = row.Field<string>("display_name");
                    old_email = row.Field<string>("email");
                }
            }

            string name, display_name, email, password;
            bool is_active;
            Console.WriteLine($"Enter new User Name for ID {id} blank to keep the same:");
            Console.WriteLine($"Old User Name: {old_name}");
            name = Console.ReadLine();

            if (name.Trim() == "")
                name = old_name;

            Console.WriteLine($"Enter new User Display Name for ID {id} blank to keep the same:");
            Console.WriteLine($"Old User Display Name: {old_display_name}");
            display_name = Console.ReadLine();

            if (display_name.Trim() == "")
                display_name = old_display_name;

            Console.WriteLine($"Enter new Email for ID {id} blank to keep the same:");
            Console.WriteLine($"Old Email: {old_email}");
            email = Console.ReadLine();

            if (email.Trim() == "")
                email = old_email;

            Console.WriteLine($"Enter new Password for ID {id}:");
            password = Console.ReadLine();

            string encryptedPw = EncryptionDecryptionService.Encrypt(data.getResource("key"), password);

            Console.WriteLine($"Is {id} still Active? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
                is_active = true;
            else
                is_active = false;

            sql = $"update Users set user_name='{name}', display_name='{display_name}', email='{email}', password='{encryptedPw}', is_active={is_active} WHERE id = {id}";
            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void DeleteUser()
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

        public static bool DoesUserExist(string name)
        {

            var sql = $"select user_name from Users WHERE user_name = '{name}';";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}
