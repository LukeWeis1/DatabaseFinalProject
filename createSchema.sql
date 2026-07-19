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