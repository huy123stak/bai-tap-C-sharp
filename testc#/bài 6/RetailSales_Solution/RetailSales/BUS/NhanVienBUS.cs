using System;
using System.Data;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable GetAll()
        {
            return dal.Search("");
        }

        public DataTable Search(string kw)
        {
            return dal.Search((kw ?? "").Trim());
        }

        private void Validate(NhanVienDTO n)
        {
            if (string.IsNullOrWhiteSpace(n.TenNV)) throw new Exception("Tên nhân viên không được để trống.");
            n.TenNV = n.TenNV.Trim();
            n.SoDienThoai = string.IsNullOrWhiteSpace(n.SoDienThoai) ? null : n.SoDienThoai.Trim();
            n.ChucVu = string.IsNullOrWhiteSpace(n.ChucVu) ? null : n.ChucVu.Trim();
        }

        public void Insert(NhanVienDTO n)
        {
            Validate(n);
            dal.Insert(n);
        }

        public void Update(NhanVienDTO n)
        {
            if (n.MaNV <= 0) throw new Exception("Vui lòng chọn nhân viên cần sửa.");
            Validate(n);
            if (dal.Update(n) == 0) throw new Exception("Không tìm thấy nhân viên cần sửa.");
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new Exception("Vui lòng chọn nhân viên cần xóa.");
            if (dal.Delete(ma) == 0) throw new Exception("Không tìm thấy nhân viên cần xóa.");
        }
    }
}
