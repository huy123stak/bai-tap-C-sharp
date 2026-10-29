using System.Data;
using System.Data.SqlClient;
using CafePOS.DTO;

namespace CafePOS.DAL;

public class MonAnDAL
{
    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT m.MaMon, m.TenMon, m.MaDanhMuc, d.TenDanhMuc, m.DonGia " +
            "FROM MonAn m JOIN DanhMuc d ON m.MaDanhMuc = d.MaDanhMuc ORDER BY m.MaMon");
    }

    public DataTable GetByDanhMuc(int maDanhMuc)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT MaMon, TenMon, MaDanhMuc, DonGia FROM MonAn WHERE MaDanhMuc=@Ma ORDER BY TenMon",
            new SqlParameter("@Ma", maDanhMuc));
    }

    public int Insert(MonAnDTO m)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO MonAn(TenMon, MaDanhMuc, DonGia) VALUES(@Ten, @DM, @Gia)",
            new SqlParameter("@Ten", m.TenMon),
            new SqlParameter("@DM", m.MaDanhMuc),
            new SqlParameter("@Gia", m.DonGia));
    }

    public int Update(MonAnDTO m)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "UPDATE MonAn SET TenMon=@Ten, MaDanhMuc=@DM, DonGia=@Gia WHERE MaMon=@Ma",
            new SqlParameter("@Ten", m.TenMon),
            new SqlParameter("@DM", m.MaDanhMuc),
            new SqlParameter("@Gia", m.DonGia),
            new SqlParameter("@Ma", m.MaMon));
    }

    public int Delete(int maMon)
    {
        return DatabaseHelper.ExecuteNonQuery("DELETE FROM MonAn WHERE MaMon=@Ma",
            new SqlParameter("@Ma", maMon));
    }
}
