namespace DigitalBanking.Domain.Common
{
    // Every entity will inherit from this
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
