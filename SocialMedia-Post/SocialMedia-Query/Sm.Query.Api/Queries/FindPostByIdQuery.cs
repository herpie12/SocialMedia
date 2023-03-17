using CQRS.Core.Queries;

namespace Sm.Query.Api.Queries
{
    public class FindPostByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}
