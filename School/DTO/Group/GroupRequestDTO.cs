namespace School.DTO.Group;

public class GroupRequestDTO
{
    public string Division { get; set; }
    public int Grade { get; set; }

    public Guid? TeacherGuid { get; set; }
}
