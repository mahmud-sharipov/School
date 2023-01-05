using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace School.Data.TableConfigurations;

public class SubjectTableConfigurations : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable(nameof(Subject));
        builder.HasKey(p => p.Guid);
        builder.Property(p => p.Guid).ValueGeneratedOnAdd();
    }
}