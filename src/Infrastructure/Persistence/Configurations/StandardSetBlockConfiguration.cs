using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class StandardSetBlockConfiguration : IEntityTypeConfiguration<StandardSetBlock>
{
    public void Configure(EntityTypeBuilder<StandardSetBlock> builder)
    {
        builder.Property(s => s.MovementModifier).HasConversion<string>();
        builder.Property(s => s.IntensityTechnique).HasConversion<string>();
        builder.Property(s => s.DropSetScope).HasConversion<string>();
        builder.Property(s => s.TutTempo).HasMaxLength(20);

        builder.HasOne(s => s.Exercise)
               .WithMany()
               .HasForeignKey(s => s.ExerciseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Equipment)
               .WithMany()
               .HasForeignKey(s => s.EquipmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.EquipmentVariant)
               .WithMany()
               .HasForeignKey(s => s.EquipmentVariantId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}