namespace DigitalBanking.Domain.Exceptions
{
    public class AccountFrozenException : DomainException
    {
        public AccountFrozenException() : base("Transactions not allowed. Account is freezed")
        {
        }
    }
}
