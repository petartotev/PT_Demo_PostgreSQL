using DemoPostgreSql.Models;

namespace DemoPostgreSql;

public class Program
{
    public static async Task<int> Main()
    {
        CommandManager.Execute("docker rm -f postgrescntr && docker run --name postgrescntr -e POSTGRES_PASSWORD=test1234 -p 5432:5432 -d postgres");

        var sqlManager = new SqlManager();

        #region Persons

        await sqlManager.CreateTablePersonsAsync();

        await sqlManager.InsertIntoPersonsAsync("Johnny", "Cash", "0", "Burgas", "123 Krum Popov str.");
        await sqlManager.InsertIntoPersonsAsync("Marie", "Curie", "1", "Stara Zagora", "456 Tri Ushi str.");
        await sqlManager.InsertIntoPersonsAsync("Maya", "Papaya", "1", "Sofia", "234 Kosta Vrachanski str.");

        await sqlManager.UpdatePersonsRowAsync();

        var people = await sqlManager.GetAllPersonsAsync();

        await PrintManager.PrintAllPersonEntitiesAsync(people);

        #endregion

        #region Employees

        await sqlManager.CreateTableEmployeesAsync();

        sqlManager.InsertIntoEmployees(new Employee { FirstName = "John", LastName = "Romero", BirthDate = new DateTime(1954, 7, 15), EmployeeId = 1 });
        sqlManager.InsertIntoEmployees(new Employee { FirstName = "John", LastName = "Carmack", BirthDate = new DateTime(1957, 8, 28), EmployeeId = 2 });
        sqlManager.InsertIntoEmployees(new Employee { FirstName = "John", LastName = "Snow", BirthDate = new DateTime(1966, 6, 16), EmployeeId = 3 });

        var employees = sqlManager.GetAllEmployees();

        await PrintManager.PrintAllEmployeeEntitiesAsync(employees);

        var chosenEmployee = sqlManager.GetEmployeeById(2);

        await PrintManager.PrintEmployeeAsync(chosenEmployee);

        #endregion

        #region Students

        await sqlManager.CreateTypeAddressAsync();
        await sqlManager.CreateTableStudentsAsync();

        sqlManager.InsertIntoStudents(new Student { StudentId = 1, FirstName = "Petar", LastName = "Totev", Age = 34, Address = new ("23 ML", "Burgas", "Burgas", "8000")});
        sqlManager.InsertIntoStudents(new Student { StudentId = 2, FirstName = "Maya", LastName = "Toteva", Age = 1, Address = new ("2 MKV", "Sofia", "Sofia", "1000")});
        sqlManager.InsertIntoStudents(new Student { StudentId = 2, FirstName = "Mark", LastName = "Twain", Age = 123, Address = new ("66 Route", "Illinois", "Detroit", "115315")});

        var students = sqlManager.GetAllStudents();

        await PrintManager.PrintAllStudentEntitiesAsync(students);

        #endregion

        return 0;
    }
}