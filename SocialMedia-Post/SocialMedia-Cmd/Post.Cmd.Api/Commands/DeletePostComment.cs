using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class DeletePostComment: BaseCommand
    {
        public string Username { get; set; }
    }
}
