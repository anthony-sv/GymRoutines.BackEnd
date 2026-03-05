using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class CircuitExerciseConfiguration : IEntityTypeConfiguration<CircuitExercise>
{
    public void Configure(EntityTypeBuilder<CircuitExercise> builder)
    {
        builder.HasKey(ce => ce.Id);
        builder.Property(ce => ce.MovementModifier).HasConversion<string>();

        builder.HasOne(ce => ce.Exercise)
               .WithMany()
               .HasForeignKey(ce => ce.ExerciseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ce => ce.Equipment)
               .WithMany()
               .HasForeignKey(ce => ce.EquipmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ce => ce.EquipmentVariant)
               .WithMany()
               .HasForeignKey(ce => ce.EquipmentVariantId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(ce => new { ce.CircuitBlockId, ce.Order });
    }
}