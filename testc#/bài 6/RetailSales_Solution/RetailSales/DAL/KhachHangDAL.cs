using System.Data;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class KhachHangDAL
    {
        public DataTable Search(string kw)
        {
            return DbHelper.GetTable(
                "SELECT MaKH, TenKH, DienThoai, DiemTichLuy FROM KhachHang WHERE TenKH LIKE @kw OR DienThoai LIKE @kw ORDER BY TenKH",
                DbHelper.P("@kw", "%" + kw + "%"));
        }

        public int Insert(KhachHangDTO k)
        {
            return DbHelper.Execute(
                "INSERT INTO KhachHang(TenKH, DienThoai) VALUES(@ten, @sdt)",
                DbHelper.P("@ten", k.TenKH), DbHelper.P("@sdt", k.DienThoai));
        }

        public int Update(KhachHangDTO k)
        {
            return DbHelper.Execute(
                "UPDATE KhachHang SET TenKH = @ten, DienThoai = @sdt WHERE MaKH = @ma",
                DbHelper.P("@ma", k.MaKH), DbHelper.P("@ten", k.TenKH), DbHelper.P("@sdt", k.DienThoai));
        }

        public int Delete(int ma)
        {
            return DbHelper.Execute("DELETE FROM KhachHang WHERE MaKH = @ma", DbHelper.P("@ma", ma));
        }
    }
}
