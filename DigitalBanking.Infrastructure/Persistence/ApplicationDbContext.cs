using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Domain.Common;
using DigitalBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUserService _currentUserService;

        public DbSet<Customer> Customers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions, 
            IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService) : base(dbContextOptions)
        {  
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var entity in entries)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.SetCreatedBy(_currentUserService.CustomerId, _dateTimeProvider.UtcNow);
                        break;
                    case EntityState.Modified:
                        entity.Entity.MarkAsUpdated(_currentUserService.CustomerId, _dateTimeProvider.UtcNow);
                        break;
                    case EntityState.Deleted:
                        entity.State = EntityState.Modified;
                        entity.Entity.MarkAsDeleted(_currentUserService.CustomerId, _dateTimeProvider.UtcNow);
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        // keeps entity mapping separate from entities and follows the Single Responsibility Principle.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
