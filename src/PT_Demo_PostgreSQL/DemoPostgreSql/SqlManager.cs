using DemoPostgreSql.Models;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace DemoPostgreSql;

public class SqlManager
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=test1234;Database=postgres";

    #region Persons

    public async Task CreateTablePersonsAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Table Persons:
        var cmdCreateTable =
            @"CREATE TABLE Persons(
              PersonID SERIAL PRIMARY KEY NOT NULL,
              FirstName varchar(255),
              LastName varchar(255),
              Gender varchar(1),
              Address varchar(255),
              City varchar(255)
              );";

        await using (var cmd = new NpgsqlCommand(cmdCreateTable, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task InsertIntoPersonsAsync(string firstName, string lastName, string gender, string city, string address)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Insert into Persons
        await using (var cmd = new NpgsqlCommand("INSERT INTO Persons (FirstName, LastName, Gender, Address, City) VALUES (@fn,@ln,@g,@a,@c)", conn))
        {
            cmd.Parameters.AddWithValue("fn", firstName);
            cmd.Parameters.AddWithValue("ln", lastName);
            cmd.Parameters.AddWithValue("g", gender);
            cmd.Parameters.AddWithValue("a", address);
            cmd.Parameters.AddWithValue("c", city);

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdatePersonsRowAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Insert into Persons
        await using (var cmd = new NpgsqlCommand("UPDATE Persons SET Gender = @g WHERE PersonId = 1", conn))
        {
            cmd.Parameters.AddWithValue("g", "M");

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<List<PersonEntity>> GetAllPersonsAsync()
    {
        List<PersonEntity> personsList = new();

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Retrieve all rows
        await using (var cmd = new NpgsqlCommand("SELECT * FROM Persons", conn))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            if (reader.HasRows)
            {
                // Iterate through the rows and map data to PersonsEntity objects
                while (reader.Read())
                {
                    var person = new PersonEntity
                    {
                        PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        City = reader.GetString(reader.GetOrdinal("City")),
                        Address = reader.GetString(reader.GetOrdinal("Address")),
                        Gender = Convert.ToChar(reader.GetString(reader.GetOrdinal("Gender")))
                    };

                    personsList.Add(person);
                }
            }
        }

        return personsList;
    }

    #endregion

    #region Employees
    public async Task CreateTableEmployeesAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Table Employees:
        var cmdCreateTable =
            @"CREATE TABLE Employees (
              EmployeeId SERIAL PRIMARY KEY,
              EmployeeData JSONB
              );";

        await using (var cmd = new NpgsqlCommand(cmdCreateTable, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public void InsertIntoEmployees(Employee employee)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            var json = JsonConvert.SerializeObject(employee);

            using (var command = new NpgsqlCommand("INSERT INTO Employees (EmployeeData) VALUES (@employeeData)", connection))
            {
                command.Parameters.AddWithValue("employeeData", NpgsqlDbType.Jsonb, json);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Employee> GetAllEmployees()
    {
        var employees = new List<Employee>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT EmployeeData FROM Employees", connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string json = reader.GetString(0);
                        Employee employee = JsonConvert.DeserializeObject<Employee>(json);
                        employees.Add(employee);
                    }
                }
            }
        }

        return employees;
    }

    public Employee GetEmployeeById(int employeeId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand("SELECT EmployeeData FROM Employees WHERE EmployeeId = @employeeId", connection))
            {
                command.Parameters.AddWithValue("employeeId", employeeId);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string json = reader.GetString(0);
                        return JsonConvert.DeserializeObject<Employee>(json);
                    }
                }
            }
        }

        return null;
    }

    #endregion

    #region Students

    public async Task CreateTypeAddressAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Type Address:
        var cmdCreateType =
            @"CREATE TYPE address_type AS (
              street VARCHAR,
              city VARCHAR,
              state VARCHAR,
              zip_code VARCHAR
              );";

        await using (var cmd = new NpgsqlCommand(cmdCreateType, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task CreateTableStudentsAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Table Students:
        var cmdCreateTable =
            @"CREATE TABLE students (
              student_id SERIAL PRIMARY KEY,
              first_name VARCHAR NOT NULL,
              last_name VARCHAR NOT NULL,
              birth_date DATE NOT NULL,
              address address_type
              );";

        await using (var cmd = new NpgsqlCommand(cmdCreateTable, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task CreateTableStudentsContactsAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Table Student Contacts:
        var cmdCreateTable =
            @"CREATE TABLE student_contacts (
              student_id SERIAL PRIMARY KEY,
              email VARCHAR,
              phone_number VARCHAR,
              CONSTRAINT fk_student FOREIGN KEY (student_id) REFERENCES students(student_id) ON DELETE CASCADE
              );";

        await using (var cmd = new NpgsqlCommand(cmdCreateTable, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public void InsertIntoStudents()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            //var sql = 
            //    @"INSERT INTO students (first_name, last_name, birth_date, address)
            //      VALUES (@firstName, @lastName, @birthDate, @address)
            //      RETURNING student_id;";

            var sql =
                  @"INSERT INTO students (first_name, last_name, birth_date)
                  VALUES (@firstName, @lastName, @birthDate)
                  RETURNING student_id;";

            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("firstName", "John");
                command.Parameters.AddWithValue("lastName", "Doe");
                command.Parameters.AddWithValue("birthDate", new DateTime(1990, 1, 1));

                //// Serialize the Address object to JSON
                //var address = JsonConvert.SerializeObject(new Address { Street = "123 Main St", City = "Cityville", State = "CA", ZipCode = "12345" });

                //// Use NpgsqlDbType.Jsonb for the 'address' parameter
                //command.Parameters.AddWithValue("address", NpgsqlDbType.Jsonb, address);

                int studentId = (int)command.ExecuteScalar();

                // Insert additional contact information
                using (NpgsqlCommand contactCommand = new NpgsqlCommand(@"
                    INSERT INTO student_contacts (student_id, email, phone_number)
                    VALUES (@studentId, @email, @phoneNumber)", connection))
                {
                    contactCommand.Parameters.AddWithValue("studentId", studentId);
                    contactCommand.Parameters.AddWithValue("email", "john.doe@example.com");
                    contactCommand.Parameters.AddWithValue("phoneNumber", "555-1234");

                    contactCommand.ExecuteNonQuery();
                }
            }
        }
    }

    public void GetAllStudents()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new NpgsqlCommand(@"
                SELECT s.*, c.email, c.phone_number
                FROM students s
                LEFT JOIN student_contacts c ON s.student_id = c.student_id", connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            StudentId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            BirthDate = reader.GetDateTime(3),
                            //Address = JsonConvert.DeserializeObject<Address>(reader.GetString(4)),

                            // Additional contact information
                            Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                            PhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                        };

                        Console.WriteLine($"StudentId: {student.StudentId}, FirstName: {student.FirstName}, LastName: {student.LastName}, BirthDate: {student.BirthDate}");
                        //Console.WriteLine($"Address: {student.Address.Street}, {student.Address.City}, {student.Address.State} {student.Address.ZipCode}");
                        Console.WriteLine($"Email: {student.Email ?? "N/A"}, PhoneNumber: {student.PhoneNumber ?? "N/A"}");
                        Console.WriteLine();
                    }
                }
            }
        }
    }

    #endregion
}
