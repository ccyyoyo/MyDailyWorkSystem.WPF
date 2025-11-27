using Microsoft.Data.Sqlite;
using MyDailyWorkSystem.Data.Database;

namespace MyDailyWorkSystem.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IDataConnectionFactory _factory;

        protected BaseRepository(IDataConnectionFactory factory)
        {
            _factory = factory;
        }

        protected SqliteConnection GetConnection()
        {
            return _factory.GetConnection();
        }
    }
}
