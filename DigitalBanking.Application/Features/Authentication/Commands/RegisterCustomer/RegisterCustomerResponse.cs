namespace DigitalBanking.Application.Features.Authentication.Commands.RegisterCustomer
{
    public sealed class RegisterCustomerResponse
    {
        public Guid CustomerId { get; set; }
        public string Message { get; set; } = string.Empty;

        public RegisterCustomerResponse(Guid customerId, string message)
        {
            CustomerId = customerId;
            Message = message;
        }
    }
}
