using System.Data;
using System.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL;

public class NhaCungCapDAL
{
    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT MaNCC, TenNCC, DienThoai, DiaChi FROM NhaCungCap ORDER BY MaNCC");
    }

    public DataTable Search(string keyword)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT MaNCC, TenNCC, DienThoai, DiaChi FROM NhaCungCap " +
            "WHERE TenNCC LIKE @KW OR DienThoai LIKE @KW ORDER BY MaNCC",
            new SqlParameter("@KW", "%" + keyword + "%"));
    }

    public int Insert(NhaCungCapDTO n)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO NhaCungCap(TenNCC, DienThoai, DiaChi) VALUES(@Ten, @DT, @DC)",
            new SqlParameter("@Ten", n.TenNCC),
            new SqlParameter("@DT", string.IsNullOrWhiteSpace(n.DienThoai) ? (object)DBNull.Value : n.DienThoai),
            new SqlParameter("@DC", string.IsNullOrWhiteSpace(n.DiaChi) ? (object)DBNull.Value : n.DiaChi));
    }

    public int Update(NhaCungCapDTO n)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "UPDATE NhaCungCap SET TenNCC=@Ten, DienThoai=@DT, DiaChi=@DC WHERE MaNCC=@Ma",
            new SqlParameter("@Ten", n.TenNCC),
            new SqlParameter("@DT", string.IsNullOrWhiteSpace(n.DienThoai) ? (object)DBNull.Value : n.DienThoai),
            new SqlParameter("@DC", string.IsNullOrWhiteSpace(n.DiaChi) ? (object)DBNull.Value : n.DiaChi),
            new SqlParameter("@Ma", n.MaNCC));
    }

    public int Delete(int ma)
    {
        return DatabaseHelper.ExecuteNonQuery("DELETE FROM NhaCungCap WHERE MaNCC=@Ma",
            new SqlParameter("@Ma", ma));
    }
}
