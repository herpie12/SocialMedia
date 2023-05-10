using CQRS.Core.Domain;
using Post.Commen.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _auther;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new Dictionary<Guid, Tuple<string, string>>();

        public bool Active { get => _active; set => _active = value; }

        public PostAggregate()
        {

        }
        public PostAggregate(Guid id, string auther, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = auther,
                Message = message,
                DatePosted = DateTime.Now,
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _auther = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post!");
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a vaild {nameof(message)}!");
            }

            RaiseEvent(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost(string likedByUsername)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like inactive post!");
            }

            RaiseEvent(new PostLikedEvent
            {
                Id = _id,
                LikedByUsername = likedByUsername,
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add comment to an inactive post!");
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a vaild {nameof(comment)}!");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now,
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit comment to an inactive post!");
            }
            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now,
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove comment to an inactive post!");
            }
            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user!");
            }

            RaiseEvent(new CommentRemoveEvent
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void Apply(CommentRemoveEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed!");
            }
            if (!_auther.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post that was made by someone else!");
            }
            RaiseEvent(new PostRemovedEvent
            {
                Id = _id,
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}
