namespace DigitalBanking.Application.Features.Statements.DTOs
{
    public sealed class FileExportDto
    {
        public byte[] Content { get; init; } = [];
        public string ContentType { get; init; } = "text/csv";
        public string FileName { get; init; }
    }
}
