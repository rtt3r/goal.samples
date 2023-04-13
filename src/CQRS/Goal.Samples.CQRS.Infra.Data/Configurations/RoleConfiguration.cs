using Goal.Samples.CQRS.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles", "Identity");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.NormalizedName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(512);

            builder.Property(p => p.ApplicationId)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(p => p.ConcurrencyStamp)
                .IsConcurrencyToken();

            builder.HasIndex(p => new { p.ApplicationId, p.NormalizedName })
                .IsUnique();

            builder.HasMany(e => e.UserMembers)
                .WithMany(e => e.MemberOf)
                .UsingEntity("UserRoles");

            builder.HasMany(e => e.RoleMembers)
                .WithMany(e => e.MemberOf)
                .UsingEntity(
                    "RoleRoles",
                    l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                    r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("MemberId").HasPrincipalKey(nameof(Role.Id)),
                    j => j.HasKey("RoleId", "MemberId"));

            builder.HasMany(p => p.Authorizations)
                .WithOne(p => p.Role)
                .HasForeignKey(p => p.RoleId);
        }
    }
}
