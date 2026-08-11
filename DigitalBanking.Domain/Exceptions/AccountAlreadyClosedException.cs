namespace DigitalBanking.Domain.Exceptions
{
    public class AccountAlreadyClosedException : DomainException
    {
        public AccountAlreadyClosedException() : base("The account is already closed")
        {
        }
    }
}
