namespace DemoPostgreSql.Models;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public (string Street, string City, string State, string ZipCode) Address { get; set; }
}
