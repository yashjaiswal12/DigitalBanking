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
            entityTypeBuilder.Property(x => x.Email).IsRequired(true).HasMaxLength(256);
            entityTypeBuilder.Property(x => x.PhoneNumber).IsRequired(true).HasMaxLength(20);
            entityTypeBuilder.Property(x => x.PasswordHash).IsRequired(true).HasMaxLength(500);
            entityTypeBuilder.Property(x => x.IsActive).IsRequired(true);
            entityTypeBuilder.Property(x => x.CreatedBy).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.UpdatedBy).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.DeletedBy).HasMaxLength(100);

            entityTypeBuilder.Property(x => x.Role).HasConversion<int>().IsRequired();
            entityTypeBuilder.Property(x => x.SecurityStamp).IsRequired();
            entityTypeBuilder.Property(x => x.TokenVersion).IsRequired();
            entityTypeBuilder.Property(x => x.FailedLoginAttempts).IsRequired();
            entityTypeBuilder.Property(x => x.IsLocked).IsRequired();
            entityTypeBuilder.Property(x => x.LastLoginAt);
            entityTypeBuilder.Property(x => x.LastFailedLoginAt);

            entityTypeBuilder.HasIndex(x => x.Email).IsUnique(true);
            entityTypeBuilder.HasIndex(x => x.PhoneNumber).IsUnique(true);
            entityTypeBuilder.HasIndex(x => x.LastName);
            entityTypeBuilder.HasIndex(x => x.CreatedAtUtc);
            entityTypeBuilder.HasIndex(x => new {x.IsDeleted, x.IsActive, x.CreatedAtUtc});
            entityTypeBuilder.HasIndex(x => x.Role);

            entityTypeBuilder.HasQueryFilter(x => !x.IsDeleted);

            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken(true);
        }
    }
}
