using System.Data;
using System.Data.SqlClient;
using CafePOS.DTO;

namespace CafePOS.DAL;

public class DanhMucDAL
{
    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery("SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc ORDER BY MaDanhMuc");
    }

    public int Insert(DanhMucDTO dm)
    {
        return DatabaseHelper.ExecuteNonQuery("INSERT INTO DanhMuc(TenDanhMuc) VALUES(@Ten)",
            new SqlParameter("@Ten", dm.TenDanhMuc));
    }

    public int Update(DanhMucDTO dm)
    {
        return DatabaseHelper.ExecuteNonQuery("UPDATE DanhMuc SET TenDanhMuc=@Ten WHERE MaDanhMuc=@Ma",
            new SqlParameter("@Ten", dm.TenDanhMuc),
            new SqlParameter("@Ma", dm.MaDanhMuc));
    }

    public int Delete(int ma)
    {
        return DatabaseHelper.ExecuteNonQuery("DELETE FROM DanhMuc WHERE MaDanhMuc=@Ma",
            new SqlParameter("@Ma", ma));
    }
}
