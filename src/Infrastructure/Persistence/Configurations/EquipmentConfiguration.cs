using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Category).HasConversion<string>();
        builder.HasMany(e => e.Variants)
               .WithOne(v => v.Equipment)
               .HasForeignKey(v => v.EquipmentId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}