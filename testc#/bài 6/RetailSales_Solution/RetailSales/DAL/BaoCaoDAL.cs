using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class BaoCaoDAL
    {
        public decimal DoanhThuNgay(DateTime ngay)
        {
            object kq = DbHelper.Scalar(
                "SELECT ISNULL(SUM(TongTien), 0) FROM HoaDon WHERE NgayLap >= @d AND NgayLap < DATEADD(DAY, 1, @d)",
                DbHelper.P("@d", ngay.Date));
            return Convert.ToDecimal(kq);
        }

        public decimal DoanhThuThang(int thang, int nam)
        {
            object kq = DbHelper.Scalar(
                "SELECT ISNULL(SUM(TongTien), 0) FROM HoaDon WHERE MONTH(NgayLap) = @t AND YEAR(NgayLap) = @n",
                DbHelper.P("@t", thang), DbHelper.P("@n", nam));
            return Convert.ToDecimal(kq);
        }

        public List<TopSanPhamDTO> Top5BanChay()
        {
            var ds = new List<TopSanPhamDTO>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT TOP 5 sp.MaSP, sp.TenSP, SUM(ct.SoLuong) AS TongSL, SUM(ct.SoLuong * ct.DonGia) AS DoanhThu " +
                "FROM ChiTietHoaDon ct JOIN SanPham sp ON ct.MaSP = sp.MaSP " +
                "GROUP BY sp.MaSP, sp.TenSP ORDER BY TongSL DESC, DoanhThu DESC", conn))
            {
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ds.Add(new TopSanPhamDTO
                        {
                            MaSP = rd.GetString(0),
                            TenSP = rd.GetString(1),
                            TongSoLuong = rd.GetInt32(2),
                            TongDoanhThu = rd.GetDecimal(3)
                        });
                    }
                }
            }
            return ds;
        }
    }
}
