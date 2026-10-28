using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Bai4_1_Calculator
{
    /// <summary>Mot dong lich su tinh toan.</summary>
    public class HistoryItem
    {
        public long Id { get; set; }
        public string Expression { get; set; }   // vd: "12 + 5"
        public double Result { get; set; }       // vd: 17
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Lop truy cap CSDL SQLite (Data Access Layer).
    /// Tach rieng khoi giao dien de MainForm khong phai viet SQL.
    /// </summary>
    public class CalculatorDb
    {
        private readonly string _connectionString;

        public CalculatorDb(string dbPath = null)
        {
            // Mac dinh: file calculator.db nam canh file .exe
            dbPath ??= Path.Combine(AppContext.BaseDirectory, "calculator.db");
            _connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath
            }.ToString();

            Initialize();
        }

        // Tao bang neu chua ton tai
        private void Initialize()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS History (
                    Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                    Expression TEXT    NOT NULL,
                    Result     REAL    NOT NULL,
                    CreatedAt  TEXT    NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        /// <summary>Them 1 phep tinh vao lich su, tra ve Id vua tao.</summary>
        public long AddHistory(string expression, double result)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO History (Expression, Result, CreatedAt)
                VALUES ($expr, $result, $time);
                SELECT last_insert_rowid();";
            // Luon dung tham so ($name) de tranh SQL Injection
            cmd.Parameters.AddWithValue("$expr", expression);
            cmd.Parameters.AddWithValue("$result", result);
            cmd.Parameters.AddWithValue("$time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return (long)cmd.ExecuteScalar();
        }

        /// <summary>Lay N phep tinh gan nhat (moi nhat truoc).</summary>
        public List<HistoryItem> GetHistory(int limit = 100)
        {
            var list = new List<HistoryItem>();

            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Expression, Result, CreatedAt
                FROM History
                ORDER BY Id DESC
                LIMIT $limit;";
            cmd.Parameters.AddWithValue("$limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new HistoryItem
                {
                    Id = reader.GetInt64(0),
                    Expression = reader.GetString(1),
                    Result = reader.GetDouble(2),
                    CreatedAt = DateTime.Parse(reader.GetString(3))
                });
            }
            return list;
        }

        /// <summary>Xoa 1 dong lich su theo Id.</summary>
        public void DeleteHistory(long id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM History WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        /// <summary>Xoa toan bo lich su.</summary>
        public void ClearHistory()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM History;";
            cmd.ExecuteNonQuery();
        }
    }
}
