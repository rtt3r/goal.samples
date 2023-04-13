using Goal.Samples.CQRS.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("Resources", "Identity");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasMaxLength(64)
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

            builder.HasIndex(p => new { p.ApplicationId, p.NormalizedName })
                .IsUnique();

            builder.HasMany(p => p.Permissions)
                .WithOne(p => p.Resource)
                .HasForeignKey(p => p.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
