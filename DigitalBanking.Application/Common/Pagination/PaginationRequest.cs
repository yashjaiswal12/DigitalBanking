namespace DigitalBanking.Application.Common.Pagination
{
    public abstract class PaginationRequest
    {
        public int Page { get; init; } = PaginationConstants.DefaultPage;
        public int PageSize { get; init; } = PaginationConstants.DefaultPageSize;
    }
}
