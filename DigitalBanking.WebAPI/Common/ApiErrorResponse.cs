namespace DigitalBanking.WebAPI.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
