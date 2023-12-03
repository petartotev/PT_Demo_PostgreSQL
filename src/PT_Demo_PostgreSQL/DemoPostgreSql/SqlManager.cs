using Npgsql;

namespace DemoPostgreSql;

public class SqlManager
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=testmest1234;Database=postgres";

    public async Task CreateTablePersonsAsync()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        // Create Table Persons
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
}
