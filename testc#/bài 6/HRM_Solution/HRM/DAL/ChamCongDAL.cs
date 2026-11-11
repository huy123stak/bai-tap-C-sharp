using System;
using System.Data;
using System.Data.SqlClient;

namespace HRM.DAL
{
    public class ChamCongDAL
    {
        private SqlDataAdapter adapter;
        private SqlCommandBuilder builder;

        public DataTable Load(string thangNam)
        {
            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(
                        "INSERT INTO BangChamCong(MaNV, ThangNam) SELECT nv.MaNV, @tn FROM NhanVien nv " +
                        "WHERE NOT EXISTS (SELECT 1 FROM BangChamCong cc WHERE cc.MaNV = nv.MaNV AND cc.ThangNam = @tn)", conn))
                    {
                        cmd.Parameters.Add(DbHelper.P("@tn", thangNam));
                        cmd.ExecuteNonQuery();
                    }
                }

                adapter = new SqlDataAdapter(
                    "SELECT MaCong, MaNV, ThangNam, SoNgayCong, SoGioOT, DaKhoa FROM BangChamCong WHERE ThangNam = @tn ORDER BY MaNV",
                    DbHelper.ConnStr);
                adapter.SelectCommand.Parameters.Add(DbHelper.P("@tn", thangNam));
                builder = new SqlCommandBuilder(adapter);
                builder.ConflictOption = ConflictOption.OverwriteChanges;

                var dt = new DataTable("BangChamCong");
                adapter.Fill(dt);

                var names = DbHelper.GetTable("SELECT MaNV, HoTen FROM NhanVien");
                var map = new System.Collections.Generic.Dictionary<int, string>();
                foreach (DataRow r in names.Rows) map[Convert.ToInt32(r["MaNV"])] = Convert.ToString(r["HoTen"]);

                dt.Columns.Add("HoTen", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    string ten;
                    r["HoTen"] = map.TryGetValue(Convert.ToInt32(r["MaNV"]), out ten) ? ten : "";
                }
                dt.AcceptChanges();
                return dt;
            }
            catch (SqlException ex)
            {
                throw DbHelper.Translate(ex);
            }
        }

        public int Save(DataTable dt)
        {
            if (adapter == null) throw new Exception("Vui lòng nạp dữ liệu chấm công trước khi lưu.");
            try
            {
                return adapter.Update(dt);
            }
            catch (SqlException ex)
            {
                throw DbHelper.Translate(ex);
            }
        }

        public bool DaChot(string thangNam)
        {
            object kq = DbHelper.Scalar(
                "SELECT COUNT(*) FROM BangChamCong WHERE ThangNam = @tn AND DaKhoa = 1",
                DbHelper.P("@tn", thangNam));
            return Convert.ToInt32(kq) > 0;
        }
    }
}
