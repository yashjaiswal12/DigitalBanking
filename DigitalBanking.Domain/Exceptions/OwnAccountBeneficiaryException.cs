namespace DigitalBanking.Domain.Exceptions
{
    public class OwnAccountBeneficiaryException : DomainException
    {
        public OwnAccountBeneficiaryException() : base("Cannot add self account as beneficiary")
        {
        }
    }
}
