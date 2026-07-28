using System;
using Npgsql;

namespace VehicleRentalsCLI
{
    public class DatabaseContext
    {
        private const string ConnectionString = "Host=localhost;Username=postgres;Password=luke;Database=VehicleRental";

        public User? GetStaffMemberByCredentials(int employeeId, string password)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT EmployeeID, FirstName, LastName, IsManager FROM StaffMember WHERE EmployeeID = @id::integer AND Password = @password;";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("id", employeeId);
                        cmd.Parameters.AddWithValue("password", password);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    EmployeeId = (int)reader["EmployeeID"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = (string)reader["LastName"],
                                    IsManager = (bool)reader["IsManager"]
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Database connection failed: \n {ex.Message}");
                return null;
            }
        }

        public bool AddCustomer(Customer newCustomer)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string sqlCust = @"
                                INSERT INTO Customer (DriversLicenseNumber, FirstName, LastName, DateOfBirth, CardNumber, CreatedBy)
                                VALUES (@license, @firstName, @lastName, @dob, @cardNumber, @createdBy);";

                            using (var cmd = new NpgsqlCommand(sqlCust, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("license", newCustomer.DriversLicenseNumber);
                                cmd.Parameters.AddWithValue("firstName", newCustomer.FirstName);
                                cmd.Parameters.AddWithValue("lastName", newCustomer.LastName);
                                cmd.Parameters.AddWithValue("dob", newCustomer.DateOfBirth);
                                cmd.Parameters.AddWithValue("cardNumber", newCustomer.CardNumber);
                                cmd.Parameters.AddWithValue("createdBy", newCustomer.CreatedBy);
                                cmd.ExecuteNonQuery();
                            }

                            foreach (string email in newCustomer.Emails)
                            {
                                string sqlEmail = "INSERT INTO CustomerEmails (DriversLicenseNumber, EmailAddress) VALUES (@license, @email);";
                                using (var cmd = new NpgsqlCommand(sqlEmail, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("license", newCustomer.DriversLicenseNumber);
                                    cmd.Parameters.AddWithValue("email", email);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            foreach (string phone in newCustomer.PhoneNumbers)
                            {
                                string sqlPhone = "INSERT INTO CustomerPhoneNumbers (DriversLicenseNumber, PhoneNumber) VALUES (@license, @phone);";
                                using (var cmd = new NpgsqlCommand(sqlPhone, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("license", newCustomer.DriversLicenseNumber);
                                    cmd.Parameters.AddWithValue("phone", phone);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"\n Could not add customer. Changes rolled back: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not connect to database: {ex.Message}");
                return false;
            }
        }

        public bool AddVehicle(Vehicle newVehicle)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO Vehicle (LicensePlate, Make, Model, ModelYear, TypeOfVehicle, IsAvailable, CreatedBy)
                        VALUES (@plate, @make, @model, @year, @type, @isAvailable, @createdBy);";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("plate", newVehicle.LicensePlate);
                        cmd.Parameters.AddWithValue("make", newVehicle.Make);
                        cmd.Parameters.AddWithValue("model", newVehicle.Model);
                        cmd.Parameters.AddWithValue("year", newVehicle.ModelYear);
                        cmd.Parameters.AddWithValue("type", newVehicle.TypeOfVehicle);
                        cmd.Parameters.AddWithValue("isAvailable", newVehicle.IsAvailable);
                        cmd.Parameters.AddWithValue("createdBy", newVehicle.CreatedBy);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not add vehicle: {ex.Message}");
                return false;
            }
        }

        public bool AddStaffMember(StaffMember newStaff)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO StaffMember (FirstName, LastName, DateOfBirth, IsManager, Password, CompanyPhoneNumber, CompanyEmailAddress, CreatedBy)
                        VALUES (@first, @last, @dob, @isManager, @password, @phone, @email, @createdBy);";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("first", newStaff.FirstName);
                        cmd.Parameters.AddWithValue("last", newStaff.LastName);
                        cmd.Parameters.AddWithValue("dob", newStaff.DateOfBirth);
                        cmd.Parameters.AddWithValue("isManager", newStaff.IsManager);
                        cmd.Parameters.AddWithValue("password", newStaff.Password);
                        cmd.Parameters.AddWithValue("phone", newStaff.CompanyPhoneNumber);
                        cmd.Parameters.AddWithValue("email", newStaff.CompanyEmailAddress);

                        if (newStaff.CreatedBy.HasValue)
                            cmd.Parameters.AddWithValue("createdBy", newStaff.CreatedBy.Value);
                        else
                            cmd.Parameters.AddWithValue("createdBy", DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not add staff member: {ex.Message}");
                return false;
            }
        }

        public System.Collections.Generic.List<Customer> GetAllCustomers()
        {
            var customers = new System.Collections.Generic.List<Customer>();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"
                        SELECT
                            c.DriversLicenseNumber,
                            c.FirstName,
                            c.LastName,
                            c.DateOfBirth,
                            STRING_AGG(DISTINCT ce.EmailAddress, ',') AS Emails,
                            STRING_AGG(DISTINCT cp.PhoneNumber, ',') AS PhoneNumbers
                        FROM Customer c
                        LEFT JOIN CustomerEmails ce ON c.DriversLicenseNumber = ce.DriversLicenseNumber
                        LEFT JOIN CustomerPhoneNumbers cp ON c.DriversLicenseNumber = cp.DriversLicenseNumber
                        GROUP BY c.DriversLicenseNumber, c.FirstName, c.LastName, c.DateOfBirth
                        ORDER BY c.LastName, c.FirstName;";
                    //Used left join so it would still show customers without emails and phone numbers

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cust = new Customer
                            {
                                DriversLicenseNumber = (string)reader["DriversLicenseNumber"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                DateOfBirth = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("DateOfBirth")).ToDateTime(TimeOnly.MinValue)
                            };

                            string emailsAgg = reader["Emails"] as string ?? "";
                            if (!string.IsNullOrEmpty(emailsAgg))
                            {
                                cust.Emails = new System.Collections.Generic.List<string>(emailsAgg.Split(','));
                            }

                            string phonesAgg = reader["PhoneNumbers"] as string ?? "";
                            if (!string.IsNullOrEmpty(phonesAgg))
                            {
                                cust.PhoneNumbers = new System.Collections.Generic.List<string>(phonesAgg.Split(','));
                            }

                            customers.Add(cust);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not retrieve customers: {ex.Message}");
            }
            return customers;
        }
        public System.Collections.Generic.List<Vehicle> GetAllVehicles(bool? isAvailable = null)
        {
            var vehicles = new System.Collections.Generic.List<Vehicle>();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT LicensePlate, Make, Model, ModelYear, TypeOfVehicle, IsAvailable FROM Vehicle";

                    if (isAvailable.HasValue)
                    {
                        sql += " WHERE IsAvailable = @isAvailable";
                    }

                    sql += " ORDER BY Make, Model;";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        if (isAvailable.HasValue)
                        {
                            cmd.Parameters.AddWithValue("isAvailable", isAvailable.Value);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vehicles.Add(new Vehicle
                                {
                                    LicensePlate = (string)reader["LicensePlate"],
                                    Make = (string)reader["Make"],
                                    Model = (string)reader["Model"],
                                    ModelYear = (int)reader["ModelYear"],
                                    TypeOfVehicle = (string)reader["TypeOfVehicle"],
                                    IsAvailable = (bool)reader["IsAvailable"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not retrieve vehicles: {ex.Message}");
            }
            return vehicles;
        }

        public System.Collections.Generic.List<StaffMember> GetAllStaffMembers()
        {
            var staffList = new System.Collections.Generic.List<StaffMember>();
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT EmployeeID, FirstName, LastName, DateOfBirth, IsManager, CompanyPhoneNumber, CompanyEmailAddress FROM StaffMember ORDER BY EmployeeID;";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staffList.Add(new StaffMember
                            {
                                EmployeeId = (int)reader["EmployeeID"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                DateOfBirth = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("DateOfBirth")).ToDateTime(TimeOnly.MinValue),
                                IsManager = (bool)reader["IsManager"],
                                CompanyPhoneNumber = (string)reader["CompanyPhoneNumber"],
                                CompanyEmailAddress = (string)reader["CompanyEmailAddress"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not retrieve staff members: {ex.Message}");
            }
            return staffList;
        }

        public bool RentVehicle(string licensePlate, string driversLicense, DateTime expectedReturnDate, int employeeId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string updateSql = "UPDATE Vehicle SET IsAvailable = FALSE WHERE LicensePlate = @plate AND IsAvailable = TRUE;";
                            using (var cmd = new NpgsqlCommand(updateSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("plate", licensePlate);
                                int rows = cmd.ExecuteNonQuery();

                                if (rows == 0)
                                {
                                    Console.WriteLine("\nERROR: The vehicle is either already rented out or does not exist.");
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            string insertSql = @"
                                INSERT INTO Rents (LicensePlate, DriversLicenseNumber, EmployeeID, RentedDate, ExpectedReturnDate, ReturnDate) 
                                VALUES (@plate, @dl, @empId, CURRENT_DATE, @expected, NULL);";

                            using (var cmd = new NpgsqlCommand(insertSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("plate", licensePlate);
                                cmd.Parameters.AddWithValue("dl", driversLicense);
                                cmd.Parameters.AddWithValue("empId", employeeId);
                                cmd.Parameters.AddWithValue("expected", expectedReturnDate);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine($"\nERROR: Could not process rental: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nCould not connect to database: {ex.Message}");
                return false;
            }
        }

        public bool ReturnVehicle(string licensePlate)
        {
            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string updateRentsSql = "UPDATE Rents SET ReturnDate = CURRENT_DATE WHERE LicensePlate = @plate AND ReturnDate IS NULL;";
                            using (var cmd = new NpgsqlCommand(updateRentsSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("plate", licensePlate);
                                int rows = cmd.ExecuteNonQuery();

                                if (rows == 0)
                                {
                                    Console.WriteLine("\nERROR: No active rental found for this vehicle (it might already be returned or doesn't exist).");
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            string updateVehicleSql = "UPDATE Vehicle SET IsAvailable = TRUE WHERE LicensePlate = @plate;";
                            using (var cmd = new NpgsqlCommand(updateVehicleSql, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("plate", licensePlate);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"\nERROR: Could not process return: {ex.Message}");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nCould not connect to database: {ex.Message}");
                return false;
            }
        }
    }
}