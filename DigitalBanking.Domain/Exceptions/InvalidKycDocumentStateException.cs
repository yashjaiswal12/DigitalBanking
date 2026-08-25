namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidKycDocumentStateException : DomainException
    {
        public InvalidKycDocumentStateException(string message) : base(message)
        {
        }
    }
}
