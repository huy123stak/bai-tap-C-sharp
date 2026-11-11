using System;
using System.Data;
using HRM.DAL;

namespace HRM.BUS
{
    public class LuongBUS
    {
        public const double NgayCongChuan = 22.0;
        public const double DonGiaOT = 50000.0;

        private readonly LuongDAL dal = new LuongDAL();

        public DataTable TamTinh(int thang, int nam)
        {
            return dal.TamTinh(ChamCongBUS.ThangNam(thang, nam), NgayCongChuan, DonGiaOT);
        }

        public decimal TinhThucLinh(decimal luongCoBan, double heSo, double ngayCong, double gioOT)
        {
            return (decimal)((double)luongCoBan * heSo / NgayCongChuan * ngayCong + gioOT * DonGiaOT);
        }

        public int ChotLuong(int thang, int nam)
        {
            return dal.ChotLuong(ChamCongBUS.ThangNam(thang, nam), NgayCongChuan, DonGiaOT);
        }
    }
}
