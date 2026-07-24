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
}