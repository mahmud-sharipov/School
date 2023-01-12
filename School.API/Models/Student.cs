namespace School.API.Models;

public class Student : Person
{
    public Guid GroupGuid { get; set; }
    public virtual Group Group { get; set; }

    public int Test { get; set; }
}
