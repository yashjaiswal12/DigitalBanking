namespace DigitalBanking.Domain.Exceptions
{
    public class InsufficientBalanceException : DomainException
    {
        public InsufficientBalanceException() : base("The account does not have sufficient available balance")
        {
        }
    }
}
