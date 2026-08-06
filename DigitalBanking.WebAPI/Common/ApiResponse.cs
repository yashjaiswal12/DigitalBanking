namespace DigitalBanking.WebAPI.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; } = true;
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }
        public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
    }
}
