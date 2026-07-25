using System;

namespace VehicleRentalsCLI
{
    class Program
    {
        static void Main()
        {
            DatabaseContext dbContext = new DatabaseContext();
            AuthService authService = new AuthService(dbContext);
            CustomerService customerService = new CustomerService(dbContext);

            StaffMember? currentUser = null;

            while (currentUser == null)
            {
                currentUser = HandleLoginScreen(authService);
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
                Console.WriteLine("2. Exit");
                Console.Write("\nChoose an option (1-2): ");

                string choice = Console.ReadLine() ?? "";

                if (choice == "1")
                {
                    HandleAddCustomerScreen(customerService, currentUser.EmployeeId);
                }
                else if (choice == "2")
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

        static StaffMember? HandleLoginScreen(AuthService authService)
        {
            Console.WriteLine("\n --- LOGIN ---");
            Console.Write("Enter Employee ID: ");
            string idInput = Console.ReadLine() ?? "";

            Console.Write("Enter Password: ");
            string password = Console.ReadLine() ?? "";

            StaffMember? loggedInUser = authService.AttemptLogin(idInput, password);

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
        static void HandleAddCustomerScreen(CustomerService customerService, int currentStaffId)
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

            bool success = customerService.RegisterNewCustomer(license, firstName, lastName, dob, card, currentStaffId);

            if (success)
            {
                Console.WriteLine($"\n Customer {firstName} {lastName} was successfully added to the system!");
            }
        }

    }
}