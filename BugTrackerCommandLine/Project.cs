using System;
using System.Data;

// @TODO Change projects to be based on current company
// @TODO User should have the default project that loads when logged in
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

            var sql = "select id, project_name, is_active from Project;";

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

            string name = "";

            DataEntry data = new DataEntry();
            do
            {
                Console.WriteLine("Enter Project Name:");
                name = Console.ReadLine();
            } while (DoesProjectExist(name));

            string sql = $"INSERT INTO Project (project_name, is_active) VALUES ('{name}', 1)";

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

            string sql = $"SELECT project_name FROM Project WHERE id = {id}";

            DataEntry data = new DataEntry();
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

        private static bool DoesProjectExist(string name)
        {
            var sql = $"select project_name from Project WHERE project_name = '{name}';";

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
