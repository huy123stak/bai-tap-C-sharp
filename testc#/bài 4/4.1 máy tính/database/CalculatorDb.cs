using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

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
    /// Lop truy cap CSDL MySQL (XAMPP) bang ADO.NET.
    /// Quy trinh chuan: MySqlConnection -> MySqlCommand -> (MySqlDataReader) -> dong ket noi.
    /// </summary>
    public class CalculatorDb
    {
        // Cau hinh mac dinh cua XAMPP: user root, KHONG co mat khau, cong 3306
        private const string Server = "localhost";
        private const uint Port = 3306;
        private const string User = "root";
        private const string Password = "";
        private const string DatabaseName = "calculator_db";

        private readonly string _connectionString;

        public CalculatorDb()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = Port,
                UserID = User,
                Password = Password,
                Database = DatabaseName,
                CharacterSet = "utf8mb4"
            };
            _connectionString = builder.ConnectionString;

            Initialize();
        }

        // Tao database + bang neu chua co (de chay lan dau khong can import SQL thu cong)
        private void Initialize()
        {
            // Buoc 1: ket noi KHONG chi dinh database de tao database
            var serverBuilder = new MySqlConnectionStringBuilder(_connectionString)
            {
                Database = ""
            };
            using (var conn = new MySqlConnection(serverBuilder.ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    $"CREATE DATABASE IF NOT EXISTS `{DatabaseName}` " +
                    "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Buoc 2: ket noi vao database vua tao de tao bang
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS History (
                        Id         BIGINT AUTO_INCREMENT PRIMARY KEY,
                        Expression VARCHAR(255) NOT NULL,
                        Result     DOUBLE       NOT NULL,
                        CreatedAt  DATETIME     NOT NULL
                    );", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Them 1 phep tinh vao lich su, tra ve Id vua tao.</summary>
        public long AddHistory(string expression, double result)
        {
            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(
                "INSERT INTO History (Expression, Result, CreatedAt) " +
                "VALUES (@expr, @result, @time);", conn))
            {
                // Luon dung tham so (@name) de tranh SQL Injection
                cmd.Parameters.AddWithValue("@expr", expression);
                cmd.Parameters.AddWithValue("@result", result);
                cmd.Parameters.AddWithValue("@time", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
                return cmd.LastInsertedId;
            }
        }

        /// <summary>Lay N phep tinh gan nhat (moi nhat truoc).</summary>
        public List<HistoryItem> GetHistory(int limit = 100)
        {
            var list = new List<HistoryItem>();

            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(
                "SELECT Id, Expression, Result, CreatedAt " +
                "FROM History ORDER BY Id DESC LIMIT @limit;", conn))
            {
                cmd.Parameters.AddWithValue("@limit", limit);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new HistoryItem
                        {
                            Id = reader.GetInt64("Id"),
                            Expression = reader.GetString("Expression"),
                            Result = reader.GetDouble("Result"),
                            CreatedAt = reader.GetDateTime("CreatedAt")
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>Xoa 1 dong lich su theo Id.</summary>
        public void DeleteHistory(long id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("DELETE FROM History WHERE Id = @id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Xoa toan bo lich su.</summary>
        public void ClearHistory()
        {
            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("DELETE FROM History;", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
