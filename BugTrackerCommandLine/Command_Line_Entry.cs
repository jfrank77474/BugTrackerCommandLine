using System;

namespace BugTrackerCommandLine
{
    class Command_Line_Entry
    {
        static void Main(string[] args)
        {
            // THis is an example on how to connect to db and select from table
            //var sql = "select * from Boards;";

            //DataEntry data = new DataEntry();
            //data.ConnectToDatabase();
            //var results = data.RunSQL(sql);

            //foreach(DataRow row in results.Rows)
            //{
            //    Console.WriteLine(row.Field<int>("id") + " " + row.Field<string>("default_boards"));
            //}

            string userInput = "";
            while (userInput.Trim(' ') != "0")
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("Bug Tracker Command Line");
                Console.WriteLine("1) Create a new ticket.");
                Console.WriteLine("2) Change a Ticket Status.");
                Console.WriteLine("3) List all tickets.");
                Console.WriteLine("4) Manage Users.");
                Console.WriteLine("5) Manage Companies.");
                Console.WriteLine("0) Exit.");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("");

                userInput = Console.ReadLine();

                UsersChoice(userInput.Trim(' '));
            }
        }

        static void UsersChoice(string choice)
        {
            switch(choice)
            {
                case "1":
                    Console.WriteLine("New Ticket");
                    break;
                case "2":
                    Console.WriteLine("Change Ticket");
                    break;
                case "3":
                    Console.WriteLine("List Ticket");
                    break;
                case "4":
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
                                Users.ListUsers();
                                break;
                            case "2":
                                Users.CreateUser();
                                break;
                            case "3":
                                Users.ModifyUser();
                                break;
                            case "4":
                                Users.DeleteUser();
                                break;
                            case "0":
                                break;
                        }
                    }

                    break;
                case "5":
                    Console.WriteLine("Create Company");
                    break;
                default:
                    Console.WriteLine("Select one of the menu items");
                    break;
            }
        }
    }
}
