using System;
using System.Data;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAL dal = new KhachHangDAL();

        public DataTable GetAll()
        {
            return dal.Search("");
        }

        public DataTable Search(string kw)
        {
            return dal.Search((kw ?? "").Trim());
        }

        private void Validate(KhachHangDTO k)
        {
            if (string.IsNullOrWhiteSpace(k.TenKH)) throw new Exception("Tên khách hàng không được để trống.");
            k.TenKH = k.TenKH.Trim();
            k.DienThoai = string.IsNullOrWhiteSpace(k.DienThoai) ? null : k.DienThoai.Trim();
        }

        public void Insert(KhachHangDTO k)
        {
            Validate(k);
            dal.Insert(k);
        }

        public void Update(KhachHangDTO k)
        {
            if (k.MaKH <= 0) throw new Exception("Vui lòng chọn khách hàng cần sửa.");
            Validate(k);
            if (dal.Update(k) == 0) throw new Exception("Không tìm thấy khách hàng cần sửa.");
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new Exception("Vui lòng chọn khách hàng cần xóa.");
            if (dal.Delete(ma) == 0) throw new Exception("Không tìm thấy khách hàng cần xóa.");
        }
    }
}
