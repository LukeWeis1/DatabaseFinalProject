INSERT INTO StaffMember (FirstName, LastName, DateOfBirth, IsManager, Password, CompanyPhoneNumber, CompanyEmailAddress, CreatedBy)
VALUES 
('Admin', 'User', '1980-01-01', TRUE, 'admin_hash', '555-0000', 'admin@rentals.com', NULL),
('Luke', 'Weis', '2005-06-18', FALSE, 'luke_hash', '713-3626', 'luke@rentals.com', 1);

INSERT INTO Customer (DriversLicenseNumber, FirstName, LastName, DateOfBirth, CardNumber, CreatedBy)
VALUES 
('RICK-137-C137', 'Rick', 'Sanchez', '1943-01-01', '5555-5555-5555-5555', 1),
('MORTY-99-S99', 'Morty', 'Smith', '1999-07-20', '1234-5678-9101-1121', 2);

INSERT INTO CustomerEmails (DriversLicenseNumber, EmailAddress) 
VALUES 
('RICK-137-C137', 'rick.sanchez@citadel.com'), 
('RICK-137-C137', 'wubbalubba@portal.net'),
('MORTY-99-S99', 'morty.smith@highschool.edu'), 
('MORTY-99-S99', 'ohgee@gmail.com');

INSERT INTO CustomerPhoneNumbers (DriversLicenseNumber, PhoneNumber) 
VALUES 
('RICK-137-C137', '555-3453'), 
('RICK-137-C137', '555-4355'),
('MORTY-99-S99', '555-3451'), 
('MORTY-99-S99', '555-0567');

INSERT INTO Vehicle (LicensePlate, Make, Model, ModelYear, TypeOfVehicle, IsAvailable, CreatedBy)
VALUES 
('TEST-01', 'Tesla', 'Model 3', 2024, 'Sedan', TRUE, 1),
('TEST-02', 'Ford', 'Bronco', 2023, 'SUV', FALSE, 2);