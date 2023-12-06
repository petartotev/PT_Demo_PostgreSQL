using DemoPostgreSql.Models;

namespace DemoPostgreSql;

public class PrintManager
{
    public static async Task PrintAllPersonEntitiesAsync(List<Person> people)
    {
        await Console.Out.WriteLineAsync();

        foreach (var person in people)
        {
            await Console.Out.WriteLineAsync(
                $"{nameof(Person)} {person.FirstName} {person.LastName} (ID {person.PersonId}), {person.Gender}, lives in {person.City}, {person.Address}.");
        }
    }

    public static async Task PrintAllEmployeeEntitiesAsync(List<Employee> employees)
    {
        await Console.Out.WriteLineAsync();

        foreach (var employee in employees)
        {
            await PrintEmployeeAsync(employee);
        }
    }

    public static async Task PrintEmployeeAsync(Employee employee)
    {
        await Console.Out.WriteLineAsync(
            $"{nameof(Employee)} {employee.FirstName} {employee.LastName} (ID {employee.EmployeeId}), was born in {employee.BirthDate:yyyy-MM-dd}.");
    }

    public static async Task PrintAllStudentEntitiesAsync(List<Student> students)
    {
        await Console.Out.WriteLineAsync();

        foreach (var student in students)
        {
            await Console.Out.WriteLineAsync(
                $"{nameof(Student)} {student.FirstName} {student.LastName} (ID {student.StudentId}), {student.Age} years old, lives in {student.Address.State}, {student.Address.City}, {student.Address.Street}.");
        }
    }
}
