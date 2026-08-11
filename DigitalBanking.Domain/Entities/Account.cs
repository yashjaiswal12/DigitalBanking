using DigitalBanking.Domain.Enums;
using DigitalBanking.Domain.Exceptions;

namespace DigitalBanking.Domain.Entities
{
    public class Account
    {
        #region Properties
        public Guid Id { get; private set; }
        public string AccountNumber { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public AccountType Type { get; private set; }
        public AccountStatus Status { get; private set; }
        public string Currency { get; private set; } = string.Empty;
        public decimal LedgerBalance { get; private set; }
        public decimal AvailableBalance { get; private set; }
        public decimal MinimumBalance { get; private set; }
        public DateTimeOffset? OpenedOn { get; private set; }
        public DateTimeOffset? FrozenOn { get; private set; }
        public DateTimeOffset? ClosedOn { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public string CreatedBy { get; private set; } = string.Empty;
        public DateTimeOffset? ModifiedOn { get; private set; }
        public string ModifiedBy { get; private set;} = string.Empty;
        public byte[] RowVersion { get; private set; } = [];

        #endregion

        #region Constructors

        private Account() // Required by EF core
        {
        }

        private Account(Guid id, string accountNumber, Guid customerId, AccountType accountType, string currency,
            decimal initialBalance, decimal minimumBalance, DateTimeOffset createdOn, string createdBy)
        {
            ValidateIdentity(id);
            ValidateAccountNumber(accountNumber);
            ValidateCustomerId(customerId);
            ValidateCurrencyCode(currency);
            ValidateAccountType(accountType);
            ValidateBalances(initialBalance, minimumBalance);
            ValidateCreatedBy(createdBy);

            Id = id; 
            AccountNumber = accountNumber;
            CustomerId = customerId;
            Type = accountType;
            Currency = currency.ToUpperInvariant();
            LedgerBalance = initialBalance;
            AvailableBalance = initialBalance;
            MinimumBalance = minimumBalance;

            Status = AccountStatus.Pending;
            CreatedOn = createdOn;
            CreatedBy = createdBy;

            OpenedOn = null;
            FrozenOn = null;
            ClosedOn = null;
        }

        #endregion

        #region Behaviors

        public static Account Create(Guid id, string accountNumber, Guid customerId, AccountType accountType, string currency,
            decimal initialBalance, decimal minimumBalance, DateTimeOffset createdOn, string createdBy)
        {
            return new Account(id, accountNumber, customerId, accountType, currency, initialBalance, minimumBalance, createdOn, createdBy);
        }

        public void Credit(decimal amount, AccountStatus status)
        {
            ValidateAmount(amount);
            EnsureAccountCanProcessPayment(status);

            LedgerBalance += amount;
            AvailableBalance += amount;
        }

        public void Debit(decimal amount, AccountStatus status)
        {
            ValidateAmount(amount);
            EnsureAccountCanProcessPayment(status);

            if (amount > AvailableBalance)
                throw new InsufficientBalanceException();

            var resultingBalance = AvailableBalance - amount;
            if (resultingBalance < MinimumBalance)
                throw new MinimumBalanceViolationException();

            LedgerBalance -= amount;
            AvailableBalance = resultingBalance;
        }

        public void Activate(DateTimeOffset activatedOn)
        {
            if (Status != AccountStatus.Pending)
                throw new InvalidAccountStatusException($"Account cannot be activated from status {Status}");

            Status = AccountStatus.Active;
            CreatedOn = activatedOn;
            FrozenOn = null;
            ClosedOn = null;
        }

        public void Close(DateTimeOffset closedOn)
        {
            if (Status == AccountStatus.Closed)
                throw new AccountAlreadyClosedException();

            if (Status != AccountStatus.Active && Status != AccountStatus.Frozen)
                throw new InvalidAccountStatusException($"Account cannot be closed from status {Status}");

            if (LedgerBalance > 0)
                throw new InvalidAccountOperationException("An account cannot be closed while it has a non-zero ledger balance");

            Status = AccountStatus.Closed;
            ClosedOn = closedOn;
            FrozenOn = null;
        }

        public void Freeze(DateTimeOffset dateTimeOffset)
        {
            if (Status == AccountStatus.Closed)
                throw new AccountAlreadyClosedException();

            if (Status == AccountStatus.Frozen)
                throw new AccountAlreadyFrozenException();

            if (Status != AccountStatus.Active)
                throw new InvalidAccountStatusException($"Account cannot be frozen from status {Status}");

            Status = AccountStatus.Frozen;
            FrozenOn = dateTimeOffset;
        }

        public void Unfreeze()
        {
            if (Status == AccountStatus.Closed)
                throw new AccountAlreadyClosedException();

            if (Status != AccountStatus.Frozen)
                throw new InvalidAccountStatusException($"Account cannot be unfrozen from status {Status}");

            Status = AccountStatus.Active;
            FrozenOn = null;
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative");
        }

        private static void EnsureAccountCanProcessPayment(AccountStatus accountStatus)
        {
            if (accountStatus == AccountStatus.Closed)
                throw new AccountAlreadyClosedException();

            if (accountStatus == AccountStatus.Frozen)
                throw new InvalidAccountOperationException("Payment options are not allowed on frozen account");

            if (accountStatus != AccountStatus.Active)
                throw new InvalidAccountOperationException("Payment options are only allowed on active account");
        }

        private static void ValidateBalances(decimal initialBalance, decimal minimumBalance)
        {
            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance cannot be negative");

            if (minimumBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumBalance), "Minimum balance cannot be negative");

            if (initialBalance < minimumBalance)
                throw new ArgumentOutOfRangeException(nameof(minimumBalance), "Initial balance cannot be less than minimum balance");
        }

        private static void ValidateIdentity(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Account ID cannot be empty", nameof(id));
        }

        private static void ValidateAccountNumber(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number is required", nameof(accountNumber));
        }

        private static void ValidateCustomerId(Guid customerId)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer ID cannot be empty", nameof(customerId));
        }

        private static void ValidateCurrencyCode(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency is required", nameof(currency));

            if (currency.Trim().Length != 3)
                throw new ArgumentException("Currency must be a three-letter ISO currency code", nameof(currency));
        }

        private static void ValidateAccountType(AccountType accountType)
        {
            if (!Enum.IsDefined(accountType))
                throw new ArgumentOutOfRangeException(nameof(accountType), "Invalid account type");
        }

        private static void ValidateCreatedBy(string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("Created by is required", nameof(createdBy));
        }

        #endregion
    }
}

