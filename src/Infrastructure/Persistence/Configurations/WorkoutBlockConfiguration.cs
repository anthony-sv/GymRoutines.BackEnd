using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class WorkoutBlockConfiguration : IEntityTypeConfiguration<WorkoutBlock>
{
    public void Configure(EntityTypeBuilder<WorkoutBlock> builder)
    {
        // Table-Per-Hierarchy
        builder.HasKey(b => b.Id);
        builder.HasDiscriminator<string>("BlockDiscriminator")
               .HasValue<StandardSetBlock>("StandardSet")
               .HasValue<CircuitBlock>("Circuit")
               .HasValue<CardioBlock>("Cardio");

        builder.Property(b => b.BlockType).HasConversion<string>();
        builder.HasIndex(b => new { b.WorkoutDayId, b.Order });
    }
}