using System;
using Npgsql;

class VehicleRentalsApplication
{

    //Database Connection String
    private const string ConnectionString = "Host=localhost;Username=postgres;Password=luke;Database=VehicleRental";

    static void Main()
    {

        bool isLoggedIn = false;
        while (!isLoggedIn)
        {
            isLoggedIn = Login();
            if (!isLoggedIn)
            {
                Console.WriteLine("Please try again.");
            }
        }
    }

    static bool Login()
    {
        Console.WriteLine("\n--- LOGIN ---");
        Console.Write("Enter Employee ID: ");
        string idInput = Console.ReadLine() ?? "";
        if (!int.TryParse(idInput, out int employeeId))
        {
            Console.WriteLine("[!] Invalid ID format. Must be a number.");
            return false;
        }

        Console.Write("Enter Password: ");
        string password = Console.ReadLine() ?? "";

        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT FirstName, LastName FROM StaffMember WHERE EmployeeID = @id::integer AND Password = @password;";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", employeeId);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = (string)reader["FirstName"];
                            string lastName = (string)reader["LastName"];

                            Console.WriteLine($"\n Welcome, {firstName} {lastName}!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("\n Invalid Employee ID or Password.");
                            return false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n Database connection failed:\n{ex.Message}");
            return false;
        }
    }
}


