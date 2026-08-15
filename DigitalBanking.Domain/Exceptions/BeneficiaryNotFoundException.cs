namespace DigitalBanking.Domain.Exceptions
{
    public class BeneficiaryNotFoundException : DomainException
    {
        public BeneficiaryNotFoundException() : base("Beneficiary not found")
        {
        }
    }
}
