namespace DigitalBanking.Domain.Exceptions
{
    public class KycDocumentNotFoundException : DomainException
    {
        public KycDocumentNotFoundException() : base ("Document not found")
        {
        }

        public KycDocumentNotFoundException(string message) : base (message)
        {
        }
    }
}
