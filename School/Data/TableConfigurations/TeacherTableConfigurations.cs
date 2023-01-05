using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace School.Data.TableConfigurations;

public class TeacherTableConfigurations : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable(nameof(Teacher));
        builder.HasKey(p => p.Guid);
        builder.Property(p => p.Guid).ValueGeneratedOnAdd();

        builder.HasMany(s => s.Groups)
            .WithOne(g => g.Teacher)
            .HasForeignKey(g => g.TeacherGuid)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
