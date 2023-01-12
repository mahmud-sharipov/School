using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.API.Models;

namespace School.API.Data.TableConfigurations;

public class GroupTableConfigurations : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable(nameof(Group));
        builder.HasKey(p => p.Guid);
        builder.Property(p => p.Guid).ValueGeneratedOnAdd();

        builder.HasOne(s => s.Teacher)
            .WithMany(g => g.Groups)
            .HasForeignKey(g => g.TeacherGuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
