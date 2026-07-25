
namespace VehicleRentalsCLI
{
    public class StaffMember
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool IsManager { get; set; }
    }

    public class Customer
    {
        public string DriversLicenseNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string CardNumber { get; set; } = "";
        public int CreatedBy { get; set; } //EmployeeID of the staff member who created them
    }
}