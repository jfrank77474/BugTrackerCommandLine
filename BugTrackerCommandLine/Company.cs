using System;
using System.Data;

namespace BugTrackerCommandLine
{
    // @TODO need to create function for listing, modifying, and deleting Companies
    public class Company
    {
        public static bool CheckLogin(string company, string user, string password)
        {
            Console.WriteLine("\n-----------------------------------------------");
            string sql = $"SELECT id FROM Company WHERE company_name = '{company}'";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                int id = 0;
                foreach (DataRow row in results.Rows)
                {
                    id = row.Field<int>("id");
                    break;
                }

                sql = $"SELECT user_name, password FROM Users WHERE company_id = '{id}' AND user_name = '{user}'";
                data.ConnectToDatabase();
                var user_check = data.RunSQL(sql);

                if (user_check.Rows.Count > 0)
                {
                    string encryptedPW = "";
                    foreach (DataRow row in user_check.Rows)
                    {
                        encryptedPW = row.Field<string>("password");
                    }

                    if (EncryptionDecryptionService.Encrypt(data.getResource("key"), password) == encryptedPW)
                        return true;
                    else
                    {
                        Console.WriteLine("Wrong Password");
                        Console.WriteLine("-----------------------------------------------");
                        Console.WriteLine("");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("No User with that User Name. Please contact administrator.");
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                    return false;
                }
            }
            else
            {
                // No company found need to create new one
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("No Company Found with that name would you like to create one? (y/n)");
                string answer = Console.ReadLine();

                if (answer.ToLower() == "y")
                    CreateCompany();
            }
            return false;
        }

        public static void ListCompany()
        {
        }

        public static void CreateCompany()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Create a new Company");

            string name = "";

            DataEntry data = new DataEntry();
            do
            {
                Console.WriteLine("Enter User Name:");
                name = Console.ReadLine();
            } while (DoesUserExist(name));

            string sql = $"INSERT INTO Company (company_name, is_active) VALUES  ('{name}', 1)";

            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        public static void ModifyCompany()
        {
        }

        public static void DeleteCompany()
        {
        }

        private static bool DoesUserExist(string name)
        {

            var sql = $"select company_name from Company WHERE company_name = '{name}';";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                Console.WriteLine("Company already Exists");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return true;
            }
            else
                return false;
        }
    }
}
