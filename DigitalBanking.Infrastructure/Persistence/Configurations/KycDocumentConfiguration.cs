using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBanking.Infrastructure.Persistence.Configurations
{
    public class KycDocumentConfiguration : IEntityTypeConfiguration<KycDocument>
    {
        public void Configure(EntityTypeBuilder<KycDocument> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("KycDocuments");
            
            entityTypeBuilder.Property(x => x.Id).IsRequired();
            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.Property(x => x.CustomerId).IsRequired();
            entityTypeBuilder.Property(x => x.Type).IsRequired();
            entityTypeBuilder.Property(x => x.Status).IsRequired();
            entityTypeBuilder.Property(x => x.OriginalFileName).IsRequired().HasMaxLength(255);
            entityTypeBuilder.Property(x => x.ContentType).IsRequired();
            entityTypeBuilder.Property(x => x.FileSize).IsRequired();
            entityTypeBuilder.Property(x => x.BlobContainer).IsRequired().HasMaxLength(100);
            entityTypeBuilder.Property(x => x.BlobName).IsRequired().HasMaxLength(500);
            entityTypeBuilder.Property(x => x.VerifiedOn).IsRequired();
            entityTypeBuilder.Property(x => x.UploadedOn).IsRequired();
            entityTypeBuilder.Property(x => x.RejectedOn).IsRequired();
            entityTypeBuilder.Property(x => x.RejectedReason).IsRequired().HasMaxLength(500);

            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();

            entityTypeBuilder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasIndex(x => x.CustomerId);
            entityTypeBuilder.HasIndex(x => new {x.CustomerId, x.Status});
        }
    }
}
