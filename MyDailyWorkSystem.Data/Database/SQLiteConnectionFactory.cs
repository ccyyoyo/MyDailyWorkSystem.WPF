using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace MyDailyWorkSystem.Data.Database
{
    //1.持有資料庫檔案路徑

    //2.檢查資料庫是否存在
    //　→ 若不存在則建立（SQLite 可以直接建立新檔案）

    //3.建立 SQLiteConnection 實例

    //4.回傳開啟好的連線給 Repository 使用

    public class SQLiteConnectionFactory
    {
        // 修正屬性語法錯誤，改用私有欄位搭配公開屬性
        private string _databasePath = "C:\\Users\\<你>\\AppData\\Local\\MyDailyWorkSystem\\database.db";
        public string DatabasePath
        {
            get => _databasePath;
            set => _databasePath = value;
        }

        public SQLiteConnectionFactory(string databasePath)
        {
            DatabasePath = databasePath;
        }

        public SqliteConnection GetConnection()
        {
            // 檢查資料庫檔案是否存在，若不存在則建立
            if (!System.IO.File.Exists(DatabasePath))
            {
                EnsureDirectoryExists(System.IO.Path.GetDirectoryName(DatabasePath));
            }

            var conn = new SqliteConnection($"Data Source={DatabasePath}");
            conn.Open();
            return conn;
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
        }
    }


}
