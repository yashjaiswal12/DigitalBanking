using DigitalBanking.Application.Features.Customers.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Customers.Queries.SearchCustomers
{
    public class SearchCustomerQuery : IRequest<List<Customer>>
    {
        public string SearchTerm { get; init; } = string.Empty;
        //public int Page { get; init; }
        //public int PageSize { get; init; }
        //public string? SortBy { get; init; }
        //public string? SortDirection { get; init; }
        public bool? IsActive { get; init; }
    }
}
