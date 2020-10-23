using System;
using System.Data;

namespace BugTrackerCommandLine
{
    class Project
    {
        public static void ProjectMenu()
        {
            string userInput = "";
            while (userInput.Trim(' ') != "0")
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("Manage Projects.");
                Console.WriteLine("1) List Projects");
                Console.WriteLine("2) Create a new Project");
                Console.WriteLine("3) Change current Project");
                Console.WriteLine("0) Previous Menu");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ListProject();
                        break;
                    case "2":
                        CreateProject();
                        break;
                    case "3":
                        ChangeProject();
                        break;
                    case "0":
                        Globals.currentMenu.Pop();
                        break;
                }
            }
        }

        private static void ListProject()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("List Projects");

            var sql = $"select P.id, P.project_name, P.is_active from Project P, Company C WHERE P.company_id = C.id AND C.company_name = '{Globals.currentCompany}';";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("Id: " + row.Field<int>("id"));
                    Console.WriteLine("Project Name: " + row.Field<string>("project_name"));
                    Console.WriteLine("Is Active: " + row.Field<bool>("is_active"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("No Projects Found");
                ProjectMenu();
            }
        }

        private static void CreateProject()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Create a new Project");

            string sql = $"SELECT id FROM Company WHERE company_name = '{Globals.currentCompany}'";
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

            string name = "";
            do
            {
                Console.WriteLine("Enter Project Name:");
                name = Console.ReadLine();
            } while (DoesProjectExist(name));

            sql = $"INSERT INTO Project (company_id, project_name, is_active) VALUES ({company_id}, '{name}', 1)";

            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void ChangeProject()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Change Current Project");

            ListProject();

            Console.WriteLine("Enter the ID for current project selection");
            string id = Console.ReadLine();

            string sql = $"SELECT P.id, P.project_name FROM Project P, Company C WHERE P.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND P.id = {id}";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach(DataRow row in results.Rows)
                {
                    Globals.currentProject = row.Field<string>("project_name");
                    Users.SetUserDefaultProject(row.Field<int>("id"));
                }
            }
        }

        private static bool DoesProjectExist(string name)
        {
            var sql = $"select P.project_name from Project P, Company C WHERE P.company_id = C.id AND C.company_name = '{Globals.currentCompany}' AND P.project_name = '{name}';";

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
