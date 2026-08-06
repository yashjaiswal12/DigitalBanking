namespace DigitalBanking.Domain.Exceptions
{
    public class CustomerNotFoundException : DomainException
    {
        public CustomerNotFoundException() : base("Customer Not Found!")
        {
        }
    }
}
