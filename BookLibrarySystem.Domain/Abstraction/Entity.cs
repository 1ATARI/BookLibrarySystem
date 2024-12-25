namespace BookLibrarySystem.Domain.Abstraction;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
        
    }
    public Guid Id { get; init; }
    /// <summary>
    ///    retrieves the list of domain events raised by this entity
    /// </summary>
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    /// <summary>
    /// This is a helper method that entities can use to raise domain events. It adds the event to the _domainEvents list.
    /// When an entity undergoes a state change, it calls this method to record the event.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}