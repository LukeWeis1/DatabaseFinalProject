namespace VehicleRentalsCLI
{
    public class AuthService
    {
        private readonly DatabaseContext _dbContext;

        public AuthService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StaffMember? AttemptLogin(string idInput, string password)
        {
            //EmployeeID must be an integer
            if (!int.TryParse(idInput, out int employeeId))
            {
                return null;
            }

            //Password cannot be empty
            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            StaffMember? loggedInUser = _dbContext.GetStaffMemberByCredentials(employeeId, password);

            return loggedInUser;
        }
    }


    public class CustomerService
    {
        private readonly DatabaseContext _dbContext;

        public CustomerService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

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
    }
}
