using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace School.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        return await _context.Groups.ToListAsync();
    }

    [HttpGet("conf")]
    public ActionResult<string> GetConf()
    {
        throw new Exception("Tu dalbayob!");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(Guid id)
    {
        var @group = await _context.Groups.FindAsync(id);

        if (@group == null)
        {
            return NotFound();
        }

        return @group;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(Guid id, Group @group)
    {
        if (id != @group.Guid)
        {
            return BadRequest();
        }

        _context.Entry(@group).State = EntityState.Modified;

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
