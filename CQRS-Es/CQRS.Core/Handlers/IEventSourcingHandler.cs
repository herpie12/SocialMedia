using CQRS.Core.Domain;

namespace CQRS.Core.Handlers
{
    public interface IEventSourcingHandler<T>
    {
        Task SaveAsync(AggregateRoot aggregateRoot);

        Task<T> GetByIdAsync(Guid aggregateId);
    }
}
