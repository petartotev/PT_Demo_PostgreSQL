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

    public async Task<List<Person>> GetAllPersonsAsync()
    {
        List<Person> personsList = new();

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
                    var person = new Person
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
            @"CREATE TYPE Address AS (
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

    public async Task CreateFunctionGetAddressAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Function get_address:
        var cmdCreateFunction =
            @"CREATE FUNCTION get_address(p_address Address)
              RETURNS TABLE(street VARCHAR, city VARCHAR, state VARCHAR, zip_code VARCHAR) AS $$
              BEGIN
                  RETURN QUERY SELECT (p_address).street, (p_address).city, (p_address).state, (p_address).zip_code;
              END; $$
              LANGUAGE plpgsql;";

        await using (var cmd = new NpgsqlCommand(cmdCreateFunction, conn))
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
            @"CREATE TABLE Students (
              studentid SERIAL PRIMARY KEY,
              firstname VARCHAR,
              lastname VARCHAR,
              age INT,
              address Address
              );";

        await using (var cmd = new NpgsqlCommand(cmdCreateTable, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public void InsertIntoStudents(Student student)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var cmdInsert =
            @"INSERT INTO Students (firstname, lastname, age, address)
              VALUES (@firstname, @lastname, @age, ROW(@street, @city, @state, @zip_code)::Address)";

        using var cmd = new NpgsqlCommand(cmdInsert, conn);

        cmd.Parameters.AddWithValue("firstname", student.FirstName);
        cmd.Parameters.AddWithValue("lastname", student.LastName);
        cmd.Parameters.AddWithValue("age", student.Age);
        cmd.Parameters.AddWithValue("street", student.Address.Street);
        cmd.Parameters.AddWithValue("city", student.Address.City);
        cmd.Parameters.AddWithValue("state", student.Address.State);
        cmd.Parameters.AddWithValue("zip_code", student.Address.ZipCode);
        cmd.ExecuteNonQuery();
    }

    public List<Student> GetAllStudents()
    {
        var students = new List<Student>();

        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT studentid, firstname, lastname, age, (address).street, (address).city, (address).state, (address).zip_code FROM Students", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Student
            {
                StudentId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Age = reader.GetInt32(3),
                Address = (reader.GetString(4),
                           reader.GetString(5),
                           reader.GetString(6),
                           reader.GetString(7))
            });
        }

        return students;
    }

    #endregion
}
