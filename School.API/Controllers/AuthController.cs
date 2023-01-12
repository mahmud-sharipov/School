using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.API.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(SchoolContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.FirstName == loginDto.Username && t.LastName == loginDto.Password);
        if (teacher == null)
        {
            return Unauthorized();
        }

        var jwtConfig = _configuration.GetSection("JWT");
        var claims = new List<Claim>
        {
            new Claim("userName", loginDto.Username),
            new Claim("userPass", loginDto.Password),
            new Claim(ClaimTypes.Sid, teacher.Guid.ToString()),
            new Claim("userEmail", teacher.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "student")
        };

        var exDate = DateTime.UtcNow.Add(TimeSpan.FromMinutes(3));
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"])), SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(jwtConfig["ValidIssuer"], jwtConfig["ValidAudience"], claims, DateTime.UtcNow, exDate, signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}


public class LoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}