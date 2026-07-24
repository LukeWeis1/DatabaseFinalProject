using System;

namespace VehicleRentalsCLI
{
    class Program
    {
        static void Main()
        {
            DatabaseContext dbContext = new DatabaseContext();
            AuthService authService = new AuthService(dbContext);

            StaffMember? currentUser = null;

            while (currentUser == null)
            {
                currentUser = HandleLoginScreen(authService);
                if (currentUser == null)
                {
                    Console.WriteLine("Please try again.\n");
                }
            }

            Console.WriteLine($"\n==============================");
            Console.WriteLine($" MAIN MENU (Logged in as: {currentUser.FirstName})");
            Console.WriteLine($"==============================");

        }

        static StaffMember? HandleLoginScreen(AuthService authService)
        {
            Console.WriteLine("\n--- LOGIN ---");
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
    }
}