using DigitalBanking.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Transfers.Events
{
    public class FundsTransferredDomainEventHandler : INotificationHandler<FundsTransferredDomainEvent>
    {
        private readonly ILogger<FundsTransferredDomainEventHandler> _logger;

        public FundsTransferredDomainEventHandler(ILogger<FundsTransferredDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(FundsTransferredDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Funds transferred. Ref: {ReferenceNumber}", notification.ReferenceNumber);
            return Task.CompletedTask;
        }
    }
}
