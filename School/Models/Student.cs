namespace Schoole.Models
{
    public class Student : Person
    {
        public Guid GroupGuid { get; set; }
        public virtual Group Group { get; set; }
    }
}
