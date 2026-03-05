using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class WorkoutDayConfiguration : IEntityTypeConfiguration<WorkoutDay>
{
    public void Configure(EntityTypeBuilder<WorkoutDay> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.DayType).HasConversion<string>();
        builder.Property(d => d.Label).HasMaxLength(100);

        builder.HasMany(d => d.Blocks)
               .WithOne(b => b.WorkoutDay)
               .HasForeignKey(b => b.WorkoutDayId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => new { d.WeekTemplateId, d.DayOfWeek }).IsUnique();
    }
}