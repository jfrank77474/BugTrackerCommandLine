﻿using System;
using System.Data;

namespace BugTrackerCommandLine
{
    public class Company
    {
        public static void CompanyMenu()
        {
            string userInput = "";
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
                        ListCompany();
                        break;
                    case "2":
                        CreateCompany();
                        break;
                    case "3":
                        ModifyCompany();
                        break;
                    case "4":
                        DeleteCompany();
                        break;
                    case "0":
                        CurrentMenu.currentMenu.Pop();
                        break;
                }
            }
        }

        public static void NewCompany()
        {
            CreateCompany();
            CurrentMenu.currentMenu.Pop();
        }

        private static void ListCompany()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("List Companies");

            var sql = "select company_name, is_active from Company;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("Company Name: " + row.Field<string>("company_name"));
                    Console.WriteLine("Is Active: " + row.Field<bool>("is_active"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }
            }
            else
                Console.WriteLine("No Companies Found");

            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void CreateCompany()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Create a new Company");

            string name = "";

            DataEntry data = new DataEntry();
            do
            {
                Console.WriteLine("Enter Company Name:");
                name = Console.ReadLine();
            } while (DoesCompanyExist(name));

            string sql = $"INSERT INTO Company (company_name, is_active) VALUES  ('{name}', 1)";

            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void ModifyCompany()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Modify a Company");

            var sql = "select id, user_name from Company;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("ID: " + row.Field<int>("id"));
                    Console.WriteLine("Company Name: " + row.Field<string>("company_name"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }
            }
            else
                Console.WriteLine("No Companies Found");

            Console.WriteLine("Enter the ID you want to modify:");
            string id = Console.ReadLine();

            string name;
            bool is_active;
            Console.WriteLine($"Enter new Company Name for ID {id}:");
            name = Console.ReadLine();

            Console.WriteLine($"Is {id} still Active? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
                is_active = true;
            else
                is_active = false;

            sql = $"update Company set company_name='{name}', is_active={is_active} WHERE id = {id}";
            data.ConnectToDatabase();
            data.RunSQL(sql);

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        private static void DeleteCompany()
        {
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Delete a Company");

            var sql = "select id, company_name from Company;";

            DataEntry data = new DataEntry();
            data.ConnectToDatabase();
            var results = data.RunSQL(sql);

            if (results.Rows.Count > 0)
            {
                foreach (DataRow row in results.Rows)
                {
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("ID: " + row.Field<int>("id"));
                    Console.WriteLine("Company Name: " + row.Field<string>("company_name"));
                    Console.WriteLine("-----------------------------------------------");
                    Console.WriteLine("");
                }

                Console.WriteLine("Enter the ID you want to delete:");
                string id = Console.ReadLine();

                Console.WriteLine($"Are you sure you would like to delete ID {id}? (y/n)");
                string answer = Console.ReadLine();

                if (answer.ToLower() == "y")
                {
                    sql = $"DELETE FROM Company WHERE id = {id}";
                    data.ConnectToDatabase();
                    data.RunSQL(sql);
                }
            }
            else
            {
                Console.WriteLine("No Companies Found");
                Console.ReadKey();
            }

            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("");
        }

        public static bool DoesCompanyExist(string name)
        {

            var sql = $"select company_name from Company WHERE company_name = '{name}';";

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