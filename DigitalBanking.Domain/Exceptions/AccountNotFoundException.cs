namespace DigitalBanking.Domain.Exceptions
{
    public class AccountNotFoundException : DomainException
    {
        public AccountNotFoundException() : base("Account not found")
        {
        }
    }
}
