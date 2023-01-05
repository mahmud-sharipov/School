namespace School.Models;

public abstract class Person : EntityBase
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public Gender Gender { get; set; }
    public DateTime Birthdate { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
