using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.HasIndex(e => e.Name);
        builder.Property(e => e.Type).HasConversion<string>();
        builder.Property(e => e.DefaultCardioType).HasConversion<string>();

        // Store muscle groups as JSON arrays (PostgreSQL)
        builder.Property<List<MuscleGroup>>("_primaryMuscles")
               .HasColumnName("PrimaryMuscles")
               .HasColumnType("jsonb");

        builder.Property<List<MuscleGroup>>("_secondaryMuscles")
               .HasColumnName("SecondaryMuscles")
               .HasColumnType("jsonb");

        builder.HasOne(e => e.DefaultEquipment)
               .WithMany()
               .HasForeignKey(e => e.DefaultEquipmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.DefaultEquipmentVariant)
               .WithMany()
               .HasForeignKey(e => e.DefaultEquipmentVariantId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}