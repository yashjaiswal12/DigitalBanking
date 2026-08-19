namespace DigitalBanking.Domain.Common
{
    // Every entity will inherit from this
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; } = Guid.NewGuid();
        private List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
