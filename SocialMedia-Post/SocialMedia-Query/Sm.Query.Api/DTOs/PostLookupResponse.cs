using Post.Commen.DTOs;
using Sm.Query.Domain.Entities;

namespace Sm.Query.Api.DTOs
{
    public class PostLookupResponse : BaseReponse
    {
        public List<PostEntity> Posts { get; set; }
    }
}
