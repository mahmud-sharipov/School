using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.API.Models;

namespace School.API.Data.TableConfigurations;

public class StudentTableConfigurations : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable(nameof(Student));
        builder.HasKey(p => p.Guid);
        builder.Property(p => p.Guid).ValueGeneratedOnAdd();

        builder.HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(g => g.GroupGuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}