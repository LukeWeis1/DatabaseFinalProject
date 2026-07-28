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

                Console.WriteLine("4. View All Customers");
                Console.WriteLine("5. View All Vehicles");
                Console.WriteLine("6. View Staff Directory");
                Console.WriteLine("7. Rent a Vehicle");
                Console.WriteLine("8. Return a Vehicle");
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
                else if (choice == "4")
                {
                    HandleViewCustomersScreen(businessLogic);
                }
                else if (choice == "5")
                {
                    HandleViewVehiclesScreen(businessLogic);
                }
                else if (choice == "6")
                {
                    HandleViewStaffScreen(businessLogic);
                }
                else if (choice == "7")
                {
                    HandleRentVehicleScreen(businessLogic, currentUser.EmployeeId);
                }
                else if (choice == "8")
                {
                    HandleReturnVehicleScreen(businessLogic);
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

            System.Collections.Generic.List<string> emails = new System.Collections.Generic.List<string>();
            while (true)
            {
                Console.Write("Enter Email Address (or press Enter to finish): ");
                string email = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(email)) break;
                emails.Add(email);
            }

            System.Collections.Generic.List<string> phones = new System.Collections.Generic.List<string>();
            while (true)
            {
                Console.Write("Enter Phone Number (or press Enter to finish): ");
                string phone = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(phone)) break;
                phones.Add(phone);
            }


            bool success = businessLogic.RegisterNewCustomer(license, firstName, lastName, dob, card, emails, phones, currentStaffId);
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

        static void HandleViewCustomersScreen(BusinessLogic businessLogic)
        {
            Console.WriteLine("\n--- CUSTOMER DIRECTORY ---");
            var customers = businessLogic.GetAllCustomers();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found in the database.");
                return;
            }

            foreach (var c in customers)
            {
                Console.WriteLine($"\nName: {c.FirstName} {c.LastName}");
                Console.WriteLine($"DOB: {c.DateOfBirth.ToString("yyyy-MM-dd")} | License: {c.DriversLicenseNumber}");

                if (c.Emails.Count > 0)
                    Console.WriteLine($"Emails: {string.Join(", ", c.Emails)}");
                else
                    Console.WriteLine("Emails: None");

                if (c.PhoneNumbers.Count > 0)
                    Console.WriteLine($"Phones: {string.Join(", ", c.PhoneNumbers)}");
                else
                    Console.WriteLine("Phones: None");
            }
            Console.WriteLine("\n--------------------------");
        }

        static void HandleViewVehiclesScreen(BusinessLogic businessLogic)
        {
            Console.WriteLine("\n--- VEHICLE FLEET ---");
            Console.WriteLine("1. Show Available Vehicles");
            Console.WriteLine("2. Show Rented Out Vehicles");
            Console.WriteLine("3. Show All Vehicles");
            Console.Write("Filter choice (1-3): ");

            string choice = Console.ReadLine() ?? "";
            bool? isAvailable = null;

            if (choice == "1")
            {
                isAvailable = true;
                Console.WriteLine("\nShowing: AVAILABLE VEHICLES");
            }
            else if (choice == "2")
            {
                isAvailable = false;
                Console.WriteLine("\nShowing: RENTED OUT VEHICLES");
            }
            else
            {
                Console.WriteLine("\nShowing: ALL VEHICLES");
            }

            var vehicles = businessLogic.GetAllVehicles(isAvailable);

            if (vehicles.Count == 0)
            {
                Console.WriteLine("No vehicles found matching that filter.");
                return;
            }

            foreach (var v in vehicles)
            {
                string status = v.IsAvailable ? "Available" : "Rented Out";
                Console.WriteLine($"[{v.LicensePlate}] {v.ModelYear} {v.Make} {v.Model} ({v.TypeOfVehicle}) - Status: {status}");
            }
            Console.WriteLine("---------------------");
        }

        static void HandleViewStaffScreen(BusinessLogic businessLogic)
        {
            Console.WriteLine("\n--- STAFF DIRECTORY ---");
            var staff = businessLogic.GetAllStaffMembers();

            if (staff.Count == 0)
            {
                Console.WriteLine("No staff found in the database.");
                return;
            }

            foreach (var s in staff)
            {
                string role = s.IsManager ? "Manager" : "Staff";
                Console.WriteLine($"[ID: {s.EmployeeId}] {s.FirstName} {s.LastName} ({role})");
                Console.WriteLine($"    Email: {s.CompanyEmailAddress} | Phone: {s.CompanyPhoneNumber}");
            }
            Console.WriteLine("-----------------------");
        }

        static void HandleRentVehicleScreen(BusinessLogic businessLogic, int currentStaffId)
        {
            Console.WriteLine("\n--- RENT A VEHICLE ---");
            Console.Write("Enter Vehicle License Plate: ");
            string plate = Console.ReadLine() ?? "";

            Console.Write("Enter Customer Drivers License Number: ");
            string dl = Console.ReadLine() ?? "";

            Console.Write("Enter Expected Return Date (YYYY-MM-DD): ");
            string expectedDate = Console.ReadLine() ?? "";

            bool success = businessLogic.RentVehicle(plate, dl, expectedDate, currentStaffId);
            if (success)
            {
                Console.WriteLine($"\nVehicle {plate} has been successfully rented to customer {dl}!");
                Console.WriteLine($"          Expected Return: {expectedDate}");
            }
        }

        static void HandleReturnVehicleScreen(BusinessLogic businessLogic)
        {
            Console.WriteLine("\n--- RETURN A VEHICLE ---");
            Console.Write("Enter Vehicle License Plate: ");
            string plate = Console.ReadLine() ?? "";

            bool success = businessLogic.ReturnVehicle(plate);
            if (success)
            {
                Console.WriteLine($"\nVehicle {plate} has been successfully returned and is now available!");
            }
        }
    }
}