using DigitalBanking.Application.Interfaces.Services;
using DigitalBanking.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Infrastructure.Persistence
{
    // Find domain events
    // Publish through MediatR
    // Clear events

    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DomainEventDispatcher> _logger;
        private readonly ApplicationDbContext _context;

        public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger, ApplicationDbContext context)
        {
            _mediator = mediator;
            _logger = logger;
            _context = context;
        }

        public async Task DispatchAsync(CancellationToken cancellationToken)
        {
            var domainEvents = _context.ChangeTracker.Entries<BaseEntity>().SelectMany(x => x.Entity.DomainEvents).ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            foreach (var entity in _context.ChangeTracker.Entries<BaseEntity>())
            {
                entity.Entity.ClearDomainEvents();
            }

            _logger.LogInformation("");
        }
    }
}
