using System;

namespace VehicleRentalsCLI
{
    public class BusinessLogic
    {
        private readonly DatabaseContext _dbContext;

        public BusinessLogic(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Authentication Logic
        public User? AttemptLogin(string idInput, string password)
        {
            if (!int.TryParse(idInput, out int employeeId))
            {
                return null; 
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return _dbContext.GetStaffMemberByCredentials(employeeId, password);
        }

        //Customer Logic
        public bool RegisterNewCustomer(string license, string firstName, string lastName, string dobInput, string card, int staffId)
        {
            if (string.IsNullOrWhiteSpace(license) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("\n License, First Name, and Last Name cannot be empty.");
                return false;
            }

            if (!DateTime.TryParse(dobInput, out DateTime dateOfBirth))
            {
                Console.WriteLine("\n Invalid Date of Birth. Please use format YYYY-MM-DD.");
                return false;
            }

            Customer newCustomer = new Customer
            {
                DriversLicenseNumber = license,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                CardNumber = card,
                CreatedBy = staffId
            };

            return _dbContext.AddCustomer(newCustomer);
        }

        //Vehicle Logic
        public bool RegisterNewVehicle(string licensePlate, string make, string model, string yearInput, string type, int creatorId, bool isManager)
        {
            if (!isManager)
            {
                Console.WriteLine("\n ACCESS DENIED: Only managers are authorized to add new vehicles to the fleet.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(licensePlate) || string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model))
            {
                Console.WriteLine("\n License Plate, Make, and Model cannot be empty.");
                return false;
            }

            if (!int.TryParse(yearInput, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                Console.WriteLine("\n Invalid Model Year.");
                return false;
            }

            Vehicle newVehicle = new Vehicle
            {
                LicensePlate = licensePlate,
                Make = make,
                Model = model,
                ModelYear = year,
                TypeOfVehicle = type,
                IsAvailable = true,
                CreatedBy = creatorId
            };

            return _dbContext.AddVehicle(newVehicle);
        }

        //Staff logic
        public bool RegisterNewStaff(string firstName, string lastName, string dobInput, bool makeManager, string password, string phone, string email, int creatorId, bool isManager)
        {
            if (!isManager)
            {
                Console.WriteLine("\n ACCESS DENIED: Only managers can add new staff.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("\n First Name, Last Name, and Password are required.");
                return false;
            }

            if (!DateTime.TryParse(dobInput, out DateTime dateOfBirth))
            {
                Console.WriteLine("\n Invalid Date of Birth. Please use format YYYY-MM-DD.");
                return false;
            }

            StaffMember newStaff = new StaffMember
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                IsManager = makeManager,
                Password = password,
                CompanyPhoneNumber = phone,
                CompanyEmailAddress = email,
                CreatedBy = creatorId
            };

            return _dbContext.AddStaffMember(newStaff);
        }
    }
}