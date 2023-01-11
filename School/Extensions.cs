using Microsoft.EntityFrameworkCore;
namespace School;

public static class Extensions
{
    public static IServiceCollection AddAll(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<SchoolContext>(options =>
        {
            options.UseLazyLoadingProxies();
            //options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        }, ServiceLifetime.Scoped);
        return services;
    }
}