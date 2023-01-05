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
    public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
    {
        return await _context.Teachers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Teacher>> GetTeacher(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null)
        {
            return NotFound();
        }

        return teacher;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTeacher(Guid id, Teacher teacher)
    {
        if (id != teacher.Guid)
        {
            return BadRequest();
        }

        _context.Entry(teacher).State = EntityState.Modified;

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

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTeacher", new { id = teacher.Guid }, teacher);
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
