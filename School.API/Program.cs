using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.API.Data;
using System.Security.Claims;
using System.Text;

namespace School.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
             {
                 var configuration = builder.Configuration.GetSection("JWT");
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = configuration["ValidIssuer"],
                     ValidateAudience = true,
                     ValidAudience = configuration["ValidAudience"],
                     ValidateLifetime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"])),
                     ValidateIssuerSigningKey = true,
                 };
             });

        builder.Services.AddAuthorization(opts =>
        {

            opts.AddPolicy("ForStudents", policy =>
            {
                policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "student");
            });

            opts.AddPolicy("ForTeachers", policy =>
            {
                policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "teacher");
            });
        });

        builder.Services.AddDbContext<SchoolContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        }, ServiceLifetime.Scoped);

        var app = builder.Build();
        //app.UseMiddleware<Middlewares.ApiExceptionHandlingMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseCors(options =>
        {
            options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //options.WithOrigins("school.UI.com", "school.mobile.com").AllowAnyMethod().AllowAnyHeader();
            //options.WithOrigins("ntc.tj").WithMethods("GET POST");

            //options.AllowAnyOrigin().WithMethods("GET");
        });
        app.UseHttpsRedirection();

        app.UseAuthentication();   // добавление middleware аутентификации 
        app.UseAuthorization();   // добавление middleware авторизации

        app.MapControllers();
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<SchoolContext>();
        context.Database.Migrate();
        app.Run();
    }
}