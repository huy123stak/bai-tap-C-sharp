using System.Data;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class NhanVienDAL
    {
        public DataTable Search(string kw)
        {
            return DbHelper.GetTable(
                "SELECT MaNV, TenNV, SoDienThoai, ChucVu FROM NhanVien WHERE TenNV LIKE @kw OR SoDienThoai LIKE @kw ORDER BY TenNV",
                DbHelper.P("@kw", "%" + kw + "%"));
        }

        public int Insert(NhanVienDTO n)
        {
            return DbHelper.Execute(
                "INSERT INTO NhanVien(TenNV, SoDienThoai, ChucVu) VALUES(@ten, @sdt, @cv)",
                DbHelper.P("@ten", n.TenNV), DbHelper.P("@sdt", n.SoDienThoai), DbHelper.P("@cv", n.ChucVu));
        }

        public int Update(NhanVienDTO n)
        {
            return DbHelper.Execute(
                "UPDATE NhanVien SET TenNV = @ten, SoDienThoai = @sdt, ChucVu = @cv WHERE MaNV = @ma",
                DbHelper.P("@ma", n.MaNV), DbHelper.P("@ten", n.TenNV),
                DbHelper.P("@sdt", n.SoDienThoai), DbHelper.P("@cv", n.ChucVu));
        }

        public int Delete(int ma)
        {
            return DbHelper.Execute("DELETE FROM NhanVien WHERE MaNV = @ma", DbHelper.P("@ma", ma));
        }
    }
}
