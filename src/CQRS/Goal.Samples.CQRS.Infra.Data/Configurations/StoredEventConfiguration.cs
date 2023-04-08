using Goal.Samples.CQRS.Infra.Data.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.CQRS.Infra.Data.Configurations
{
    internal sealed class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
    {
        public void Configure(EntityTypeBuilder<StoredEvent> builder)
        {
            builder.ToTable("StoredEvents");
            builder.HasKey(p => p.Id);

            builder.Property(c => c.EventType)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
