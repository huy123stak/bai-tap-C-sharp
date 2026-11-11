using System.Data;
using HRM.DAL;

namespace HRM.BUS
{
    public class BaoCaoBUS
    {
        private readonly BaoCaoDAL dal = new BaoCaoDAL();

        public decimal TongLuongThang(int thang, int nam)
        {
            return dal.TongLuongThang(ChamCongBUS.ThangNam(thang, nam));
        }

        public DataTable ChiTietLuong(int thang, int nam)
        {
            return dal.ChiTietLuong(ChamCongBUS.ThangNam(thang, nam));
        }

        public DataTable LuongTheoPhongBan(int thang, int nam)
        {
            return dal.LuongTheoPhongBan(ChamCongBUS.ThangNam(thang, nam));
        }
    }
}
