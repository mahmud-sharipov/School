using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace School.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly SchoolContext _context;

    public StudentsController(SchoolContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentReponseDTO>>> GetStudents()
    {
        var entitys = await _context.Students.ToListAsync();
        var result = new List<StudentReponseDTO>();
        foreach (var entity in entitys)
        {
            result.Add(StudentReponseDTO.FromEntity(entity));
        }

        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentReponseDTO>> GetStudent(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        return StudentReponseDTO.FromEntity(student);
    }

    [HttpPut("{id}")]

    public async Task<ActionResult<StudentReponseDTO>> PutStudent(Guid id, StudentRequestDTO studentDto)
    {

        var student = await _context.Students.FirstOrDefaultAsync(g => g.Guid == id);

        student.FirstName = studentDto.FirstName;
        student.LastName = studentDto.LastName;
        student.GroupGuid = studentDto.GroupGuid;
        student.MiddleName = studentDto.MiddleName;
        student.Phone = studentDto.Phone;
        student.Gender = studentDto.Gender;
        student.Email = studentDto.Email;
        student.Address = studentDto.Address;
        student.Birthdate = studentDto.Birthdate;

        _context.Students.Update(student);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return StudentReponseDTO.FromEntity(student);
    }

    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(StudentRequestDTO studentDto)
    {
        var validation = new ModelStateDictionary();

        if (studentDto.FirstName == "")
            validation.AddModelError("FirstName", "FirstName cannot be empty");

        if (studentDto.LastName == "")
            validation.AddModelError("LastName", "LastName cannot be empty");

        if (studentDto.MiddleName == "")
            validation.AddModelError("MiddleName", "MiddleName cannot be empty");

        if (studentDto.Address.Length < 10)
            validation.AddModelError("Address", "Address cannot be less the 10 letters");

        if (studentDto.Phone == "")
            validation.AddModelError("Phone", "Phone cannot be empty");

        if (studentDto.Phone.Length < 9)
            validation.AddModelError("Phone", "Phone cannot be less the 9 letters");

        if (!studentDto.Email.Contains("@"))
            validation.AddModelError("Email", "Please, To create an email, use the example: gmail@gmail.com ");

        if (validation.ErrorCount > 0)
            return ValidationProblem(validation);

        var student = new Student()
        {
            Gender = studentDto.Gender,
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            MiddleName = studentDto.MiddleName,
            Address = studentDto.Address,
            Phone = studentDto.Phone,
            Birthdate = studentDto.Birthdate,
            Email = studentDto.Email,
            GroupGuid = studentDto.GroupGuid,
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStudent", new { id = student.Guid }, StudentReponseDTO.FromEntity(student));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StudentExists(Guid id)
    {
        return _context.Students.Any(e => e.Guid == id);
    }
}
