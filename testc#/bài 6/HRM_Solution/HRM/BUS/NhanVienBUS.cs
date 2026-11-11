using System;
using System.Data;
using HRM.DAL;
using HRM.DTO;

namespace HRM.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL dal = new NhanVienDAL();

        public DataTable Search(string kw, int maPB)
        {
            return dal.Search((kw ?? "").Trim(), maPB);
        }

        private void Validate(NhanVienDTO n)
        {
            if (string.IsNullOrWhiteSpace(n.HoTen)) throw new Exception("Họ tên không được để trống.");
            if (n.MaPB <= 0) throw new Exception("Vui lòng chọn phòng ban.");
            if (n.LuongCoBan <= 0) throw new Exception("Lương cơ bản phải lớn hơn 0.");
            if (n.HeSoLuong <= 0) throw new Exception("Hệ số lương phải lớn hơn 0.");
            if (n.NgaySinh.HasValue && n.NgaySinh.Value > DateTime.Today) throw new Exception("Ngày sinh không hợp lệ.");
            n.HoTen = n.HoTen.Trim();
            n.CCCD = string.IsNullOrWhiteSpace(n.CCCD) ? null : n.CCCD.Trim();
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

        public void ChuyenPhongBan(int maNV, int maPB)
        {
            if (maNV <= 0) throw new Exception("Vui lòng chọn nhân viên cần chuyển phòng ban.");
            if (maPB <= 0) throw new Exception("Vui lòng chọn phòng ban mới.");
            if (dal.ChuyenPhongBan(maNV, maPB) == 0) throw new Exception("Không tìm thấy nhân viên cần chuyển.");
        }

        public void Delete(int ma)
        {
            if (ma <= 0) throw new Exception("Vui lòng chọn nhân viên cần xóa.");
            if (dal.Delete(ma) == 0) throw new Exception("Không tìm thấy nhân viên cần xóa.");
        }
    }
}
