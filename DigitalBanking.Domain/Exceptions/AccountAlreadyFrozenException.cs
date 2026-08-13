namespace DigitalBanking.Domain.Exceptions
{
    public class AccountAlreadyFrozenException : DomainException
    {
        public AccountAlreadyFrozenException() : base("The account is already frozen")
        {
        }
    }
}
