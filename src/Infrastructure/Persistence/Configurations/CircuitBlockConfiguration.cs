using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class CircuitBlockConfiguration : IEntityTypeConfiguration<CircuitBlock>
{
    public void Configure(EntityTypeBuilder<CircuitBlock> builder)
    {
        builder.HasMany(c => c.Exercises)
               .WithOne(e => e.CircuitBlock)
               .HasForeignKey(e => e.CircuitBlockId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}