using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace School.API.Data;

public class SchoolContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile($"appsettings.json", optional: true)
                          .AddJsonFile($"appsettings.Development.json", optional: true)
                          .AddEnvironmentVariables()
                          .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseLazyLoadingProxies();
        var connnectionString = configuration.GetConnectionString("SqlServer");
        optionsBuilder.UseSqlServer(connnectionString);

        return new DbContext(optionsBuilder.Options);
    }
}
