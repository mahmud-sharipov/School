namespace School.Models;

public class Teacher : Person
{
    public Teacher()
    {
        Groups = new HashSet<Group>();
    }

    public int WorkExperience { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
}