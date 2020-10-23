using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace BugTrackerCommandLine
{
    public enum Menus
    {
        Login,
        Main,
        Project,
        User,
        NewUser,
        Company,
        NewCompany,
        List,
        NewTicket,
        Status,
        Quit
    };

    public class Globals
    {
        public static Stack<Menus> currentMenu = new Stack<Menus>();
        public static string currentProject = null;
        public static string currentCompany = null;
        public static string currentUser = null;
    }

    // @TODO set up company for user/projects/tickets
    //       tickets created, projects created, users created should only
    //       be in the currently logged in company
    class Command_Line_Entry
    {
        static void Main(string[] args)
        {
            Globals.currentMenu.Push(Menus.Login);
            while(true)
            {
                switch(Globals.currentMenu.FirstOrDefault())
                {
                    case Menus.Login:
                        Login();
                        break;
                    case Menus.Main:
                        MainMenu();
                        break;
                    case Menus.Project:
                        Project.ProjectMenu();
                        break;
                    case Menus.User:
                        Users.UserMenu();
                        break;
                    case Menus.NewUser:
                        Users.NewUser();
                        break;
                    case Menus.Company:
                        Company.CompanyMenu();
                        break;
                    case Menus.NewCompany:
                        Company.NewCompany();
                        break;
                    case Menus.NewTicket:
                        Ticket.CreateTicket();
                        break;
                    case Menus.Status:
                        Ticket.ChangeTicketStatus();
                        break;
                    case Menus.List:
                        Ticket.ListTickets();
                        break;
                    case Menus.Quit:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void MainMenu()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine($"Company: {Globals.currentCompany}");
            if(Globals.currentProject != null)
                Console.WriteLine($"Project: {Globals.currentProject}");
            Console.WriteLine("");
            Console.WriteLine("Bug Tracker Command Line");
            Console.WriteLine("1) Create a new ticket.");
            Console.WriteLine("2) Change a Ticket Status.");
            Console.WriteLine("3) List all tickets.");
            Console.WriteLine("4) Manage Projects.");
            Console.WriteLine("5) Manage Users.");
            Console.WriteLine("6) Manage Companies.");
            Console.WriteLine("0) Exit.");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                // @TODO Get Tickets working
                case "1":
                    Globals.currentMenu.Push(Menus.NewTicket);
                    break;
                case "2":
                    Globals.currentMenu.Push(Menus.Status);
                    break;
                case "3":
                    Globals.currentMenu.Push(Menus.List);
                    break;
                case "4":
                    Globals.currentMenu.Push(Menus.Project);
                    break;
                case "5":
                    Globals.currentMenu.Push(Menus.User);
                    break;
                case "6":
                    Globals.currentMenu.Push(Menus.Company);
                    break;
                case "0":
                    Globals.currentMenu.Pop();
                    break;
                default:
                    Console.WriteLine("Select one of the menu items");
                    break;
            }
        }

        static void Login()
        {
            Console.WriteLine("Company? (quit to exit)");
            string c = Console.ReadLine();

            if (c.ToLower() == "quit")
            {
                Globals.currentMenu.Push(Menus.Quit);
                return;
            }

            if (!Company.DoesCompanyExist(c))
            {
                // No company found need to create new one
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("No Company Found with that name would you like to create one? (y/n)");
                string answer = Console.ReadLine();

                if (answer.ToLower() == "y")
                    Globals.currentMenu.Push(Menus.NewCompany);
                return;
            }
            Globals.currentCompany = c;

            Console.WriteLine("User Name? (quit to exit)");
            string u = Console.ReadLine();

            if (u.ToLower() == "quit")
            {
                Globals.currentMenu.Push(Menus.Quit);
                return;
            }

            if (!Users.DoesUserExist(u))
            {
                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("No User with that User Name would you like to create one? (y/n)");
                string answer = Console.ReadLine();

                if (answer.ToLower() == "y")
                    Globals.currentMenu.Push(Menus.NewUser);
                return;
            }
            Globals.currentUser = u;

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

            Console.WriteLine("\n-----------------------------------------------");
            string sql = $"SELECT C.company_name, U.user_name, U.password FROM Users AS U, Company AS C WHERE C.id = U.company_id AND C.company_name = '{c}' AND U.user_name = '{u}' LIMIT 1";
            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var user_check = data.RunSQL(sql);

            if (user_check.Rows.Count > 0)
            {
                string encryptedPW = "";
                foreach (DataRow row in user_check.Rows)
                {
                    encryptedPW = row.Field<string>("password");
                }

                // @TODO change this to use the password as the key and have it encrypted to the current key
                if (EncryptionDecryptionService.Encrypt(data.getResource("key"), p) == encryptedPW)
                {
                    Users.GetUserDefaultProject();
                    Globals.currentMenu.Push(Menus.Main);
                }
                else
                {
                    Console.WriteLine("Wrong Password");
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                    return;
                }
            }
        }
    }
}
