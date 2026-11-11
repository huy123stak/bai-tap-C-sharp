using System;
using System.Collections.Generic;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL dal = new HoaDonDAL();

        public static decimal TinhTongTien(List<ChiTietHoaDonDTO> ds)
        {
            decimal tong = 0;
            foreach (var ct in ds) tong += ct.DonGia * ct.SoLuong;
            return tong;
        }

        public static int TinhDiem(decimal tongTien)
        {
            return (int)(tongTien / 100000m);
        }

        public int ThanhToan(HoaDonDTO hd, List<ChiTietHoaDonDTO> ds)
        {
            if (ds == null || ds.Count == 0) throw new Exception("Giỏ hàng đang trống.");
            if (hd.MaNV <= 0) throw new Exception("Vui lòng chọn nhân viên lập hóa đơn.");
            foreach (var ct in ds)
            {
                if (ct.SoLuong <= 0) throw new Exception("Số lượng bán phải lớn hơn 0.");
            }
            hd.NgayLap = DateTime.Now;
            hd.TongTien = TinhTongTien(ds);
            return dal.ThanhToan(hd, ds);
        }
    }
}
