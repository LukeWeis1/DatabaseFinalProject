using System;

namespace VehicleRentalsCLI
{
    class Program
    {
        static void Main()
        {
            DatabaseContext dbContext = new DatabaseContext();
            BusinessLogic businessLogic = new BusinessLogic(dbContext);

            User? currentUser = null;

            while (currentUser == null)
            {
                currentUser = HandleLoginScreen(businessLogic);
                if (currentUser == null)
                {
                    Console.WriteLine("Please try again.\n");
                }
            }

            bool exitApp = false;
            while (!exitApp)
            {
                Console.WriteLine($"\n==============================");
                Console.WriteLine($" MAIN MENU (Logged in as: {currentUser.FirstName})");
                Console.WriteLine($"==============================");
                Console.WriteLine("1. Add New Customer");

                if (currentUser.IsManager)
                {
                    Console.WriteLine("2. Add New Vehicle");
                    Console.WriteLine("3. Add New Staff Member");
                }

                Console.WriteLine("9. Exit");
                Console.Write("\n Choose an option: ");

                string choice = Console.ReadLine() ?? "";

                if (choice == "1")
                {
                    HandleAddCustomerScreen(businessLogic, currentUser.EmployeeId);
                }
                else if (choice == "2" && currentUser.IsManager)
                {
                    HandleAddVehicleScreen(businessLogic, currentUser.EmployeeId, currentUser.IsManager);
                }
                else if (choice == "3" && currentUser.IsManager)
                {
                    HandleAddStaffScreen(businessLogic, currentUser.EmployeeId, currentUser.IsManager);
                }
                else if (choice == "9")
                {
                    exitApp = true;
                    Console.WriteLine("\n Goodbye!");
                }
                else
                {
                    Console.WriteLine("\n Invalid choice. Please try again.");
                }
            }
        }

        static void HandleAddVehicleScreen(BusinessLogic businessLogic, int currentStaffId, bool isManager)
        {
            Console.WriteLine("\n--- ADD NEW VEHICLE ---");
            Console.Write("License Plate: ");
            string plate = Console.ReadLine() ?? "";

            Console.Write("Make: ");
            string make = Console.ReadLine() ?? "";

            Console.Write("Model: ");
            string model = Console.ReadLine() ?? "";

            Console.Write("Model Year: ");
            string year = Console.ReadLine() ?? "";

            Console.Write("Type of Vehicle (e.g., SUV, Sedan): ");
            string type = Console.ReadLine() ?? "";

            bool success = businessLogic.RegisterNewVehicle(plate, make, model, year, type, currentStaffId, isManager);
            if (success)
            {
                Console.WriteLine($"\n Vehicle {make} {model} ({plate}) was added!");
            }
        }

        static void HandleAddStaffScreen(BusinessLogic businessLogic, int currentStaffId, bool isManager)
        {
            Console.WriteLine("\n--- HIRE NEW STAFF MEMBER ---");
            Console.Write("First Name: ");
            string first = Console.ReadLine() ?? "";

            Console.Write("Last Name: ");
            string last = Console.ReadLine() ?? "";

            Console.Write("Date of Birth (YYYY-MM-DD): ");
            string dob = Console.ReadLine() ?? "";

            Console.Write("Are they a Manager? (y/n): ");
            bool makeManager = (Console.ReadLine() ?? "").Trim().ToLower() == "y";

            Console.Write("Set temporary password: ");
            string password = Console.ReadLine() ?? "";

            Console.Write("Company Phone: ");
            string phone = Console.ReadLine() ?? "";

            Console.Write("Company Email: ");
            string email = Console.ReadLine() ?? "";

            bool success = businessLogic.RegisterNewStaff(first, last, dob, makeManager, password, phone, email, currentStaffId, isManager);
            if (success)
            {
                Console.WriteLine($"\n Staff Member {first} {last} was successfully added!");
            }
        }

        static void HandleAddCustomerScreen(BusinessLogic businessLogic, int currentStaffId)
        {
            Console.WriteLine("\n--- ADD NEW CUSTOMER ---");

            Console.Write("Drivers License Number: ");
            string license = Console.ReadLine() ?? "";

            Console.Write("First Name: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Date of Birth (YYYY-MM-DD): ");
            string dob = Console.ReadLine() ?? "";

            Console.Write("Credit Card Number: ");
            string card = Console.ReadLine() ?? "";

            bool success = businessLogic.RegisterNewCustomer(license, firstName, lastName, dob, card, currentStaffId);
            if (success)
            {
                Console.WriteLine($"\n Customer {firstName} {lastName} was successfully added to the system!");
            }
        }

        static User? HandleLoginScreen(BusinessLogic businessLogic)
        {
            Console.WriteLine("\n--- LOGIN ---");
            Console.Write("Enter Employee ID: ");
            string idInput = Console.ReadLine() ?? "";

            Console.Write("Enter Password: ");
            string password = Console.ReadLine() ?? "";

            User? loggedInUser = businessLogic.AttemptLogin(idInput, password);

            if (loggedInUser != null)
            {
                Console.WriteLine($"\n Welcome, {loggedInUser.FirstName} {loggedInUser.LastName}!");
                return loggedInUser;
            }
            else
            {
                Console.WriteLine("\n Invalid Employee ID or Password.");
                return null;
            }
        }
    }
}