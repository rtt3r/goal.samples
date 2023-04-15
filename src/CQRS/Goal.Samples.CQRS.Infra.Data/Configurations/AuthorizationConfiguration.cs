using Goal.Samples.CQRS.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class AuthorizationConfiguration : IEntityTypeConfiguration<Authorization>
    {
        public void Configure(EntityTypeBuilder<Authorization> builder)
        {
            builder.ToTable("Authorizations", "Identity");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(p => p.PermissionId)
                .HasMaxLength(64);

            builder.Property(p => p.UserId)
                .HasMaxLength(450);

            builder.Property(p => p.RoleId)
                .HasMaxLength(450);

            builder.HasIndex(p => new { p.PermissionId, p.UserId })
                .IsUnique()
                .HasFilter($"[{nameof(Authorization.UserId)}] IS NOT NULL");

            builder.HasIndex(p => new { p.PermissionId, p.RoleId })
                .IsUnique()
                .HasFilter($"[{nameof(Authorization.RoleId)}] IS NOT NULL");
        }
    }
}