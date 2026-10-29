using System.Data;
using System.Data.SqlClient;

namespace HotelManagement.DAL;

public class PhongDAL
{
    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT SoPhong, LoaiPhong, GiaTheoGio, TrangThai FROM Phong ORDER BY SoPhong");
    }

    public DataTable GetByTrangThai(string trangThai)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT SoPhong, LoaiPhong, GiaTheoGio FROM Phong WHERE TrangThai=@TT ORDER BY SoPhong",
            new SqlParameter("@TT", trangThai));
    }

    public decimal GetGia(string soPhong)
    {
        var o = DatabaseHelper.ExecuteScalar("SELECT GiaTheoGio FROM Phong WHERE SoPhong=@P",
            new SqlParameter("@P", soPhong));
        if (o == null) throw new InvalidOperationException("Phòng không tồn tại");
        return Convert.ToDecimal(o);
    }

    public int UpdateTrangThai(string soPhong, string trangThai)
    {
        return DatabaseHelper.ExecuteNonQuery("UPDATE Phong SET TrangThai=@TT WHERE SoPhong=@P",
            new SqlParameter("@TT", trangThai),
            new SqlParameter("@P", soPhong));
    }
}
