namespace School.API.DTO;

public class StudentReponseDTO : StudentRequestDTO
{
    public Guid Guid { get; set; }


    public static StudentReponseDTO FromEntity(Student student)
    {
        return new StudentReponseDTO()
        {
            Guid = student.Guid,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Address = student.Address,
            Birthdate = student.Birthdate,
            Email = student.Email,
            Gender = student.Gender,
            GroupGuid = student.GroupGuid,
            MiddleName = student.MiddleName,
            Phone = student.Phone
        };
    }
}
