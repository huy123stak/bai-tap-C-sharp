using System;
using System.Data;
using RetailSales.DAL;
using RetailSales.DTO;

namespace RetailSales.BUS
{
    public class SanPhamBUS
    {
        private readonly SanPhamDAL dal = new SanPhamDAL();

        public DataTable GetAll()
        {
            return dal.Search("");
        }

        public DataTable Search(string kw)
        {
            return dal.Search((kw ?? "").Trim());
        }

        public SanPhamDTO GetById(string ma)
        {
            return dal.GetById(ma);
        }

        private void Validate(SanPhamDTO s)
        {
            if (string.IsNullOrWhiteSpace(s.MaSP)) throw new Exception("Mã sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(s.TenSP)) throw new Exception("Tên sản phẩm không được để trống.");
            if (s.GiaBan <= 0) throw new Exception("Giá bán phải lớn hơn 0.");
            if (s.SoLuongTon < 0) throw new Exception("Số lượng tồn không được âm.");
            s.MaSP = s.MaSP.Trim();
            s.TenSP = s.TenSP.Trim();
        }

        public void Insert(SanPhamDTO s)
        {
            Validate(s);
            if (dal.GetById(s.MaSP) != null) throw new Exception("Mã sản phẩm đã tồn tại.");
            dal.Insert(s);
        }

        public void Update(SanPhamDTO s)
        {
            Validate(s);
            if (dal.Update(s) == 0) throw new Exception("Không tìm thấy sản phẩm cần sửa.");
        }

        public void Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma)) throw new Exception("Vui lòng chọn sản phẩm cần xóa.");
            if (dal.Delete(ma) == 0) throw new Exception("Không tìm thấy sản phẩm cần xóa.");
        }
    }
}
