using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : IcommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventStoreHandler;

        public CommandHandler(IEventSourcingHandler<PostAggregate> eventStoreHandler)
        {
            _eventStoreHandler = eventStoreHandler;
        }
        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);

            await _eventStoreHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventStoreHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.Id, command.Username);

            await _eventStoreHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await _eventStoreHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();

            await _eventStoreHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await _eventStoreHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.Id, command.Comment, command.Username);

            await _eventStoreHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await _eventStoreHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);

            await _eventStoreHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostComment command)
        {
            var aggregate = await _eventStoreHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.Username);

            await _eventStoreHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await _eventStoreHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.Username);

            await _eventStoreHandler.SaveAsync(aggregate);
        }
    }
}
