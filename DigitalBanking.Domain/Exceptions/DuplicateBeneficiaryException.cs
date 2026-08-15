namespace DigitalBanking.Domain.Exceptions
{
    public class DuplicateBeneficiaryException : DomainException
    {
        public DuplicateBeneficiaryException() : base("Beneficiary with provided details already exists")
        {
        }
    }
}
