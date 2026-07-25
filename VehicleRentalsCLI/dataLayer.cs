using System;
using Npgsql;

namespace VehicleRentalsCLI
{
    public class DatabaseContext
    {
        private const string ConnectionString = "Host=localhost;Username=postgres;Password=luke;Database=VehicleRental";

        public StaffMember? GetStaffMemberByCredentials(int employeeId, string password)
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
                                return new StaffMember
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
                using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO Customer (DriversLicenseNumber, FirstName, LastName, DateOfBirth, CardNumber, CreatedBy)
                        VALUES (@license, @firstName, @lastName, @dob, @cardNumber, @createdBy);";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("license", newCustomer.DriversLicenseNumber);
                        cmd.Parameters.AddWithValue("firstName", newCustomer.FirstName);
                        cmd.Parameters.AddWithValue("lastName", newCustomer.LastName);
                        cmd.Parameters.AddWithValue("dob", newCustomer.DateOfBirth);
                        cmd.Parameters.AddWithValue("cardNumber", newCustomer.CardNumber);
                        cmd.Parameters.AddWithValue("createdBy", newCustomer.CreatedBy);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Could not add customer: {ex.Message}");
                return false;
            }
        }
    }
}