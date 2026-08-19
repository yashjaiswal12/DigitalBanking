using DigitalBanking.Domain.Common;

namespace DigitalBanking.Domain.Events
{
    public sealed class FundsTransferredDomainEvent : IDomainEvent
    {
        public Guid TransactionId { get; }
        public Guid SourceAccountId { get; }
        public Guid DestinationAccountId { get; }
        public decimal Amount { get; }
        public string ReferenceNumber { get; }
        public DateTime OccuredOnUtc { get; }

        public FundsTransferredDomainEvent(Guid transactionId, Guid sourceAccountId, Guid destinationAccountId, decimal amount, 
            string referenceNumber)
        {
            TransactionId = transactionId;
            SourceAccountId = sourceAccountId; 
            DestinationAccountId = destinationAccountId;
            Amount = amount;
            ReferenceNumber = referenceNumber;
            OccuredOnUtc = DateTime.UtcNow;
        }
    }
}
