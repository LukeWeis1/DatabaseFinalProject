CREATE TABLE StaffMember (
    EmployeeID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    IsManager BOOLEAN NOT NULL,
    Password VARCHAR(255) NOT NULL,
    CompanyPhoneNumber VARCHAR(20) NOT NULL,
    CompanyEmailAddress VARCHAR(255) NOT NULL,
    CreatedBy INT,
    FOREIGN KEY (CreatedBy) REFERENCES StaffMember(EmployeeID)
);

CREATE TABLE Vehicle (
	LicensePlate VARCHAR(20) PRIMARY KEY,
	Make VARCHAR (50) NOT NULL,
	Model VARCHAR (50) NOT NULL,
	ModelYear INT NOT NULL,
	TypeOfVehicle VARCHAR(50) NOT NULL,
	IsAvailable BOOLEAN DEFAULT TRUE NOT NULL,
	CreatedBy INT,
    FOREIGN KEY (CreatedBy) REFERENCES StaffMember(EmployeeID)
);

CREATE TABLE Customer (
    DriversLicenseNumber VARCHAR(50) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    CardNumber VARCHAR(255) NOT NULL,
    CreatedBy INT,
    FOREIGN KEY (CreatedBy) REFERENCES StaffMember(EmployeeID)
);

CREATE TABLE CustomerEmails (
    DriversLicenseNumber VARCHAR(50) NOT NULL,
    EmailAddress VARCHAR(255) NOT NULL,
    PRIMARY KEY (DriversLicenseNumber, EmailAddress),
    FOREIGN KEY (DriversLicenseNumber) REFERENCES Customer(DriversLicenseNumber) ON DELETE CASCADE
);

CREATE TABLE CustomerPhoneNumbers (
    DriversLicenseNumber VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    PRIMARY KEY (DriversLicenseNumber, PhoneNumber),
    FOREIGN KEY (DriversLicenseNumber) REFERENCES Customer(DriversLicenseNumber) ON DELETE CASCADE
);
