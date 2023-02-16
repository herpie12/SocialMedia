using Sm.Query.Domain.Entities;

namespace Sm.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(PostEntity post);

        Task<List<PostEntity>> ListAllAsync();
        Task<PostEntity> GetByIdAsync(Guid postId);
        Task<List<PostEntity>> ListByAuthorAsync(Guid postId);
        Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes);
        Task<List<PostEntity>> ListWithCommentsAsync();
    }
}
