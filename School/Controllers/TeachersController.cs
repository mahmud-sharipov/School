using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace School.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly SchoolContext _context;

    public TeachersController(SchoolContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherResponseDTO>>> GetTeachers()
    {
        var entitys = await _context.Teachers.ToListAsync();
        var result = new List<TeacherResponseDTO>();
        foreach (var entity in entitys)
        {
            result.Add(TeacherResponseDTO.FromEntity(entity));
        }
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherResponseDTO>> GetTeacher(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }

        return teacher.ToResponseDto();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherResponseDTO>> PutTeacher(Guid id, TeacherRequestDTO teacherDto)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null)
        {
            NotFound();
        }

        teacher.MapFromDTO(teacherDto);

        _context.Teachers.Update(teacher);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeacherExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return teacher.ToResponseDto();
    }

    [HttpPost]
    public async Task<ActionResult<Teacher>> PostTeacher(TeacherRequestDTO teacherDto)
    {
        var teacher = new Teacher();
        teacher.MapFromDTO(teacherDto);
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTeacher", new { id = teacher.Guid }, teacher.ToResponseDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TeacherExists(Guid id)
    {
        return _context.Teachers.Any(e => e.Guid == id);
    }
}
