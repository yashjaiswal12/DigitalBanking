using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBanking.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("RefreshToken");

            entityTypeBuilder.Property(x => x.Token).IsRequired(true);
            entityTypeBuilder.Property(x => x.CustomerId).IsRequired(true);
            entityTypeBuilder.Property(x => x.ExpiresOn).IsRequired(true);

            entityTypeBuilder.HasIndex(x => x.Token).IsUnique();
            entityTypeBuilder.HasIndex(x => x.CustomerId);

            entityTypeBuilder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
