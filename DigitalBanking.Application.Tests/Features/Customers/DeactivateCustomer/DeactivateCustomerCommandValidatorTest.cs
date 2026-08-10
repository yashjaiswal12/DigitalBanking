using DigitalBanking.Application.Features.Customers.Commands.DeactivateCustomer;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Customers.DeactivateCustomer
{
    public class DeactivateCustomerCommandValidatorTest
    {
        private readonly DeactivateCustomerCommandValidator _validator;
        
        public DeactivateCustomerCommandValidatorTest()
        {
            _validator = new DeactivateCustomerCommandValidator();
        }

        [Fact]
        public void Should_Return_True_If_Data_Is_Correct()
        {
            var customerId = Guid.NewGuid();
            
            var command = new DeactivateCustomerCommand { CustomerId = customerId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_If_Data_Is_Empty()
        {
            var customerId = Guid.Empty;

            var command = new DeactivateCustomerCommand { CustomerId = customerId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}
