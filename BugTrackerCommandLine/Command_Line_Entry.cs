using Renci.SshNet;
using System;

namespace BugTrackerCommandLine
{
    class Command_Line_Entry
    {
        static void Main(string[] args)
        {
            bool is_valid = false;
            do
            {
                Console.WriteLine("Company?");
                string c = Console.ReadLine();

                Console.WriteLine("User Name?");
                string u = Console.ReadLine();

                Console.WriteLine("Password");
                string p = "";

                //Hides the plain text password from view
                ConsoleKey key;
                do
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && p.Length > 0)
                    {
                        Console.Write("\b \b");
                        p = p[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        p += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                is_valid = Company.CheckLogin(c, u, p);
            } while(!is_valid);

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
                    userInput = "";
                    while (userInput.Trim(' ') != "0")
                    {
                        Console.WriteLine("-----------------------------------------------");
                        Console.WriteLine("Manage Companies.");
                        Console.WriteLine("1) List Companies");
                        Console.WriteLine("2) Create a new Company");
                        Console.WriteLine("3) Modify a Company");
                        Console.WriteLine("4) Delete a Company");
                        Console.WriteLine("0) Previous Menu");
                        Console.WriteLine("-----------------------------------------------");
                        Console.WriteLine("");

                        userInput = Console.ReadLine();

                        switch (userInput)
                        {
                            case "1":
                                Company.ListCompany();
                                break;
                            case "2":
                                Company.CreateCompany();
                                break;
                            case "3":
                                Company.ModifyCompany();
                                break;
                            case "4":
                                Company.DeleteCompany();
                                break;
                            case "0":
                                break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Select one of the menu items");
                    break;
            }
        }
    }
}
