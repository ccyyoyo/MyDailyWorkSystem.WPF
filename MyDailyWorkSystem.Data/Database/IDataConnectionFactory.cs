using Microsoft.Data.Sqlite;

namespace MyDailyWorkSystem.Data.Database
{
    public interface IDataConnectionFactory
    {
        SqliteConnection GetConnection();
    }
}
