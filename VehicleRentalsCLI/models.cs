using System;

namespace VehicleRentalsCLI
{
    public class User
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
        public System.Collections.Generic.List<string> Emails { get; set; } = new System.Collections.Generic.List<string>();
        public System.Collections.Generic.List<string> PhoneNumbers { get; set; } = new System.Collections.Generic.List<string>();
    }

    public class Vehicle
    {
        public string LicensePlate { get; set; } = "";
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int ModelYear { get; set; }
        public string TypeOfVehicle { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
        public int CreatedBy { get; set; }
    }

    public class StaffMember
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool IsManager { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; } = "";
        public string CompanyPhoneNumber { get; set; } = "";
        public string CompanyEmailAddress { get; set; } = "";
        public int? CreatedBy { get; set; }
    }

}