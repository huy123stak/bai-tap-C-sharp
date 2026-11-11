using System;
using System.Data;

namespace HRM.DAL
{
    public class BaoCaoDAL
    {
        public decimal TongLuongThang(string thangNam)
        {
            object kq = DbHelper.Scalar(
                "SELECT ISNULL(SUM(ThucLinh), 0) FROM BangLuong WHERE ThangNam = @tn",
                DbHelper.P("@tn", thangNam));
            return Convert.ToDecimal(kq);
        }

        public DataTable ChiTietLuong(string thangNam)
        {
            return DbHelper.GetTable(
                "SELECT nv.MaNV, nv.HoTen, pb.TenPB, bl.ThangNam, bl.ThucLinh " +
                "FROM BangLuong bl JOIN NhanVien nv ON nv.MaNV = bl.MaNV " +
                "LEFT JOIN PhongBan pb ON pb.MaPB = nv.MaPB " +
                "WHERE bl.ThangNam = @tn ORDER BY bl.ThucLinh DESC",
                DbHelper.P("@tn", thangNam));
        }

        public DataTable LuongTheoPhongBan(string thangNam)
        {
            return DbHelper.GetTable(
                "SELECT pb.TenPB, COUNT(bl.MaNV) AS SoNhanVien, ISNULL(SUM(bl.ThucLinh), 0) AS TongLuong " +
                "FROM PhongBan pb LEFT JOIN NhanVien nv ON nv.MaPB = pb.MaPB " +
                "LEFT JOIN BangLuong bl ON bl.MaNV = nv.MaNV AND bl.ThangNam = @tn " +
                "GROUP BY pb.TenPB ORDER BY TongLuong DESC",
                DbHelper.P("@tn", thangNam));
        }
    }
}
