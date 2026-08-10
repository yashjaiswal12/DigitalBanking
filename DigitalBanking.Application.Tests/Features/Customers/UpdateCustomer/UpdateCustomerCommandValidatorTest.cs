using DigitalBanking.Application.Features.Customers.Commands.UpdateCustomer;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandValidatorTest
    {
        private readonly UpdateCustomerCommandValidator _validator;

        public UpdateCustomerCommandValidatorTest()
        {
            _validator = new UpdateCustomerCommandValidator();
        }

        [Fact]
        public void Should_Be_True_If_Inputs_Are_Valid()
        {
            var command = new UpdateCustomerCommand()
            {
                CustomerId = Guid.NewGuid(),
                Email = "Test@gmail.com",
                FirstName = "fname",
                LastName = "lname",
                Phone = "1234567890"
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("123e4567-e89b-12d3-a456-426614174000", "fname", "lname", "abc@gmail.com", "1234567")]
        [InlineData("123e4567-e89b-12d3-a456-426614174001", "", "lname", "abc@gmail.com", "1234567890")]
        [InlineData("123e4567-e89b-12d3-a456-426614174002", "fname", "lname", "", "12345")]
        public void Should_Be_False_If_Inputs_Are_Invalid(string id, string fname, string lname, string email, string phone)
        {
            var command = new UpdateCustomerCommand()
            {
                CustomerId = Guid.Parse(id),
                Email = email,
                FirstName = fname,
                LastName = lname,
                Phone = phone
            };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}
