using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalBanking.Infrastructure.Persistence.Configurations
{
    public class TransferFundsConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Transactions");

            entityTypeBuilder.Property(x => x.Id).IsRequired().HasMaxLength(20);
            entityTypeBuilder.Property(x => x.SourceAccountId).IsRequired();
            entityTypeBuilder.Property(x => x.DestinationAccountId).IsRequired();
            entityTypeBuilder.Property(x => x.ReferenceNumber).IsRequired().HasMaxLength(50);
            entityTypeBuilder.Property(x => x.Amount).IsRequired().HasPrecision(18, 2);
            entityTypeBuilder.Property(x => x.CreatedAtUtc).IsRequired();
            entityTypeBuilder.Property(x => x.Status).IsRequired();
            entityTypeBuilder.Property(x => x.Type).IsRequired();
            entityTypeBuilder.Property(x => x.FailureReason).HasMaxLength(500);
            entityTypeBuilder.Property(x => x.CompletedAtUtc);

            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.HasIndex(x => x.SourceAccountId);
            entityTypeBuilder.HasIndex(x => x.DestinationAccountId);
            entityTypeBuilder.HasIndex(x => x.ReferenceNumber).IsUnique();
            
            entityTypeBuilder.HasOne<Account>().WithMany().HasForeignKey(x => x.SourceAccountId).OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasOne<Account>().WithMany().HasForeignKey(x => x.DestinationAccountId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
