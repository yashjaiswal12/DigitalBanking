namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidCustomerException : DomainException
    {
        public InvalidCustomerException(string message) : base(message)
        {
        }
    }
}
