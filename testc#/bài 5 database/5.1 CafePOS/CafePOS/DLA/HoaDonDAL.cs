using System.Data;
using System.Data.SqlClient;

namespace CafePOS.DAL;

public class HoaDonDAL
{
    public DataTable GetChiTietByBan(int maBan)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT ct.MaMon, m.TenMon, ct.SoLuong, ct.DonGia, ct.SoLuong * ct.DonGia AS ThanhTien " +
            "FROM ChiTietHoaDon ct " +
            "JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon " +
            "JOIN MonAn m ON ct.MaMon = m.MaMon " +
            "WHERE h.MaBan = @MaBan AND h.TrangThai = 0 ORDER BY m.TenMon",
            new SqlParameter("@MaBan", maBan));
    }

    public void ThemMon(int maBan, int maMon, int soLuong)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            var o = DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT TOP 1 MaHoaDon FROM HoaDon WHERE MaBan=@MaBan AND TrangThai=0",
                new SqlParameter("@MaBan", maBan));
            int maHD;
            if (o == null)
            {
                maHD = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, trans,
                    "INSERT INTO HoaDon(MaBan, GioVao, TongTien, TrangThai) VALUES(@MaBan, GETDATE(), 0, 0); " +
                    "SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new SqlParameter("@MaBan", maBan)));
            }
            else
            {
                maHD = Convert.ToInt32(o);
            }

            var gia = DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT DonGia FROM MonAn WHERE MaMon=@MaMon",
                new SqlParameter("@MaMon", maMon));
            if (gia == null) throw new InvalidOperationException("Món ăn không tồn tại");
            decimal donGia = Convert.ToDecimal(gia);

            int rows = DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE ChiTietHoaDon SET SoLuong = SoLuong + @SL WHERE MaHoaDon=@HD AND MaMon=@Mon",
                new SqlParameter("@SL", soLuong),
                new SqlParameter("@HD", maHD),
                new SqlParameter("@Mon", maMon));
            if (rows == 0)
            {
                DatabaseHelper.ExecuteNonQuery(conn, trans,
                    "INSERT INTO ChiTietHoaDon(MaHoaDon, MaMon, SoLuong, DonGia) VALUES(@HD, @Mon, @SL, @Gia)",
                    new SqlParameter("@HD", maHD),
                    new SqlParameter("@Mon", maMon),
                    new SqlParameter("@SL", soLuong),
                    new SqlParameter("@Gia", donGia));
            }

            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE BanAn SET TrangThai=N'Có khách' WHERE MaBan=@MaBan",
                new SqlParameter("@MaBan", maBan));
            trans.Commit();
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }

    public decimal ThanhToan(int maBan)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            var o = DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT TOP 1 MaHoaDon FROM HoaDon WHERE MaBan=@MaBan AND TrangThai=0",
                new SqlParameter("@MaBan", maBan));
            if (o == null) throw new InvalidOperationException("Bàn này chưa có hóa đơn cần thanh toán");
            int maHD = Convert.ToInt32(o);

            decimal tong = Convert.ToDecimal(DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT ISNULL(SUM(SoLuong * DonGia), 0) FROM ChiTietHoaDon WHERE MaHoaDon=@HD",
                new SqlParameter("@HD", maHD)));

            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE HoaDon SET GioRa=GETDATE(), TongTien=@Tong, TrangThai=1 WHERE MaHoaDon=@HD",
                new SqlParameter("@Tong", tong),
                new SqlParameter("@HD", maHD));

            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE BanAn SET TrangThai=N'Trống' WHERE MaBan=@MaBan",
                new SqlParameter("@MaBan", maBan));
            trans.Commit();
            return tong;
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }

    public void DoiBan(int banCu, int banMoi)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            int trong = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT COUNT(*) FROM BanAn WHERE MaBan=@Ma AND TrangThai=N'Trống'",
                new SqlParameter("@Ma", banMoi)));
            if (trong == 0) throw new InvalidOperationException("Bàn muốn chuyển đến không còn trống");

            int rows = DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE HoaDon SET MaBan=@Moi WHERE MaBan=@Cu AND TrangThai=0",
                new SqlParameter("@Moi", banMoi),
                new SqlParameter("@Cu", banCu));
            if (rows == 0) throw new InvalidOperationException("Bàn hiện tại chưa có hóa đơn để chuyển");

            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE BanAn SET TrangThai=N'Có khách' WHERE MaBan=@Ma",
                new SqlParameter("@Ma", banMoi));
            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE BanAn SET TrangThai=N'Trống' WHERE MaBan=@Ma",
                new SqlParameter("@Ma", banCu));
            trans.Commit();
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }
}
