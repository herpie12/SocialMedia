using CQRS.Core.Domain.Repos;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Store
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;

        public EventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }
        public async Task<List<BaseEvent>> GetByIdAsync(Guid aggregateId)
        {
            var eventStrem = await _eventStoreRepository.FindByAggregateId(aggregateId);
            if (eventStrem == null || !eventStrem.Any())
            {
                throw new AggregateNotFoundException("Incorrect post id provided");
            }

            return eventStrem.OrderBy(x => x.Version).Select(x => x.EventDate).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStrem = await _eventStoreRepository.FindByAggregateId(aggregateId);

            if (expectedVersion != -1 || eventStrem[^1].Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    Timestamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventDate = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);
            }
        }
    }
}
