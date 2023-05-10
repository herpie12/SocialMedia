using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class LikePostCommand : BaseCommand
    {
       public string LikedByUsername { get; set; }
    }
}
