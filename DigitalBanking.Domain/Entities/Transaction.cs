using DigitalBanking.Domain.Common;
using DigitalBanking.Domain.Enums;
using DigitalBanking.Domain.Events;

namespace DigitalBanking.Domain.Entities
{
    public sealed class Transaction : AuditableEntity
    {
        #region Properties

        public string ReferenceNumber { get; private set; } = null!;
        public Guid SourceAccountId { get; private set; }
        public Guid DestinationAccountId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;
        public DateTime? CompletedAtUtc { get; private set; }
        public string? FailureReason { get; private set; }
        public byte[] RowVersion { get; private set; } = [];

        #endregion

        #region Constructors

        private Transaction()
        {
        }

        private Transaction(string referenceNumber, Guid sourceAccountId, Guid destinationAccountId, decimal amount, TransactionType type)
        {
            ReferenceNumber = referenceNumber;
            SourceAccountId = sourceAccountId;
            DestinationAccountId = destinationAccountId;
            Amount = amount;
            Type = type;
        }

        #endregion

        #region Methods

        public static Transaction Create(string referenceNumber, Guid sourceAccountId, Guid destinationAccountId, decimal amount, 
            TransactionType type)
        {
            if (string.IsNullOrWhiteSpace(referenceNumber))
                throw new ArgumentException("Reference number is required.", nameof(referenceNumber));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0.", nameof(amount));

            if (sourceAccountId == destinationAccountId)
                throw new ArgumentException("Source and destination accounts can not be same.");

            var transaction =  new Transaction(referenceNumber, sourceAccountId, destinationAccountId, amount, type);

            transaction.AddDomainEvent(new FundsTransferredDomainEvent(transaction.Id, transaction.SourceAccountId, transaction.DestinationAccountId,
                transaction.Amount, transaction.ReferenceNumber));
            
            return transaction;
        }

        public void MarkAsCompleted()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only pending transactions can be completed.");

            Status = TransactionStatus.Completed;
            CompletedAtUtc = DateTime.UtcNow;
        }

        public void MarkAsFailed(string reason)
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only pending transactions can failed.");

            Status = TransactionStatus.Failed;
            FailureReason = reason;
        }

        #endregion
    }
}
