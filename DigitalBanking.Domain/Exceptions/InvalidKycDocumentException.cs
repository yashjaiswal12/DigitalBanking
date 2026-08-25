namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidKycDocumentException : DomainException
    {
        public InvalidKycDocumentException() : base("Invalid document")
        {
        }
    }
}
