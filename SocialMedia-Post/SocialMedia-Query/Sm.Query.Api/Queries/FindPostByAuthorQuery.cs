using CQRS.Core.Queries;

namespace Sm.Query.Api.Queries
{
    public class FindPostByAuthorQuery : BaseQuery
    {
        public string Author { get; set; }
    }
}
