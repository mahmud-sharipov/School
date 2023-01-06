using Microsoft.EntityFrameworkCore;

namespace School.Data;

public class SchoolContext : DbContext, IContext
{
    public SchoolContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<EntityBase>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TableConfigurations.StudentTableConfigurations).Assembly);
    }
}