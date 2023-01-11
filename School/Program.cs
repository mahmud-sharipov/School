using Microsoft.EntityFrameworkCore;

namespace School;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();
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
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseRouting();
        app.MapControllers();
        app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
        //app.MapGet("me/info", (string id) =>
        //{
        //    return Results.Json("Mahdmu Sharipov | sharipov@mahmud.com");
        //});
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<SchoolContext>();
        context.Database.Migrate();
        app.Run();
    }
}