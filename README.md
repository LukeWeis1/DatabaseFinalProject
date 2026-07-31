# Vehicle Rental CLI System

This project is a C# CLI application for managing a vehicle rental business.

## Key Features
- Secure login system with different roles for managers and staff
- Add and View customers, with the ability for customers to have multiple phone numbers and email addresses
- Add (managers only) and view vehicles
  - Optionally sort by returned vehicles or vehicles that are still rented when viewing vehicles
- Add (managers only) and view staff members
- Rent vehicles to customers
- Return vehicles
- View Rental Records
  - Optionally sort by returned vehicles or vehicles that are still rented

## Architecture

This project was built using C#.  It uses a Three-Tier Architecture to separate concerns:
1. **Presentation Layer (`Program.cs`)**: Handles all user interaction, console menus, and input/output. Contains zero database or business logic.
2. **Business Logic Layer (`businessLogic.cs`)**: Validates user input, enforces business rules (i.e., restricting vehicle creation to managers), handles encryption/hashing, and dictates transaction flow.
3. **Data Access Layer (`dataLayer.cs`)**: The only layer authorized to communicate with PostgreSQL using `Npgsql`. Maps raw relational data into C# Objects (Models).

Models for each of the objects are stored in models.cs, and EncryptionHelper.cs contains the logic to actually encrypt passwords and credit card numbers before they are stored in the database.
