namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidAccountStatusException : DomainException
    {
        public InvalidAccountStatusException(string message) : base(message)
        { 
        }
    }
}
