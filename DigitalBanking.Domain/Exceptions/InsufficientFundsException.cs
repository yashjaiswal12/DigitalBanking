namespace DigitalBanking.Domain.Exceptions
{
    public class InsufficientFundsException : DomainException
    {
        public InsufficientFundsException() : base("No more transactions allowed. Insufficient Funds")
        {
        }
    }
}
