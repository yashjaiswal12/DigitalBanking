using DigitalBanking.Domain.Common;
using DigitalBanking.Domain.Enums;
using DigitalBanking.Domain.Exceptions;

namespace DigitalBanking.Domain.Entities
{
    public class Beneficiary : AuditableEntity
    {
        #region Properties

        public Guid CustomerId { get; private set; }
        public Guid AccountId { get; private set; }
        public string BeneficiaryName { get; private set; } = string.Empty;
        public string BeneficiaryAccountNumber { get; private set; } = string.Empty;
        public string BeneficiaryBankCode { get; private set; } = string.Empty;
        public string BeneficiaryBankName { get; private set; } = string.Empty;
        public BeneficiaryStatus Status { get; private set; } = BeneficiaryStatus.PendingVerification;
        public DateTime? VerifiedAt { get; private set; }

        #endregion

        #region Constructors

        private Beneficiary()
        {
        }

        private Beneficiary(Guid customerId, Guid accountId, string beneficiaryName, string bankCode, string beneficiaryAccountNumber,
            string beneficiaryBankName)
        {
            ValidateCustomerId(customerId);
            ValidateAccountId(accountId);
            ValidateBeneficiaryName(beneficiaryName);
            ValidateBeneficiaryAccountNumber(beneficiaryAccountNumber);
            ValidateBeneficiaryBankName(beneficiaryBankName);
            ValidateBeneficiaryBankCode(bankCode);

            CustomerId = customerId;
            AccountId = accountId;
            BeneficiaryName = beneficiaryName;
            BeneficiaryBankName = beneficiaryBankName;
            BeneficiaryBankCode = bankCode;
            BeneficiaryAccountNumber = beneficiaryAccountNumber;
        }

        #endregion

        #region Methods

        public static Beneficiary Create(Guid customerId, Guid accountId, string beneficiaryName, string bankCode, string beneficiaryAccountNumber,
            string beneficiaryBankName)
        {
            return new Beneficiary(customerId, accountId, beneficiaryName, bankCode, beneficiaryAccountNumber, beneficiaryBankName);
        }

        public void Verify()
        {
            if (Status == BeneficiaryStatus.Removed)
                throw new InvalidBeneficiaryStateException();

            if (Status == BeneficiaryStatus.Verified)
                throw new InvalidBeneficiaryStateException();

            Status = BeneficiaryStatus.Verified;
            VerifiedAt = DateTime.UtcNow;
        }

        public void Remove()
        {
            if (Status == BeneficiaryStatus.Removed)
                throw new InvalidBeneficiaryStateException();

            Status = BeneficiaryStatus.Removed;
        }

        private static void ValidateCustomerId(Guid customerId)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer id cannot be empty", nameof(customerId));
        }

        private static void ValidateAccountId(Guid customerId)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Account id cannot be empty", nameof(customerId));
        }

        private static void ValidateBeneficiaryName(string beneficiaryName)
        {
            if (string.IsNullOrWhiteSpace(beneficiaryName))
                throw new ArgumentException("Beneficiary name is required", nameof(beneficiaryName));
        }

        private static void ValidateBeneficiaryBankName(string beneficiaryBankName)
        {
            if (string.IsNullOrWhiteSpace(beneficiaryBankName))
                throw new ArgumentException("Beneficiary bank name is required", nameof(beneficiaryBankName));
        }

        private static void ValidateBeneficiaryAccountNumber(string beneficiaryAccountNumber)
        {
            if (string.IsNullOrWhiteSpace(beneficiaryAccountNumber))
                throw new ArgumentException("Beneficiary account number is required", nameof(beneficiaryAccountNumber));
        }

        private static void ValidateBeneficiaryBankCode(string beneficiaryBankCode)
        {
            if (string.IsNullOrWhiteSpace(beneficiaryBankCode))
                throw new ArgumentException("Beneficiary bank code is required", nameof(beneficiaryBankCode));
        }

        #endregion
    }
}
