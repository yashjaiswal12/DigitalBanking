using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBanking.Infrastructure.Persistence.Configurations
{
    public class BeneficiaryConfiguration : IEntityTypeConfiguration<Beneficiary>
    {
        public void Configure(EntityTypeBuilder<Beneficiary> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.Id).IsRequired().HasMaxLength(20);
            entityTypeBuilder.Property(x => x.CustomerId).IsRequired().HasMaxLength(20);
            entityTypeBuilder.Property(x => x.AccountId).IsRequired().HasMaxLength(20);
            entityTypeBuilder.Property(x => x.BeneficiaryName).IsRequired().HasMaxLength(100);
            entityTypeBuilder.Property(x => x.BeneficiaryBankName).IsRequired().HasMaxLength(100);
            entityTypeBuilder.Property(x => x.BeneficiaryAccountNumber).IsRequired().HasMaxLength(20);
            entityTypeBuilder.Property(x => x.BeneficiaryBankCode).IsRequired();
            entityTypeBuilder.Property(x => x.Status).IsRequired();
            entityTypeBuilder.Property(x => x.VerifiedAt).IsRequired();

            entityTypeBuilder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasOne<Account>().WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasQueryFilter(x => !x.IsDeleted);

            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.HasIndex(x => new {x.CustomerId, x.BeneficiaryAccountNumber}).IsUnique();
            entityTypeBuilder.HasIndex(x => x.CustomerId);

            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken(true);
        }
    }
}
