using Microsoft.EntityFrameworkCore;
using Sm.Query.Domain.Entities;

namespace Sm.Query.Infrastructure.DataContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
    }
}
