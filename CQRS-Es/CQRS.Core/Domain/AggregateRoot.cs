using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id { get; set; }

        private readonly List<BaseEvent> _changes = new();

        public Guid Id { get { return _id; } }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        private void ApplyChanges(BaseEvent @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}");
            }

            method.Invoke(this, new object[] { @event });

            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChanges(@event, true);
        }

        public void ReplyEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChanges(@event, false);
            }
        }
    }
}
