namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidBeneficiaryStateException : DomainException
    {
        public InvalidBeneficiaryStateException() : base("Invalid state change")
        {
        }
    }
}
