using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using System.Text;

namespace School.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly SchoolContext _context;

    public SubjectsController(SchoolContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectResponseDTO>>> GetSubjects()
    {
        var entities = await _context.Subjects.ToListAsync();
        var result = new List<SubjectResponseDTO>();
        foreach (var entity in entities)
        {
            result.Add(new SubjectResponseDTO()
            {
                Name = entity.Name,
                Key = entity.Key,
                Guid = entity.Guid
            });
        }
        return result;
    }

    [HttpGet("age-0-20/{age:int:range(0, 20)}")]
    public ActionResult<string> GetLevel(int age)
    {
        return "Bachi yosh " + age;
    }

    [HttpGet("age-21-40/{age:int:range(21, 40)}")]
    public ActionResult<string> GetLevel2(int age)
    {
        return "Bachi yosh " + age;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubjectResponseDTO>> GetSubject(Guid id)
    {
        var subject = await _context.Subjects.FindAsync(id);

        if (subject == null)
        {
            return NotFound();
        }

        return new SubjectResponseDTO()
        {
            Name = subject.Name,
            Key = subject.Key,
            Guid = subject.Guid
        };
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubjectResponseDTO>> PutSubject(Guid id, UpdateSubjectDTO subjectDto)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }
        subject.Name = subjectDto.Name;
        subject.Key = subjectDto.Key;

        _context.Subjects.Update(subject);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubjectExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return new SubjectResponseDTO()
        {
            Name = subject.Name,
            Key = subject.Key,
            Guid = subject.Guid
        };
    }

    [HttpPost]
    public async Task<ActionResult<Subject>> PostSubject(CreateSubjectDTO subjectDto)
    {
        var validation = new ModelStateDictionary();
        if (subjectDto.Name == "")
            validation.AddModelError("Name", "Name cannot be empty");

        if (subjectDto.Name.Length > 30)
            validation.AddModelError("Name", "Name length cannot be more the 30 letters");

        if (subjectDto.Name.Length < 5)
            validation.AddModelError("Name", "Name length cannot be less the 5 letters");

        if (_context.Subjects.Any(s => s.Name == subjectDto.Name))
            validation.AddModelError("Name", "Subject with the same name already exists");

        if (subjectDto.Key == "")
            validation.AddModelError("Key", "Key cannot be empty");

        if (subjectDto.Key.Trim().Split(' ').Length > 1)
            validation.AddModelError("Key", "Key cannot contain multiple words");

        if (_context.Subjects.Any(s => s.Key == subjectDto.Key))
            validation.AddModelError("Key", "Key with the same key already exists");

        if (validation.ErrorCount > 0)
            return ValidationProblem(validation);

        var subject = new Subject()
        {
            Key = subjectDto.Key,
            Name = subjectDto.Name
        };
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSubject", new { id = subject.Guid }, new SubjectResponseDTO()
        {
            Name = subject.Name,
            Key = subject.Key,
            Guid = subject.Guid
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(Guid id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SubjectExists(Guid id)
    {
        return _context.Subjects.Any(e => e.Guid == id);
    }
}
