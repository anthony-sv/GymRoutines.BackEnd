using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class TrainingPlanConfiguration : IEntityTypeConfiguration<TrainingPlan>
{
    public void Configure(EntityTypeBuilder<TrainingPlan> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.StructureType).HasConversion<string>();

        builder.HasOne(p => p.Owner)
               .WithMany(u => u.TrainingPlans)
               .HasForeignKey(p => p.OwnerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.WeekTemplates)
               .WithOne(w => w.TrainingPlan)
               .HasForeignKey(w => w.TrainingPlanId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.OwnerId);
        builder.HasIndex(p => p.IsPublic);
    }
}