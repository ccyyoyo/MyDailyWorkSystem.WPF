using Dapper;

namespace MyDailyWorkSystem.Data.Database
{
    public class DatabaseInitializer
    {
        private readonly IDataConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDataConnectionFactory factory)
        {
            _connectionFactory = factory;
        }

        public void Initialize()
        {
            using var conn = _connectionFactory.GetConnection();

            conn.Execute("""
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id TEXT PRIMARY KEY,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NOT NULL,
                    Title TEXT NOT NULL,
                    Notes TEXT,
                    Type INTEGER NOT NULL,
                    Priority INTEGER NOT NULL,
                    State INTEGER NOT NULL,
                    DueDate TEXT,
                    EstimatedHours INTEGER,
                    ActualFinishTime TEXT,
                    ProjectId TEXT,
                    CreatorUserId TEXT,
                    AssignedToUserId TEXT,
                    TagIds TEXT,
                    AttachmentIds TEXT,
                    OrderIndex INTEGER
                );

                CREATE TABLE IF NOT EXISTS Projects (
                    Id TEXT PRIMARY KEY,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NOT NULL,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Type INTEGER NOT NULL,
                    Status INTEGER NOT NULL,
                    ExpectedStartDate TEXT,
                    ExpectedEndDate TEXT,
                    ActualEndDate TEXT,
                    MainStakeholder TEXT,
                    RiskNotes TEXT,
                    TagIds TEXT,
                    LastRecordDate TEXT,
                    OrderIndex INTEGER
                );

                CREATE TABLE IF NOT EXISTS Notes (
                    Id TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Content TEXT,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NOT NULL,
                    RelatedProjectIds TEXT,
                    RelatedTaskIds TEXT,
                    RelatedPeople TEXT,
                    TagIds TEXT,
                    AttachmentIds TEXT,
                    OrderIndex INTEGER
                );
            """);
        }
    }
}
