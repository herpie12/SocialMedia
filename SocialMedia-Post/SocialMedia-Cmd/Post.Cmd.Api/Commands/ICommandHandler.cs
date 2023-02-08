namespace Post.Cmd.Api.Commands
{
    public interface IcommandHandler
    {
        Task HandleAsync(NewPostCommand command);
        Task HandleAsync(RemoveCommentCommand command);
        Task HandleAsync(LikePostCommand command);
        Task HandleAsync(EditCommentCommand command);
        Task HandleAsync(EditMessageCommand command);
        Task HandleAsync(DeletePostComment command);
        Task HandleAsync(AddCommentCommand command);
    }
}
