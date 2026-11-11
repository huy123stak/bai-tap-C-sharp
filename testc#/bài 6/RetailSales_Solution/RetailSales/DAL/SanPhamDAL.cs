using System.Data;
using System.Data.SqlClient;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class SanPhamDAL
    {
        public DataTable Search(string kw)
        {
            return DbHelper.GetTable(
                "SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham WHERE TenSP LIKE @kw OR MaSP LIKE @kw ORDER BY TenSP",
                DbHelper.P("@kw", "%" + kw + "%"));
        }

        public SanPhamDTO GetById(string ma)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqlCommand("SELECT MaSP, TenSP, GiaBan, SoLuongTon FROM SanPham WHERE MaSP = @ma", conn))
            {
                cmd.Parameters.Add(DbHelper.P("@ma", ma));
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return new SanPhamDTO
                    {
                        MaSP = rd.GetString(0),
                        TenSP = rd.GetString(1),
                        GiaBan = rd.GetDecimal(2),
                        SoLuongTon = rd.GetInt32(3)
                    };
                }
            }
        }

        public int Insert(SanPhamDTO s)
        {
            return DbHelper.Execute(
                "INSERT INTO SanPham(MaSP, TenSP, GiaBan, SoLuongTon) VALUES(@ma, @ten, @gia, @ton)",
                DbHelper.P("@ma", s.MaSP), DbHelper.P("@ten", s.TenSP),
                DbHelper.P("@gia", s.GiaBan), DbHelper.P("@ton", s.SoLuongTon));
        }

        public int Update(SanPhamDTO s)
        {
            return DbHelper.Execute(
                "UPDATE SanPham SET TenSP = @ten, GiaBan = @gia, SoLuongTon = @ton WHERE MaSP = @ma",
                DbHelper.P("@ma", s.MaSP), DbHelper.P("@ten", s.TenSP),
                DbHelper.P("@gia", s.GiaBan), DbHelper.P("@ton", s.SoLuongTon));
        }

        public int Delete(string ma)
        {
            return DbHelper.Execute("DELETE FROM SanPham WHERE MaSP = @ma", DbHelper.P("@ma", ma));
        }
    }
}
