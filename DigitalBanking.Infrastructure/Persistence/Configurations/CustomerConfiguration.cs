using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBanking.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Customer");

            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.LastName).IsRequired(true).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.Email).IsRequired(true).HasMaxLength(200);
            entityTypeBuilder.Property(x => x.PhoneNumber).IsRequired(true).HasMaxLength(20);
            entityTypeBuilder.Property(x => x.PasswordHash).IsRequired(true);
            entityTypeBuilder.Property(x => x.IsActive).IsRequired(true);

            entityTypeBuilder.HasIndex(x => x.Email).IsUnique(true);
        }
    }
}
