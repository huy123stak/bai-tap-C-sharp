using System.Data;
using System.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL;

public class VatTuDAL
{
    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu ORDER BY MaVatTu");
    }

    public DataTable Search(string keyword)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu " +
            "WHERE MaVatTu LIKE @KW OR TenVatTu LIKE @KW ORDER BY MaVatTu",
            new SqlParameter("@KW", "%" + keyword + "%"));
    }

    public int Insert(VatTuDTO v)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "INSERT INTO VatTu(MaVatTu, TenVatTu, DVT, SoLuongTon) VALUES(@Ma, @Ten, @DVT, @SL)",
            new SqlParameter("@Ma", v.MaVatTu),
            new SqlParameter("@Ten", v.TenVatTu),
            new SqlParameter("@DVT", (object)v.DVT ?? DBNull.Value),
            new SqlParameter("@SL", v.SoLuongTon));
    }

    public int Update(VatTuDTO v)
    {
        return DatabaseHelper.ExecuteNonQuery(
            "UPDATE VatTu SET TenVatTu=@Ten, DVT=@DVT, SoLuongTon=@SL WHERE MaVatTu=@Ma",
            new SqlParameter("@Ten", v.TenVatTu),
            new SqlParameter("@DVT", (object)v.DVT ?? DBNull.Value),
            new SqlParameter("@SL", v.SoLuongTon),
            new SqlParameter("@Ma", v.MaVatTu));
    }

    public int Delete(string ma)
    {
        return DatabaseHelper.ExecuteNonQuery("DELETE FROM VatTu WHERE MaVatTu=@Ma",
            new SqlParameter("@Ma", ma));
    }

    public DataTable GetDuoiDinhMuc(decimal nguong)
    {
        return DatabaseHelper.ExecuteQuery(
            "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu WHERE SoLuongTon < @N ORDER BY SoLuongTon",
            new SqlParameter("@N", nguong));
    }

    public decimal GetTongGiaTriTon()
    {
        var o = DatabaseHelper.ExecuteScalar(
            "SELECT ISNULL(SUM(vt.SoLuongTon * ISNULL(" +
            "(SELECT AVG(ct.DonGia) FROM ChiTietPhieuKho ct JOIN PhieuKho p ON ct.MaPhieu = p.MaPhieu " +
            "WHERE ct.MaVatTu = vt.MaVatTu AND p.LoaiPhieu = N'NHAP'), 0)), 0) FROM VatTu vt");
        return Convert.ToDecimal(o);
    }
}
