using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace School.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CookiesController : ControllerBase
{
    public CookiesController()
    {
    }

    [HttpGet]
    public ActionResult<IEnumerable<string>> GetCookies()
    {
        return Ok(Request.Cookies.Select(c => $"{c.Key}: {c.Value}"));
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<string>> GetCooky(string id)
    {
        if (Request.Cookies.TryGetValue(id, out string value))
            return Ok(value);

        return NotFound(id);
    }

    [HttpPost]
    public IActionResult CreateCooky([FromBody] Cooky cooky)
    {
        Response.Cookies.Append(cooky.Id, cooky.Value);
        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult<IEnumerable<string>> UpdateCooky(string id, [FromBody] string value)
    {
        if (Request.Cookies.ContainsKey(id))
        {
            Response.Cookies.Append(id, value);
            return Ok();
        }

        return NotFound(id);
    }

    [HttpDelete("{id}")]
    public ActionResult<IEnumerable<string>> DeleteCooky(string id)
    {
        if (Request.Cookies.ContainsKey(id))
        {
            Response.Cookies.Delete(id);
            return Ok();
        }

        return NotFound(id);
    }
}
