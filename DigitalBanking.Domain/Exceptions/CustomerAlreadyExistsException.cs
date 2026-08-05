namespace DigitalBanking.Domain.Exceptions
{
    public sealed class CustomerAlreadyExistsException : DomainException
    {
        public CustomerAlreadyExistsException(string email) : base($"A customer with email '{email}' already exists.")
        {
        }
    }
}
