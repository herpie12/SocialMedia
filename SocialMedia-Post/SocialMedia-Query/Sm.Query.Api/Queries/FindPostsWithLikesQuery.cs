using CQRS.Core.Queries;

namespace Sm.Query.Api.Queries
{
    public class FindPostsWithLikesQuery : BaseQuery
    {
        public int NumberOfLikes { get; set; }
    }
}
