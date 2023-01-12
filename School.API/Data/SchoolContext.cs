using Microsoft.EntityFrameworkCore;
using School.API.Abstractions;
using School.API.Data.TableConfigurations;
using School.API.Models;

namespace School.API.Data;

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentTableConfigurations).Assembly);
    }
}