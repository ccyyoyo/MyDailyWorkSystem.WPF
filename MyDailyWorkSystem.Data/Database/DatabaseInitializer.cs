/* 
計畫 (詳細偽程式說明)：
1. 修正類別建構子為 public 並儲存傳入的 SQLiteConnectionFactory 實例。
2. 在 Initialize() 中取得 DB 連線、開啟連線，並呼叫建立表格的方法。
3. 實作 CreateTablesIfNotExist(IDbConnection connection)：
   - 使用 StringBuilder 組出多個 CREATE TABLE IF NOT EXISTS SQL 陳述式（Tasks、Projects、Notes）。
   - 每個表含基本欄位（Id、Title/Name、Description、CreatedAt、UpdatedAt…），採用 SQLite 合法的資料型別與預設值。
   - 使用 Dapper 的 Execute(sql) 執行整段 SQL（一次執行多個 CREATE TABLE 陳述式）。
4. 將欄位設計為可擴充（預留 ProjectId、TaskId 外鍵欄位），若未來需要 Migration 可手動處理。
5. 處理傳入參數為 null 的防禦式程式（ArgumentNullException）。
*/

using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Text;

namespace MyDailyWorkSystem.Data.Database
{
    // 第一次啟動時自動建表（Tasks, Projects, Notes…）
    // 如果資料庫已存在 → 不動它
    // 如果你之後加欄位 → 可以手動 Migration

    public class DatabaseInitializer
    {
        private readonly SQLiteConnectionFactory _connectionFactory;

        public DatabaseInitializer(SQLiteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public void Initialize()
        {
            using SqliteConnection connection = _connectionFactory.GetConnection();
            // 若 GetConnection() 回傳 IDbConnection 且需要 Open，則開啟連線
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            CreateTaskTable(connection);
            CreateProjectTable(connection);
            CreateNoteTable(connection);
        }

        private void CreateTaskTable(SqliteConnection connection)
        {
            var sql = new StringBuilder();
            connection.Execute(
                """
                CREATE TABLE IF NOT EXISTS Tasks (
                Id TEXT PRIMARY KEY,
                CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                UpdatedAt TEXT NOT NULL DEFAULT (datetime('now')),
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
                """
                );
        }

        private void CreateProjectTable(SqliteConnection connection)
        {
            connection.Execute(
                """
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
                """);
        }

        private void CreateNoteTable(SqliteConnection connection)
        {
            connection.Execute(
                """
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
