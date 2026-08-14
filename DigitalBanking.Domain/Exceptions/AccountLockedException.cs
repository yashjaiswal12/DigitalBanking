namespace DigitalBanking.Domain.Exceptions
{
    public class AccountLockedException : DomainException
    {
        public AccountLockedException(string message) : base(message)
        {
        }
    }
}
