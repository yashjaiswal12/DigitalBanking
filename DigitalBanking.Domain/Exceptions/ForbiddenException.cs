namespace DigitalBanking.Domain.Exceptions
{
    public class ForbiddenException : DomainException
    {
        public ForbiddenException() : base("Forbidden action")
        {
        }
    }
}
