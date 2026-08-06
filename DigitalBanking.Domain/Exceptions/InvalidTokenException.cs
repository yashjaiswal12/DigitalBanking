namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidTokenException : DomainException
    {
        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}
