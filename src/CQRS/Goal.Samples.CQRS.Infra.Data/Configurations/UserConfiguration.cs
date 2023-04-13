using Goal.Samples.CQRS.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "Identity");
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

            builder.Property(p => p.UserName)
                .HasMaxLength(256);

            builder.Property(p => p.NormalizedUserName)
                .HasMaxLength(256);

            builder.Property(p => p.Email)
                .HasMaxLength(256);

            builder.Property(p => p.NormalizedEmail)
                .HasMaxLength(256);

            builder.Property(p => p.ConcurrencyStamp)
                .IsConcurrencyToken();

            builder.HasIndex(p => p.NormalizedUserName)
                .IsUnique();

            builder.HasIndex(p => p.NormalizedEmail)
                .IsUnique();

            builder.HasMany(p => p.Authorizations)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}
