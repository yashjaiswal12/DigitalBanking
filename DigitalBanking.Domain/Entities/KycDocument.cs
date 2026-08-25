using DigitalBanking.Domain.Common;
using DigitalBanking.Domain.Enums;
using DigitalBanking.Domain.Exceptions;

namespace DigitalBanking.Domain.Entities
{
    public sealed class KycDocument : AuditableEntity
    {
        #region Properties

        public Guid CustomerId { get; private set; }
        public DocumentType Type { get; private set; }
        public DocumentStatus Status { get; private set; }
        public string OriginalFileName { get; private set; } = string.Empty;
        public string BlobContainer { get; private set; } = string.Empty;
        public string BlobName { get; private set; } = string.Empty;
        public string ContentType { get; private set; } = string.Empty;
        public int FileSize { get; private set; }
        public DateTime UploadedOn { get; private set; }
        public DateTime? VerifiedOn { get; private set; }
        public DateTime? RejectedOn { get; private set; }
        public string? RejectedReason { get; private set; }
        public byte[] RowVersion { get; private set; } = [];

        #endregion

        #region Constructors

        private KycDocument()
        {
        }

        private KycDocument(Guid customerId, DocumentType type, string originalFileName, string blobContainer, string blobName,
            string contentType, int fileSize)
        {
            CustomerId = customerId;
            Type = type;
            Status = DocumentStatus.Uploaded;
            OriginalFileName = originalFileName;
            BlobContainer = blobContainer;
            BlobName = blobName;
            ContentType = contentType;
            FileSize = fileSize;
            UploadedOn = DateTime.UtcNow;
        }

        #endregion

        #region Methods

        public KycDocument Create(Guid customerId, DocumentType type, string originalFileName, string blobContainer, string blobName,
            string contentType, int fileSize)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer id cannot be empty", nameof(customerId));

            if (!Enum.IsDefined(type))
                throw new ArgumentException("Document type is invalid", nameof(type));

            if (string.IsNullOrWhiteSpace(originalFileName))
                throw new ArgumentException("File name is required", nameof(originalFileName));

            if (string.IsNullOrWhiteSpace(blobContainer))
                throw new ArgumentException("Blob container is required", nameof(blobContainer));

            if (string.IsNullOrWhiteSpace(blobName))
                throw new ArgumentException("Blob name is required", nameof(blobName));

            if (string.IsNullOrWhiteSpace(contentType))
                throw new ArgumentException("ContentType is required", nameof(contentType));

            if (fileSize <= 0)
                throw new ArgumentException("File size should be greater than 0", nameof(fileSize));

            return new KycDocument(customerId, type, originalFileName, blobContainer, blobName, contentType, fileSize);
        }

        public void StartReview()
        {
            EnsureValidStatus(DocumentStatus.Uploaded);
            Status = DocumentStatus.UnderReview;
        }

        public void Verify(DateTime verifiedOn)
        {
            EnsureValidStatus(DocumentStatus.UnderReview);

            Status = DocumentStatus.Verified;
            VerifiedOn = verifiedOn;
            RejectedOn = null;
            RejectedReason = null;
        }

        public void Reject(string rejectedReason, DateTime rejectedOn)
        {
            EnsureValidStatus(DocumentStatus.UnderReview);

            if (string.IsNullOrWhiteSpace(rejectedReason))
                throw new ArgumentException("Rejection reason is required", nameof(rejectedReason));

            Status = DocumentStatus.Rejected;
            RejectedOn = rejectedOn;
            RejectedReason = rejectedReason;
            VerifiedOn = null;
        }

        public void ReopenForReview()
        {
            EnsureValidStatus(DocumentStatus.Rejected);

            Status = DocumentStatus.UnderReview;
            RejectedOn = null;
            RejectedReason = null;
        }

        public void Delete(DateTime deletedOn)
        {
            if (Status == DocumentStatus.Deleted)
                throw new InvalidKycDocumentStateException("Kyc document has already been deleted.");
            Status = DocumentStatus.Deleted;
        }

        public void EnsureValidStatus(DocumentStatus expectedStatus)
        {
            if (Status != expectedStatus)
                throw new InvalidKycDocumentStateException($"KYC document cannot transition from {Status} to the requested state.");
        }

        #endregion
    }
}
