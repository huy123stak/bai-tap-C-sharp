using System;
using System.Data;
using HRM.DAL;

namespace HRM.BUS
{
    public class ChamCongBUS
    {
        private readonly ChamCongDAL dal = new ChamCongDAL();

        public static string ThangNam(int thang, int nam)
        {
            return string.Format("{0:00}/{1}", thang, nam);
        }

        public DataTable Load(int thang, int nam)
        {
            return dal.Load(ThangNam(thang, nam));
        }

        public bool DaChot(int thang, int nam)
        {
            return dal.DaChot(ThangNam(thang, nam));
        }

        public int Save(DataTable dt, int thang, int nam)
        {
            if (dt == null) throw new Exception("Chưa có dữ liệu chấm công để lưu.");
            if (dal.DaChot(ThangNam(thang, nam))) throw new Exception("Tháng này đã chốt lương, không thể sửa chấm công.");

            foreach (DataRow r in dt.Rows)
            {
                if (r.RowState != DataRowState.Modified) continue;
                string ten = Convert.ToString(r["HoTen"]);
                if (r["SoNgayCong"] == DBNull.Value || r["SoGioOT"] == DBNull.Value)
                    throw new Exception("Nhân viên " + ten + ": số ngày công và số giờ OT không được để trống.");
                double ngay = Convert.ToDouble(r["SoNgayCong"]);
                double ot = Convert.ToDouble(r["SoGioOT"]);
                if (ngay < 0 || ngay > 31) throw new Exception("Nhân viên " + ten + ": số ngày công phải từ 0 đến 31.");
                if (ot < 0) throw new Exception("Nhân viên " + ten + ": số giờ OT không được âm.");
            }

            return dal.Save(dt);
        }
    }
}
