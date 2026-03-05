using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class CardioBlockConfiguration : IEntityTypeConfiguration<CardioBlock>
{
    public void Configure(EntityTypeBuilder<CardioBlock> builder)
    {
        builder.Property(c => c.CardioType).HasConversion<string>();

        builder.HasOne(c => c.Equipment)
               .WithMany()
               .HasForeignKey(c => c.EquipmentId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}