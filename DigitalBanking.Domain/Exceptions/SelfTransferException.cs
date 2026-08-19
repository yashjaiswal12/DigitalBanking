namespace DigitalBanking.Domain.Exceptions
{
    public class SelfTransferException : DomainException
    {
        public SelfTransferException() : base("Self transfer not allowed")
        {
        }
    }
}
