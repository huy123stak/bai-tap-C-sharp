using System;
using System.Data;
using System.Data.SqlClient;

namespace HRM.DAL
{
    public class LuongDAL
    {
        public DataTable TamTinh(string thangNam, double ngayChuan, double donGiaOT)
        {
            return DbHelper.GetTable(
                "SELECT nv.MaNV, nv.HoTen, pb.TenPB, nv.LuongCoBan, nv.HeSoLuong, cc.SoNgayCong, cc.SoGioOT, " +
                "CAST((nv.LuongCoBan * nv.HeSoLuong / @ngay) * cc.SoNgayCong + cc.SoGioOT * @ot AS DECIMAL(18,2)) AS ThucLinh " +
                "FROM BangChamCong cc JOIN NhanVien nv ON nv.MaNV = cc.MaNV " +
                "LEFT JOIN PhongBan pb ON pb.MaPB = nv.MaPB " +
                "WHERE cc.ThangNam = @tn ORDER BY nv.MaNV",
                DbHelper.P("@tn", thangNam), DbHelper.P("@ngay", ngayChuan), DbHelper.P("@ot", donGiaOT));
        }

        public int ChotLuong(string thangNam, double ngayChuan, double donGiaOT)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        int tong, daKhoa;
                        using (var cmd = new SqlCommand(
                            "SELECT COUNT(*), ISNULL(SUM(CAST(DaKhoa AS INT)), 0) FROM BangChamCong WITH (UPDLOCK, HOLDLOCK) WHERE ThangNam = @tn",
                            conn, trans))
                        {
                            cmd.Parameters.Add(DbHelper.P("@tn", thangNam));
                            using (var rd = cmd.ExecuteReader())
                            {
                                rd.Read();
                                tong = Convert.ToInt32(rd[0]);
                                daKhoa = Convert.ToInt32(rd[1]);
                            }
                        }

                        if (tong == 0)
                            throw new Exception("Tháng " + thangNam + " chưa có dữ liệu chấm công để chốt lương.");
                        if (daKhoa > 0)
                            throw new Exception("Tháng " + thangNam + " đã được chốt lương trước đó.");

                        int soDong;
                        using (var cmd = new SqlCommand(
                            "INSERT INTO BangLuong(MaNV, ThangNam, ThucLinh) " +
                            "SELECT nv.MaNV, cc.ThangNam, " +
                            "CAST((nv.LuongCoBan * nv.HeSoLuong / @ngay) * cc.SoNgayCong + cc.SoGioOT * @ot AS DECIMAL(18,2)) " +
                            "FROM BangChamCong cc JOIN NhanVien nv ON nv.MaNV = cc.MaNV WHERE cc.ThangNam = @tn",
                            conn, trans))
                        {
                            cmd.Parameters.Add(DbHelper.P("@tn", thangNam));
                            cmd.Parameters.Add(DbHelper.P("@ngay", ngayChuan));
                            cmd.Parameters.Add(DbHelper.P("@ot", donGiaOT));
                            soDong = cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new SqlCommand(
                            "UPDATE BangChamCong SET DaKhoa = 1 WHERE ThangNam = @tn", conn, trans))
                        {
                            cmd.Parameters.Add(DbHelper.P("@tn", thangNam));
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return soDong;
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
