namespace DigitalBanking.Domain.Exceptions
{
    public class InvalidCustomerException : DomainException
    {
        public InvalidCustomerException(string email) : base($"Invalid details of customer {email}")
        {
        }
    }
}
