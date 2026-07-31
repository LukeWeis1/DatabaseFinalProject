# Vehicle Rental CLI System

This project is a C# CLI application for managing a vehicle rental business.  The requirements for this project can be found here: https://github.com/LukeWeis1/DatabaseFinalProject/blob/main/Vehicle%20Rental%20Database%20Requirements%20Gathering.pdf

## Key Features
- Secure login system with different roles for managers and staff
- Add and View customers, with the ability for customers to have multiple phone numbers and email addresses
- Add (managers only) and view vehicles
  - Optionally sort by available or rented out vehicles
- Add (managers only) and view staff members
- Rent vehicles to customers
- Return vehicles
- View Rental Records
  - Optionally sort by available or rented out vehicles

## Architecture

This project was built using C#.  It uses a Three-Tier Architecture to separate concerns:
1. **Presentation Layer (`Program.cs`)**: Handles all user interaction, console menus, and input/output. Contains zero database or business logic.
2. **Business Logic Layer (`businessLogic.cs`)**: Validates user input, enforces business rules (i.e., restricting vehicle creation to managers), handles encryption/hashing, and dictates transaction flow.
3. **Data Access Layer (`dataLayer.cs`)**: The only layer authorized to communicate with PostgreSQL using `Npgsql`. Maps raw relational data into C# Objects (Models).

Models for each of the objects are stored in models.cs, and EncryptionHelper.cs contains the logic to hash passwords and encrypt credit card numbers before they are stored in the database.

## Getting Started

To run this application locally, you will need:
 - C# .NET SDK Version 10.0.302, available at https://dotnet.microsoft.com/en-us/download
 - Postgre SQL Version 18, available at https://www.postgresql.org/download/
 - A database administration tool like pgAdmin

### Database Setup
1. Open pgAdmin and create a new database named `VehicleRental`.
2. Open the query tool and run createSchema.sql.  This will create the following tables with the following attributes:
    -   StaffMember
        - EmployeeID
        - FirstName
        - LastName
        - DateOfBirth
        - IsManager
        - Password
        - CompanyPhoneNumber
        - CompanyEmailADdress
        - CreatedBy
    -   Vehicle
        - LicensePlate
        - Make
        - Model
        - ModelYear
        - TypeOfVehicle
        - IsAvailable
        - CreatedBy
    -   Customer
        - DriversLicenseNumber
        - FirstName
        - LastName
        - DateOfBirth
        - CardNumber
        - CreatedBy
    -   CustomerEmails
        - DriversLicenseNumber
        - EmailAddress  
    -   CustomerPhoneNumbers
        - DriversLicenseNumber
        - PhoneNumber  
    -   Rents
        - LicensePlate
        - DriversLicenseNumber
        - EmployeeID
        - RentedDate
        - ExpectedReturnDate
        - ReturnDate
          
More detail on the schema can be found at https://github.com/LukeWeis1/DatabaseFinalProject/blob/main/Vehicle%20Rental%20System%20Relational%20Schema.pdf and https://github.com/LukeWeis1/DatabaseFinalProject/blob/main/Vehicle%20Rental%20System%20ER%20Diagram.pdf

3. Execute sampleData.sql in the query tool to populate the database with sample data.
    - Note: Passwords are unhashed and Credit Card numbers are unencrypted in the sample data.  The application is designed to be able to handle this data, but when creating new data in the application, it will be properly hashed/encrypted.

### Application Configuration
1. Open `dataLayer.cs`
2. Locate the `ConnectionString` variable at the top of the class.
3. Update the `Username` and `Password` to match your local PostgreSQL installation credentials
```csharp
private const string ConnectionString = "Host=localhost;Username=postgres;Password=YOUR_PASSWORD_HERE;Database=VehicleRental";
```

### Running the Application
1. In the terminal, make sure you are in the VehicleRentalsCLI folder
2. Run the command `dotnet run`
3. You can now login with 1 of the 2 provided test accounts
    - Admin Test Account:
      - Employee ID: 1
      - Password: admin_hash
   -  Staff Member Test Account:
       - Employee ID: 2
       - Password: luke_hash
