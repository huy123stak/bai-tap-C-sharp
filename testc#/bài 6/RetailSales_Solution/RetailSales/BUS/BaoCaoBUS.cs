using System;
using System.Collections.Generic;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class BaoCaoBUS
    {
        private readonly BaoCaoDAL dal = new BaoCaoDAL();

        public decimal DoanhThuNgay(DateTime ngay)
        {
            return dal.DoanhThuNgay(ngay);
        }

        public decimal DoanhThuThang(int thang, int nam)
        {
            if (thang < 1 || thang > 12) throw new Exception("Tháng không hợp lệ.");
            return dal.DoanhThuThang(thang, nam);
        }

        public List<TopSanPhamDTO> Top5BanChay()
        {
            return dal.Top5BanChay();
        }
    }
}
