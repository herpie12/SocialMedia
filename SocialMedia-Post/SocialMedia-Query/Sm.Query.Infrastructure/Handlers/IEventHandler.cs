using Post.Commen.Events;

namespace Sm.Query.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(PostCreatedEvent postCreatedEvent);
        Task On(MessageUpdatedEvent messageUpdatedEvent);
        Task On(PostLikedEvent postLikedEvent);
        Task On(CommentAddedEvent commentAddedEvent);
        Task On(CommentUpdatedEvent commentUpdatedEvent);
        Task On(CommentRemoveEvent commentRemoveEvent);
        Task On(PostRemovedEvent postRemovedEvent);

    }
}
