namespace DigitalBanking.Domain.Exceptions
{
    public class MinimumBalanceViolationException : DomainException
    {
        public MinimumBalanceViolationException() : base("The operation would violate the account minimum balance")
        {
        }
    }
}
