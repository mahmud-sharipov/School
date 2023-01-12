namespace School.API.Models;

public class Group : EntityBase
{
    public Group()
    {
        Students = new HashSet<Student>();
    }

    public string Division { get; set; }
    public int Grade { get; set; }

    public Guid? TeacherGuid { get; set; }
    public virtual Teacher Teacher { get; set; }

    public virtual ICollection<Student> Students { get; set; }

}