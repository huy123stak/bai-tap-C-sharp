using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using RetailSales.DTO;

namespace RetailSales.DAL
{
    public class HoaDonDAL
    {
        public int ThanhToan(HoaDonDTO hd, List<ChiTietHoaDonDTO> ds)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        int maHD;
                        using (var cmd = new SqlCommand(
                            "INSERT INTO HoaDon(NgayLap, MaNV, MaKH, TongTien) OUTPUT INSERTED.MaHD VALUES(@ngay, @nv, @kh, @tong)",
                            conn, trans))
                        {
                            cmd.Parameters.Add(DbHelper.P("@ngay", hd.NgayLap));
                            cmd.Parameters.Add(DbHelper.P("@nv", hd.MaNV));
                            cmd.Parameters.Add(DbHelper.P("@kh", hd.MaKH));
                            cmd.Parameters.Add(DbHelper.P("@tong", hd.TongTien));
                            maHD = (int)cmd.ExecuteScalar();
                        }

                        foreach (var ct in ds)
                        {
                            int ton;
                            using (var cmd = new SqlCommand(
                                "SELECT SoLuongTon FROM SanPham WITH (UPDLOCK, ROWLOCK) WHERE MaSP = @ma", conn, trans))
                            {
                                cmd.Parameters.Add(DbHelper.P("@ma", ct.MaSP));
                                object kq = cmd.ExecuteScalar();
                                if (kq == null)
                                    throw new Exception("Sản phẩm " + ct.MaSP + " không tồn tại.");
                                ton = Convert.ToInt32(kq);
                            }

                            if (ton < ct.SoLuong)
                            {
                                trans.Rollback();
                                throw new Exception("Sản phẩm " + ct.MaSP + " không đủ tồn kho (còn " + ton + ", cần " + ct.SoLuong + ").");
                            }

                            using (var cmd = new SqlCommand(
                                "INSERT INTO ChiTietHoaDon(MaHD, MaSP, SoLuong, DonGia) VALUES(@hd, @ma, @sl, @gia)",
                                conn, trans))
                            {
                                cmd.Parameters.Add(DbHelper.P("@hd", maHD));
                                cmd.Parameters.Add(DbHelper.P("@ma", ct.MaSP));
                                cmd.Parameters.Add(DbHelper.P("@sl", ct.SoLuong));
                                cmd.Parameters.Add(DbHelper.P("@gia", ct.DonGia));
                                cmd.ExecuteNonQuery();
                            }

                            using (var cmd = new SqlCommand(
                                "UPDATE SanPham SET SoLuongTon = SoLuongTon - @sl WHERE MaSP = @ma", conn, trans))
                            {
                                cmd.Parameters.Add(DbHelper.P("@sl", ct.SoLuong));
                                cmd.Parameters.Add(DbHelper.P("@ma", ct.MaSP));
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (hd.MaKH.HasValue)
                        {
                            int diem = (int)(hd.TongTien / 100000m);
                            using (var cmd = new SqlCommand(
                                "UPDATE KhachHang SET DiemTichLuy = DiemTichLuy + @diem WHERE MaKH = @kh", conn, trans))
                            {
                                cmd.Parameters.Add(DbHelper.P("@diem", diem));
                                cmd.Parameters.Add(DbHelper.P("@kh", hd.MaKH.Value));
                                cmd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                        return maHD;
                    }
                    catch (SqlException ex)
                    {
                        SafeRollback(trans);
                        throw DbHelper.Translate(ex);
                    }
                    catch
                    {
                        SafeRollback(trans);
                        throw;
                    }
                }
            }
        }

        private static void SafeRollback(SqlTransaction trans)
        {
            try
            {
                if (trans.Connection != null) trans.Rollback();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
