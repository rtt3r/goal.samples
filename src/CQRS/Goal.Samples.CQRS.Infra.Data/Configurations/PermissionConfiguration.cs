using Goal.Samples.CQRS.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions", "Identity");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(p => p.OperationId)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(p => p.ResourceId)
                .HasMaxLength(64)
                .IsRequired();

            builder.HasIndex(p => new { p.ResourceId, p.OperationId })
                .IsUnique();
        }
    }
}
