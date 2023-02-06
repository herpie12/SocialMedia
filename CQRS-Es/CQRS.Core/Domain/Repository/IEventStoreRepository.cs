using CQRS.Core.Events;

namespace CQRS.Core.Domain.Repos
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel @event);
        Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
    }
}
