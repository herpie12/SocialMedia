using Microsoft.EntityFrameworkCore;

namespace Sm.Query.Infrastructure.DataContext
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContexrt;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContexrt)
        {
            _configureDbContexrt = configureDbContexrt;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
            _configureDbContexrt(optionsBuilder);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
