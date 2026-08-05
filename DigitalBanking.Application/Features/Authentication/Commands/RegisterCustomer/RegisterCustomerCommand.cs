using MediatR;

namespace DigitalBanking.Application.Features.Authentication.Commands.RegisterCustomer
{
    // Vertical Slice Architecture - Instead of creating one large commands folder we're scaling based on organizing the feature
    // init prevents acciedental modificatons as commands are immutable and should not change once created

    public sealed class RegisterCustomerCommand : IRequest<RegisterCustomerResponse>
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}
