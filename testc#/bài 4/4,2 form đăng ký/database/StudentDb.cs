using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Bai4_2_StudentRegister
{
    /// <summary>
    /// Lop truy cap CSDL MySQL (XAMPP) bang ADO.NET cho bang Students.
    /// </summary>
    public class StudentDb
    {
        // Cau hinh mac dinh cua XAMPP: user root, KHONG co mat khau, cong 3306
        private const string Server = "localhost";
        private const uint Port = 3306;
        private const string User = "root";
        private const string Password = "";
        private const string DatabaseName = "student_db";

        private readonly string _connectionString;

        public StudentDb()
        {
            _connectionString = new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = Port,
                UserID = User,
                Password = Password,
                Database = DatabaseName,
                CharacterSet = "utf8mb4"
            }.ConnectionString;

            Initialize();
        }

        // Tao database + bang neu chua co
        private void Initialize()
        {
            var serverBuilder = new MySqlConnectionStringBuilder(_connectionString) { Database = "" };
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

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Students (
                        Id          INT AUTO_INCREMENT PRIMARY KEY,
                        HoTen       VARCHAR(100) NOT NULL,
                        Email       VARCHAR(150) NOT NULL UNIQUE,
                        Tuoi        INT          NOT NULL,
                        SoDienThoai VARCHAR(15)  NOT NULL,
                        CreatedAt   DATETIME     NOT NULL
                    );", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>Them hoc vien moi, tra ve Id vua tao.</summary>
        public long AddStudent(string hoTen, string email, int tuoi, string soDienThoai)
        {
            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(
                "INSERT INTO Students (HoTen, Email, Tuoi, SoDienThoai, CreatedAt) " +
                "VALUES (@hoTen, @email, @tuoi, @sdt, @time);", conn))
            {
                cmd.Parameters.AddWithValue("@hoTen", hoTen);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@tuoi", tuoi);
                cmd.Parameters.AddWithValue("@sdt", soDienThoai);
                cmd.Parameters.AddWithValue("@time", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
                return cmd.LastInsertedId;
            }
        }

        /// <summary>Kiem tra email da duoc dang ky chua (email la duy nhat).</summary>
        public bool EmailExists(string email)
        {
            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM Students WHERE Email = @email;", conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        /// <summary>Lay toan bo hoc vien (DataTable de gan thang vao DataGridView).</summary>
        public DataTable GetAll()
        {
            var table = new DataTable();
            using (var conn = new MySqlConnection(_connectionString))
            using (var adapter = new MySqlDataAdapter(
                "SELECT Id, HoTen, Email, Tuoi, SoDienThoai, CreatedAt " +
                "FROM Students ORDER BY Id DESC;", conn))
            {
                adapter.Fill(table); // DataAdapter tu mo/dong ket noi
            }
            return table;
        }

        /// <summary>Xoa hoc vien theo Id.</summary>
        public void DeleteStudent(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            using (var cmd = new MySqlCommand("DELETE FROM Students WHERE Id = @id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
