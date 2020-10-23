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
                        Globals.currentMenu.Pop();
                        break;
                }
            }
        }
        
        public static void NewUser()
        {
            CreateUser();
            Globals.currentMenu.Pop();
        }

        private static void ListUsers()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("List Users");

            var sql = $"select U.id, U.user_name, U.display_name, U.email, U.is_active from Users U, Company C WHERE U.company_id = C.id AND C.company_name = '{Globals.currentCompany}';";

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
            // @TODO check to see if currently logged in user is admin if not check to see if there are any users in company
            //       if 0 users in current company then create user and give admin rights
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Create a new User");

            string sql = $"SELECT id FROM Company Where company_name = '{Globals.currentCompany}'";
            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            int company_id = 0;
            if(results.Rows.Count > 0)
            {
                foreach(DataRow row in results.Rows)
                {
                    company_id = row.Field<int>("id");
                }
            }

            string name = "", display_name, email, password;
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

            sql = $"INSERT INTO Users (company_id, user_name, display_name, email, password, is_active) VALUES  ('{company_id}', '{name}', '{display_name}', '{email}', '{encryptedPw}', 1)";

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

            string sql = $"select U.id, U.company_id, U.user_name, U.display_name, U.email, U.is_active from Users U, Company C WHERE U.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND U.id = {id}";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            string old_name = "", old_display_name = "", old_email = "";
            int company_id = 0;
            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    company_id = row.Field<int>("company_id");
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

            sql = $"update Users set user_name='{name}', display_name='{display_name}', email='{email}', password='{encryptedPw}', is_active={is_active} WHERE id = {id} AND company_id = {company_id}";
            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void DeleteUser()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Delete a User");

            ListUsers();

            Console.WriteLine("Enter the ID you want to delete:");
            string id = Console.ReadLine();

            Console.WriteLine($"Are you sure you would like to delete ID {id}? (y/n)");
            string answer = Console.ReadLine();

            if (answer.ToLower() == "y")
            {
                string sql = $"DELETE FROM Users U, Company C WHERE U.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND U.id = {id}";

                DataEntry data = new DataEntry();
                data.ConnectToDatabase();
                data.RunSQL(sql);
            }

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        public static bool DoesUserExist(string name)
        {
            var sql = $"select user_name from Users U, Company C WHERE U.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND U.user_name = '{name}';";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
                return true;
            else
                return false;
        }
        
        public static void SetUserDefaultProject(int project_id)
        {
            DataEntry data = new DataEntry();
            data.ConnectToDatabase();

            string sql = $"update Users U, Company C set default_active_project='{project_id}' WHERE U.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND U.user_name = '{Globals.currentUser}'";
            data.ConnectToDatabase();
            data.RunSQL(sql);
        }

        public static void GetUserDefaultProject()
        {
            DataEntry data = new DataEntry();
            data.ConnectToDatabase();

            string sql = $"SELECT P.project_name FROM Project P, Users U, Company C WHERE P.id = U.default_active_project AND U.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND U.user_name = '{Globals.currentUser}'";
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach(DataRow row in results.Rows)
                {
                    Globals.currentProject = row.Field<string>("project_name");
                }
            }
        }
    }
}
