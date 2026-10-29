using System.Data;
using System.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL;

public class PhieuKhoDAL
{
    public int LapPhieu(PhieuKhoDTO phieu, List<ChiTietPhieuKhoDTO> chiTiet)
    {
        using var conn = DatabaseHelper.GetConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            int maPhieu = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, trans,
                "INSERT INTO PhieuKho(LoaiPhieu, NgayLap, MaNCC, TongTien) VALUES(@Loai, GETDATE(), @NCC, @Tong); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT)",
                new SqlParameter("@Loai", phieu.LoaiPhieu),
                new SqlParameter("@NCC", phieu.MaNCC.HasValue ? (object)phieu.MaNCC.Value : DBNull.Value),
                new SqlParameter("@Tong", phieu.TongTien)));

            foreach (var ct in chiTiet)
            {
                DatabaseHelper.ExecuteNonQuery(conn, trans,
                    "INSERT INTO ChiTietPhieuKho(MaPhieu, MaVatTu, SoLuong, DonGia) VALUES(@Phieu, @Vt, @SL, @Gia)",
                    new SqlParameter("@Phieu", maPhieu),
                    new SqlParameter("@Vt", ct.MaVatTu),
                    new SqlParameter("@SL", ct.SoLuong),
                    new SqlParameter("@Gia", ct.DonGia));

                if (phieu.LoaiPhieu == "NHAP")
                {
                    int rows = DatabaseHelper.ExecuteNonQuery(conn, trans,
                        "UPDATE VatTu SET SoLuongTon = SoLuongTon + @SL WHERE MaVatTu=@Vt",
                        new SqlParameter("@SL", ct.SoLuong),
                        new SqlParameter("@Vt", ct.MaVatTu));
                    if (rows == 0) throw new InvalidOperationException("Vật tư " + ct.MaVatTu + " không tồn tại");
                }
                else
                {
                    var ton = DatabaseHelper.ExecuteScalar(conn, trans,
                        "SELECT SoLuongTon FROM VatTu WITH (UPDLOCK) WHERE MaVatTu=@Vt",
                        new SqlParameter("@Vt", ct.MaVatTu));
                    if (ton == null) throw new InvalidOperationException("Vật tư " + ct.MaVatTu + " không tồn tại");
                    if (Convert.ToDecimal(ton) < ct.SoLuong)
                        throw new InvalidOperationException("Không đủ hàng trong kho");
                    DatabaseHelper.ExecuteNonQuery(conn, trans,
                        "UPDATE VatTu SET SoLuongTon = SoLuongTon - @SL WHERE MaVatTu=@Vt",
                        new SqlParameter("@SL", ct.SoLuong),
                        new SqlParameter("@Vt", ct.MaVatTu));
                }
            }

            trans.Commit();
            return maPhieu;
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }

    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT p.MaPhieu, p.LoaiPhieu, p.NgayLap, n.TenNCC, p.TongTien " +
            "FROM PhieuKho p LEFT JOIN NhaCungCap n ON p.MaNCC = n.MaNCC ORDER BY p.MaPhieu DESC");
    }
}
