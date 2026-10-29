using System.Data;
using System.Data.SqlClient;
using HotelManagement.DTO;

namespace HotelManagement.DAL;

public class PhieuDAL
{
    public int CheckIn(string soPhong, KhachHangDTO kh)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            int trong = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT COUNT(*) FROM Phong WHERE SoPhong=@P AND TrangThai=N'Trống'",
                new SqlParameter("@P", soPhong)));
            if (trong == 0) throw new InvalidOperationException("Phòng không còn trống");

            int maKH;
            var existing = DatabaseHelper.ExecuteScalar(conn, trans,
                "SELECT MaKH FROM KhachHang WHERE CCCD=@CCCD",
                new SqlParameter("@CCCD", kh.CCCD));
            if (existing != null)
            {
                maKH = Convert.ToInt32(existing);
            }
            else
            {
                maKH = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, trans,
                    "INSERT INTO KhachHang(HoTen, CCCD, SoDienThoai) VALUES(@HoTen, @CCCD, @SDT); " +
                    "SELECT CAST(SCOPE_IDENTITY() AS INT)",
                    new SqlParameter("@HoTen", kh.HoTen),
                    new SqlParameter("@CCCD", kh.CCCD),
                    new SqlParameter("@SDT", string.IsNullOrWhiteSpace(kh.SoDienThoai) ? (object)DBNull.Value : kh.SoDienThoai)));
            }

            int maPhieu = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, trans,
                "INSERT INTO PhieuDatPhong(SoPhong, MaKH, NgayNhanPhong) VALUES(@P, @KH, GETDATE()); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT)",
                new SqlParameter("@P", soPhong),
                new SqlParameter("@KH", maKH)));

            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE Phong SET TrangThai=N'Đang ở' WHERE SoPhong=@P",
                new SqlParameter("@P", soPhong));
            trans.Commit();
            return maPhieu;
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }

    public PhieuDatPhongDTO GetPhieuDangO(string soPhong)
    {
        var dt = DatabaseHelper.ExecuteQuery(
            "SELECT TOP 1 MaPhieu, SoPhong, MaKH, NgayNhanPhong FROM PhieuDatPhong " +
            "WHERE SoPhong=@P AND NgayTraPhong IS NULL ORDER BY NgayNhanPhong DESC",
            new SqlParameter("@P", soPhong));
        if (dt.Rows.Count == 0) return null;
        var r = dt.Rows[0];
        return new PhieuDatPhongDTO
        {
            MaPhieu = Convert.ToInt32(r["MaPhieu"]),
            SoPhong = r["SoPhong"].ToString(),
            MaKH = Convert.ToInt32(r["MaKH"]),
            NgayNhanPhong = Convert.ToDateTime(r["NgayNhanPhong"])
        };
    }

    public string GetTenKhach(int maKH)
    {
        var o = DatabaseHelper.ExecuteScalar("SELECT HoTen FROM KhachHang WHERE MaKH=@Ma",
            new SqlParameter("@Ma", maKH));
        return o == null ? "" : o.ToString();
    }

    public int AddService(SuDungDichVuDTO s)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO SuDungDichVu(MaPhieu, MaDV, SoLuong, ThoiGian) VALUES(@Phieu, @DV, @SL, GETDATE())",
            new SqlParameter("@Phieu", s.MaPhieu),
            new SqlParameter("@DV", s.MaDV),
            new SqlParameter("@SL", s.SoLuong));
    }

    public DataTable GetServicesByPhieu(int maPhieu)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT dv.TenDV, sd.SoLuong, dv.DonGia, sd.SoLuong * dv.DonGia AS ThanhTien, sd.ThoiGian " +
            "FROM SuDungDichVu sd JOIN DichVu dv ON sd.MaDV = dv.MaDV " +
            "WHERE sd.MaPhieu=@Phieu ORDER BY sd.ThoiGian",
            new SqlParameter("@Phieu", maPhieu));
    }

    public decimal GetTongTienDichVu(int maPhieu)
    {
        return Convert.ToDecimal(DatabaseHelper.ExecuteScalar(
            "SELECT ISNULL(SUM(sd.SoLuong * dv.DonGia), 0) FROM SuDungDichVu sd " +
            "JOIN DichVu dv ON sd.MaDV = dv.MaDV WHERE sd.MaPhieu=@Phieu",
            new SqlParameter("@Phieu", maPhieu)));
    }

    public void CheckOut(int maPhieu, string soPhong, DateTime ngayTra, decimal tong)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            int rows = DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE PhieuDatPhong SET NgayTraPhong=@Tra, TongTienThanhToan=@Tong " +
                "WHERE MaPhieu=@Phieu AND NgayTraPhong IS NULL",
                new SqlParameter("@Tra", ngayTra),
                new SqlParameter("@Tong", tong),
                new SqlParameter("@Phieu", maPhieu));
            if (rows == 0) throw new InvalidOperationException("Phiếu đã được thanh toán hoặc không tồn tại");

            DatabaseHelper.ExecuteNonQuery(conn, trans,
                "UPDATE Phong SET TrangThai=N'Đang dọn dẹp' WHERE SoPhong=@P",
                new SqlParameter("@P", soPhong));
            trans.Commit();
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }
}
