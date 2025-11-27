using Microsoft.Data.Sqlite;

namespace MyDailyWorkSystem.Data.Database
{
    public class SQLiteConnectionFactory : IDataConnectionFactory
    {
        private static readonly string AppFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) ,
                         "MyDailyWorkSystem");

        private static readonly string DefaultDatabasePath =
            Path.Combine(AppFolder , "database.db");

        public string DatabasePath { get; }

        public SQLiteConnectionFactory() : this(DefaultDatabasePath) { }

        public SQLiteConnectionFactory(string databasePath)
        {
            DatabasePath = databasePath;
            EnsureDirectoryExists(Path.GetDirectoryName(DatabasePath));
        }

        public SqliteConnection GetConnection()
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = DatabasePath ,
                Mode = SqliteOpenMode.ReadWriteCreate ,
                Cache = SqliteCacheMode.Shared
            };

            var conn = new SqliteConnection(builder.ConnectionString);
            conn.Open();

            // ¥[³tÅª¼g & Á×§K locked
            EnableWAL(conn);

            return conn;
        }

        private void EnableWAL(SqliteConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA journal_mode=WAL;";
            cmd.ExecuteNonQuery();
        }

        private void EnsureDirectoryExists(string? path)
        {
            if ( !string.IsNullOrWhiteSpace(path) && !Directory.Exists(path) )
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
