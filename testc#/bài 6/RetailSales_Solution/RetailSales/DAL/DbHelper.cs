using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.DAL
{
    public static class DbHelper
    {
        public static string ConnStr
        {
            get { return ConfigurationManager.ConnectionStrings["SalesDB"].ConnectionString; }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnStr);
        }

        public static SqlParameter P(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        public static Exception Translate(SqlException ex)
        {
            switch (ex.Number)
            {
                case 547:
                    return new Exception("Thao tác vi phạm ràng buộc dữ liệu (khóa ngoại hoặc điều kiện CHECK).");
                case 2627:
                case 2601:
                    return new Exception("Dữ liệu bị trùng (mã hoặc số điện thoại đã tồn tại).");
                default:
                    return new Exception(ex.Message);
            }
        }

        public static DataTable GetTable(string sql, params SqlParameter[] ps)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddRange(ps);
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException ex)
            {
                throw Translate(ex);
            }
        }

        public static int Execute(string sql, params SqlParameter[] ps)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(ps);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw Translate(ex);
            }
        }

        public static object Scalar(string sql, params SqlParameter[] ps)
        {
            try
            {
                using (var conn = GetConnection())
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(ps);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw Translate(ex);
            }
        }
    }
}
