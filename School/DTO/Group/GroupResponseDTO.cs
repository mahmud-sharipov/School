namespace School.DTO.Group;

public class GroupResponseDTO
{
    public Guid Guid { get; set; }
    public string Division { get; set; }
    public int Grade { get; set; }

    public Guid? TeacherGuid { get; set; }
}
