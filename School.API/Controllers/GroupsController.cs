using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.API.Data;
using School.API.DTO.Group;
using School.API.Models;
using System.Security.Claims;

namespace School.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "teacher")]
public class GroupsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public GroupsController(SchoolContext context, IConfiguration configuration, ILogger<GroupsController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupResponseDTO>>> GetGroups()
    {
        if (!User.HasClaim(c => c.Type == ClaimTypes.Sid))
            return Forbid();

        var teacherId = new Guid(User.FindFirst(ClaimTypes.Sid).Value);

        var groups = await _context.Groups.Where(g => g.TeacherGuid == teacherId).ToListAsync();
        var result = new List<GroupResponseDTO>();
        foreach (var group in groups)
        {
            result.Add(new GroupResponseDTO
            {
                Division = group.Division,
                Grade = group.Grade,
                Guid = group.Guid,
                TeacherGuid = group.TeacherGuid
            });
        }
        return result;
    }

    [HttpGet("conf")]
    public ActionResult<string> GetConf()
    {
        throw new Exception("Tu dalbayob!");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponseDTO>> GetGroup(Guid id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        var entity = new GroupResponseDTO()
        {
            Division = group.Division,
            Grade = group.Grade,
            Guid = group.Guid,
            TeacherGuid = group.TeacherGuid
        };

        return entity;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GroupResponseDTO>> PutGroup(Guid id, GroupRequestDTO groupDto)
    {


        _context.Entry(groupDto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return Forbid();
    }

    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(Group @group)
    {
        _context.Groups.Add(@group);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGroup", new { id = @group.Guid }, @group);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        var @group = await _context.Groups.FindAsync(id);
        if (@group == null)
        {
            return NotFound();
        }

        _context.Groups.Remove(@group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupExists(Guid id)
    {
        return _context.Groups.Any(e => e.Guid == id);
    }
}
