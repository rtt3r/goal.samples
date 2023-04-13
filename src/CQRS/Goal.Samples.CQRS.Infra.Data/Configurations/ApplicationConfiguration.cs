using Goal.Samples.CQRS.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications", "Identity");
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

            builder.HasIndex(p => p.NormalizedName)
                .IsUnique();

            builder.HasMany(p => p.Roles)
                .WithOne(p => p.Application)
                .HasForeignKey(p => p.ApplicationId);

            builder.HasMany(p => p.Resources)
                .WithOne(p => p.Application)
                .HasForeignKey(p => p.ApplicationId);

            builder.HasMany(p => p.Operations)
                .WithOne(p => p.Application)
                .HasForeignKey(p => p.ApplicationId);
        }
    }
}
