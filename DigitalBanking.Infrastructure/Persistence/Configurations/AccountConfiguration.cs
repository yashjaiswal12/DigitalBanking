using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBanking.Infrastructure.Persistence.Configurations
{
    public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Accounts");

            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.Property(x => x.AccountNumber).HasMaxLength(20).IsRequired();
            entityTypeBuilder.HasIndex(x => x.AccountNumber).IsUnique();

            entityTypeBuilder.Property(x => x.CustomerId).HasMaxLength(20).IsRequired();
            entityTypeBuilder.HasIndex(x => x.CustomerId);
            entityTypeBuilder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
            entityTypeBuilder.Property(x => x.Status).IsRequired();
            entityTypeBuilder.Property(x => x.Type).IsRequired();

            entityTypeBuilder.Property(x => x.LedgerBalance).HasPrecision(19, 4);
            entityTypeBuilder.Property(x => x.AvailableBalance).HasPrecision(19, 4);
            entityTypeBuilder.Property(x => x.MinimumBalance).HasPrecision(19, 4);

            entityTypeBuilder.Property(x => x.OpenedOn).IsRequired(false);
            entityTypeBuilder.Property(x => x.FrozenOn).IsRequired(false);
            entityTypeBuilder.Property(x => x.ClosedOn).IsRequired(false);

            entityTypeBuilder.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(x => x.ModifiedBy).HasMaxLength(100).IsRequired(false);
            entityTypeBuilder.Property(x => x.CreatedOn).IsRequired();
            entityTypeBuilder.Property(x => x.ModifiedOn).IsRequired(false);

            entityTypeBuilder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();
        }
    }
}
