namespace School.API.DTO.Teacher;

public class TeacherResponseDTO : TeacherRequestDTO
{
    public Guid Guid { get; set; }

    public static TeacherResponseDTO FromEntity(Teacher teacher)
    {
        return new TeacherResponseDTO
        {
            Gender = teacher.Gender,
            Email = teacher.Email,
            Address = teacher.Address,
            Birthdate = teacher.Birthdate,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Guid = teacher.Guid,
            MiddleName = teacher.MiddleName,
            Phone = teacher.Phone,
            WorkExperience = teacher.WorkExperience
        };
    }
}

public static class TeacherExtensions
{
    public static void MapFromDTO(this Teacher teacher, TeacherRequestDTO teacherDto)
    {
        teacher.FirstName = teacherDto.FirstName;
        teacher.LastName = teacherDto.LastName;
        teacher.Address = teacherDto.Address;
        teacher.Birthdate = teacherDto.Birthdate;
        teacher.Email = teacherDto.Email;
        teacher.Gender = teacherDto.Gender;
        teacher.MiddleName = teacherDto.MiddleName;
        teacher.Phone = teacherDto.Phone;
        teacher.WorkExperience = teacherDto.WorkExperience;
    }

    public static TeacherResponseDTO ToResponseDto(this Teacher teacher)
    {
        return new TeacherResponseDTO
        {
            Gender = teacher.Gender,
            Email = teacher.Email,
            Address = teacher.Address,
            Birthdate = teacher.Birthdate,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Guid = teacher.Guid,
            MiddleName = teacher.MiddleName,
            Phone = teacher.Phone,
            WorkExperience = teacher.WorkExperience
        };
    }
}


