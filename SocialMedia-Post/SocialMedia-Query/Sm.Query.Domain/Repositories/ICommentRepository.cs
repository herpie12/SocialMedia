using Sm.Query.Domain.Entities;

namespace Sm.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity commentEntity);
        Task UpdateAsync(CommentEntity commentEntity);
        Task<CommentEntity> GetByIdAsync(Guid commentId);
        Task DeleteAsync(Guid commentId);
    }
}
