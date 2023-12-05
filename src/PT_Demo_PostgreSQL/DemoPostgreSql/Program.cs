namespace DemoPostgreSql;

public class Program
{
    public static async Task<int> Main()
    {
        var sqlManager = new SqlManager();

        // Persons (relational database)

        await sqlManager.CreateTablePersonsAsync();

        await sqlManager.InsertIntoPersonsAsync("Johnny", "Cash", "0", "Burgas", "123 Krum Popov str.");
        await sqlManager.InsertIntoPersonsAsync("Marie", "Curie", "1", "Stara Zagora", "456 Tri Ushi str.");
        await sqlManager.InsertIntoPersonsAsync("Maya", "Papaya", "1", "Sofia", "234 Kosta Vrachanski str.");

        await sqlManager.UpdatePersonsRowAsync();

        var people = await sqlManager.GetAllPersonsAsync();

        await PrintAllPersonEntitiesAsync(people);

        // Employees (object-relational database)

        await sqlManager.CreateTableEmployeesAsync();

        sqlManager.InsertIntoEmployees(new Employee { FirstName = "John", LastName = "Romero", BirthDate = new DateTime(1954, 7, 15), EmployeeId = 1 });
        sqlManager.InsertIntoEmployees(new Employee { FirstName = "John", LastName = "Carmack", BirthDate = new DateTime(1957, 8, 28), EmployeeId = 2 });
        sqlManager.InsertIntoEmployees(new Employee { FirstName = "John", LastName = "Snow", BirthDate = new DateTime(1966, 6, 16), EmployeeId = 3 });

        var employees = sqlManager.GetAllEmployees();

        await PrintAllEmployeeEntitiesAsync(employees);

        var chosenEmployee = sqlManager.GetEmployeeById(2);

        await PrintEmployeeAsync(chosenEmployee);

        return 0;
    }

    private static async Task PrintAllPersonEntitiesAsync(List<PersonEntity> people)
    {
        foreach (var person in people)
        {
            await Console.Out.WriteLineAsync($"Person {person.FirstName} {person.LastName}, {person.Gender}, lives in {person.City}, {person.Address}.");
        }
    }

    private static async Task PrintAllEmployeeEntitiesAsync(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            await PrintEmployeeAsync(employee);
        }
    }

    private static async Task PrintEmployeeAsync(Employee employee)
    {
        await Console.Out.WriteLineAsync($"Employee {employee.FirstName} {employee.LastName} with employee ID {employee.EmployeeId} was born in {employee.BirthDate.ToString("yyyy-MM-dd")}.");
    }
}