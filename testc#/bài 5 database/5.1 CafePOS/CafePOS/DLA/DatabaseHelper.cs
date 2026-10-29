using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CafePOS.DAL;

public static class DatabaseHelper
{
    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConfigurationManager.ConnectionStrings["CafePOSDB"].ConnectionString);
    }

    public static DataTable ExecuteQuery(string sql, params SqlParameter[] ps)
    {
        using var conn = GetConnection();
        using var adapter = new SqlDataAdapter(sql, conn);
        adapter.SelectCommand.Parameters.AddRange(ps);
        var dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    public static int ExecuteNonQuery(string sql, params SqlParameter[] ps)
    {
        using var conn = GetConnection();
        conn.Open();
        return ExecuteNonQuery(conn, null, sql, ps);
    }

    public static object ExecuteScalar(string sql, params SqlParameter[] ps)
    {
        using var conn = GetConnection();
        conn.Open();
        return ExecuteScalar(conn, null, sql, ps);
    }

    public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction trans, string sql, params SqlParameter[] ps)
    {
        using var cmd = new SqlCommand(sql, conn, trans);
        cmd.Parameters.AddRange(ps);
        return cmd.ExecuteNonQuery();
    }

    public static object ExecuteScalar(SqlConnection conn, SqlTransaction trans, string sql, params SqlParameter[] ps)
    {
        using var cmd = new SqlCommand(sql, conn, trans);
        cmd.Parameters.AddRange(ps);
        return cmd.ExecuteScalar();
    }
}
