using System.Data;
using HRM.DTO;

namespace HRM.DAL
{
    public class NhanVienDAL
    {
        public DataTable Search(string kw, int maPB)
        {
            return DbHelper.GetTable(
                "SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.CCCD, nv.MaPB, pb.TenPB, nv.LuongCoBan, nv.HeSoLuong " +
                "FROM NhanVien nv JOIN PhongBan pb ON pb.MaPB = nv.MaPB " +
                "WHERE (nv.HoTen LIKE @kw OR nv.CCCD LIKE @kw OR pb.TenPB LIKE @kw) AND (@pb = 0 OR nv.MaPB = @pb) " +
                "ORDER BY nv.HoTen",
                DbHelper.P("@kw", "%" + kw + "%"), DbHelper.P("@pb", maPB));
        }

        public int Insert(NhanVienDTO n)
        {
            return DbHelper.Execute(
                "INSERT INTO NhanVien(HoTen, NgaySinh, CCCD, MaPB, LuongCoBan, HeSoLuong) VALUES(@ten, @ns, @cccd, @pb, @luong, @heso)",
                DbHelper.P("@ten", n.HoTen), DbHelper.P("@ns", n.NgaySinh), DbHelper.P("@cccd", n.CCCD),
                DbHelper.P("@pb", n.MaPB), DbHelper.P("@luong", n.LuongCoBan), DbHelper.P("@heso", n.HeSoLuong));
        }

        public int Update(NhanVienDTO n)
        {
            return DbHelper.Execute(
                "UPDATE NhanVien SET HoTen = @ten, NgaySinh = @ns, CCCD = @cccd, MaPB = @pb, LuongCoBan = @luong, HeSoLuong = @heso WHERE MaNV = @ma",
                DbHelper.P("@ma", n.MaNV), DbHelper.P("@ten", n.HoTen), DbHelper.P("@ns", n.NgaySinh),
                DbHelper.P("@cccd", n.CCCD), DbHelper.P("@pb", n.MaPB),
                DbHelper.P("@luong", n.LuongCoBan), DbHelper.P("@heso", n.HeSoLuong));
        }

        public int ChuyenPhongBan(int maNV, int maPB)
        {
            return DbHelper.Execute(
                "UPDATE NhanVien SET MaPB = @pb WHERE MaNV = @ma",
                DbHelper.P("@ma", maNV), DbHelper.P("@pb", maPB));
        }

        public int Delete(int ma)
        {
            return DbHelper.Execute("DELETE FROM NhanVien WHERE MaNV = @ma", DbHelper.P("@ma", ma));
        }
    }
}
