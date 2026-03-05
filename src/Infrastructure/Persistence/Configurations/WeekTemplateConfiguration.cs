using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class WeekTemplateConfiguration : IEntityTypeConfiguration<WeekTemplate>
{
    public void Configure(EntityTypeBuilder<WeekTemplate> builder)
    {
        builder.HasKey(w => w.Id);
        builder.HasMany(w => w.Days)
               .WithOne(d => d.WeekTemplate)
               .HasForeignKey(d => d.WeekTemplateId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}