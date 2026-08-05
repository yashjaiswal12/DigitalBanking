namespace DigitalBanking.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        // Properties
        public DateTime CreatedOn { get; protected set; } = DateTime.Now;
        public string? CreatedBy { get; protected set; }
        public DateTime? UpdatedOn { get; protected set; }
        public string? UpdatedBy { get; protected set; }

        // Behaviors
        protected void SetCreatedBy(string createdByUser)
        {
            CreatedBy = createdByUser;
        }

        protected void SetUpdatedBy(string updatedByUser, DateTime updatedOn) 
        { 
            UpdatedBy = updatedByUser; 
            UpdatedOn = updatedOn;
        }
    }
}
