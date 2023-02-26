using Microsoft.EntityFrameworkCore;
using Sm.Query.Domain.Entities;
using Sm.Query.Domain.Repositories;
using Sm.Query.Infrastructure.DataContext;

namespace Sm.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _databaseContextFactory;

        public CommentRepository(DatabaseContextFactory databaseContextFactory)
        {
            _databaseContextFactory = databaseContextFactory;
        }
        public async Task CreateAsync(CommentEntity commentEntity)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Add(commentEntity);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);

            if (comment == null) return;

            context.Comments.Remove(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            return await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity commentEntity)
        {
            using DatabaseContext context = _databaseContextFactory.CreateDbContext();
            context.Comments.Update(commentEntity);

            _ = await context.SaveChangesAsync();
        }
    }
}
