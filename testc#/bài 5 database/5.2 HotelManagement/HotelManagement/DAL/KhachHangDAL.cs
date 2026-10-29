using System.Data;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL;

public class KhachHangDAL
{
    public KhachHangDTO GetByCCCD(string cccd)
    {
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT MaKH, HoTen, CCCD, SoDienThoai FROM KhachHang WHERE CCCD=@CCCD",
            new SqlParameter("@CCCD", cccd));
        if (dt.Rows.Count == 0) return null;
        var r = dt.Rows[0];
        return new KhachHangDTO
        {
            MaKH = Convert.ToInt32(r["MaKH"]),
            HoTen = r["HoTen"].ToString(),
            CCCD = r["CCCD"].ToString(),
            SoDienThoai = r["SoDienThoai"] == DBNull.Value ? "" : r["SoDienThoai"].ToString()
        };
    }
}
