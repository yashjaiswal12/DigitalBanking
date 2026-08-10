using DigitalBanking.Application.Features.Customers.GetCustomerById;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Customers.GetCustomerById
{
    public class GetCustomerByIdQueryValidatorTest
    {
        private readonly GetCustomerByIdQueryValidator _validator;

        public GetCustomerByIdQueryValidatorTest()
        {
            _validator = new GetCustomerByIdQueryValidator();
        }

        [Theory]
        [InlineData("550e8400-e29b-41d4-a716-446655440000")]
        [InlineData("6ba7b810-9dad-11d1-80b4-00c04fd430c8")]
        public void Should_Return_True_If_Customer_Id_IS_Correct(string id)
        {
            //Arrange
            var customerId = Guid.Parse(id);

            var query = new GetCustomerByIdQuery { Id = customerId };

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_If_CustomerId_Is_Null()
        {
            var customerId = Guid.Empty;
            var query = new GetCustomerByIdQuery { Id = customerId };

            var result = _validator.Validate(query);

            result.IsValid.Should().BeFalse();
        }
    }
}
