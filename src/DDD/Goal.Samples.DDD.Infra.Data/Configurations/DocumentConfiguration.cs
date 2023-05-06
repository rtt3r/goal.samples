using Goal.Samples.DDD.Domain.People.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Goal.Samples.DDD.Infra.Data.Configurations
{
    internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Type)
                .IsRequired();

            builder.Property(p => p.Number)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
