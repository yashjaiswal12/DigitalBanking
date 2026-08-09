using DigitalBanking.Application.Features.Customers.SearchCustomers.Queries;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Customers.SearchCustomers
{
    // Arrange Act Assert

    public class SearchCustomerQueryValidatorTests
    {
        private readonly SearchCustomerQueryValidator _validator;

        public SearchCustomerQueryValidatorTests()
        {
            _validator = new SearchCustomerQueryValidator();
        }

        [Fact]
        public void Should_Return_True_If_SearchTerm_Is_Valid()
        {
            // Arrange
            var query = new SearchCustomerQuery { SearchTerm = "Yash" };

            //Act
            var result = _validator.Validate(query);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_When_SerachTerm_Is_Empty()
        {
            var query = new SearchCustomerQuery { SearchTerm = string.Empty };
            var result = _validator.Validate(query);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(SearchCustomerQuery.SearchTerm));
        }

        [Fact]
        public void Should_Return_False_When_Input_Is_Short()
        {
            var query = new SearchCustomerQuery { SearchTerm = "Y" };
            var result = _validator.Validate(query);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_False_When_Input_Is_High()
        {
            string input = new string('A', 101);
            var query = new SearchCustomerQuery { SearchTerm = input };
            var result = _validator.Validate(query);
            result.IsValid.Should().BeFalse();
        }
    }
}
