using Sm.Query.Domain.Entities;

namespace Sm.Query.Api.Queries
{
    public interface IQueryHandler
    {
        Task<List<PostEntity>> HandlerAsync(FindAllPostsQuery query);

        Task<List<PostEntity>> HandlerAsync(FindPostByAuthorQuery query);

        Task<List<PostEntity>> HandlerAsync(FindPostByIdQuery query);

        Task<List<PostEntity>> HandlerAsync(FindPostsWithCommentsQuery query);

        Task<List<PostEntity>> HandlerAsync(FindPostsWithLikesQuery query);
    }
}
