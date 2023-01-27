using CQRS.Core.Events;

namespace Post.Commen.Events
{
    public class PostLikedEvent : BaseEvent
    {
        public PostLikedEvent() : base(nameof(PostLikedEvent))
        { 
        }
    }
}
