using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class FingerprintTemplateConfiguration : IEntityTypeConfiguration<FingerprintTemplate>
{
    public void Configure(EntityTypeBuilder<FingerprintTemplate> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.FingerIndex }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany(u => u.FingerprintTemplates)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}