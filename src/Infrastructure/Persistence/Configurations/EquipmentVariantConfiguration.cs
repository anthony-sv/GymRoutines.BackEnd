using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class EquipmentVariantConfiguration : IEntityTypeConfiguration<EquipmentVariant>
{
    public void Configure(EntityTypeBuilder<EquipmentVariant> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
    }
}