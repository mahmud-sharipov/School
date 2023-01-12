using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace School.Controllers;

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
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        return await _context.Students.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(Guid id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return student;
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> PutStudent(Guid id, Student student)
    {
        if (id != student.Guid)
        {
            return BadRequest();
        }

        _context.Entry(student).State = EntityState.Modified;

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

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(CreateStrudentDTO studentDto)
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

        return CreatedAtAction("GetStudent", new { id = student.Guid }, studentDto);
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
