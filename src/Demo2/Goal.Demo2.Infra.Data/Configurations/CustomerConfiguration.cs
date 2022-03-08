using Goal.Demo2.Domain.Aggregates.Customers;
using Goal.Domain.Seedwork.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Demo2.Infra.Data.Configurations
{
    internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.BirthDate)
                .IsRequired();
        }
    }

    internal sealed class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
    {
        public void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            builder.ToTable("StoredEvents");
            builder.HasKey(p => p.Id);

            builder.Property(c => c.MessageType)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
