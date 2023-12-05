namespace DemoPostgreSql.Models;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
