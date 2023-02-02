using CQRS.Core.Events;

namespace Post.Commen.Events
{
    public class CommentRemoveEvent : BaseEvent
    {
        public CommentRemoveEvent() : base(nameof(CommentRemoveEvent))
        {
        }
        public Guid CommentId { get; set; }
    }
}
