using Post.Commen.Events;
using Sm.Query.Domain.Entities;
using Sm.Query.Domain.Repositories;

namespace Sm.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task On(PostCreatedEvent postCreatedEvent)
        {
            var post = new PostEntity
            {
                PostId = postCreatedEvent.Id,
                Author = postCreatedEvent.Author,
                DatePosted = postCreatedEvent.DatePosted,
                Message = postCreatedEvent.Message,
            };

            await _postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent messageUpdatedEvent)
        {
            var post = await _postRepository.GetByIdAsync(messageUpdatedEvent.Id);

            if (post == null) return;

            post.Message = messageUpdatedEvent.Message;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(PostLikedEvent postLikedEvent)
        {
            var post = await _postRepository.GetByIdAsync(postLikedEvent.Id);

            if (post == null) return;

            post.Likes++;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(CommentAddedEvent commentAddedEvent)
        {
            var comment = new CommentEntity
            {
                PostId = commentAddedEvent.Id,
                CommentId = commentAddedEvent.CommentId,
                CommentDate = commentAddedEvent.CommentDate,
                Comment = commentAddedEvent.Comment,
                UserName = commentAddedEvent.Username,
                Edited = false
            };

            await _commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentUpdatedEvent commentUpdatedEvent)
        {
            var comment = await _commentRepository.GetByIdAsync(commentUpdatedEvent.CommentId);

            if (comment == null) return;

            comment.Comment = commentUpdatedEvent.Comment;
            comment.Edited = true;
            comment.CommentDate = commentUpdatedEvent.EditDate;

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentRemoveEvent commentRemoveEvent)
        {
            await _commentRepository.DeleteAsync(commentRemoveEvent.CommentId);
        }

        public async Task On(PostRemovedEvent postRemovedEvent)
        {
            await _postRepository.DeleteAsync(postRemovedEvent.Id);
        }
    }
}
