namespace DigitalBanking.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        // Properties
        public DateTime? CreatedAtUtc { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? UpdatedAtUtc { get; protected set; }
        public string? UpdatedBy { get; protected set; }
        public DateTime? DeletedAtUtc { get; protected set; }
        public string? DeletedBy { get; protected set; }
        public bool IsDeleted { get; protected set; }

        // Behaviors
        public void SetCreatedBy(string createdByUser, DateTime utcNow)
        {
            CreatedBy = createdByUser;
            CreatedAtUtc = utcNow;
        }

        public void MarkAsDeleted(string deletedBy, DateTime utcNow)
        {
            DeletedBy = deletedBy;
            DeletedAtUtc = utcNow;
            IsDeleted = true;
        }

        public void MarkAsUpdated(string updatedBy, DateTime utcNow)
        {
            UpdatedBy = updatedBy;
            UpdatedAtUtc = utcNow;
        }
    }
}
