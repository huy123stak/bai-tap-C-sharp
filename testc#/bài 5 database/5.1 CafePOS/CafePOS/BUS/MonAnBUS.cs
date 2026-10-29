using System.Data;
using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS;

public class MonAnBUS
{
    private readonly MonAnDAL dal = new();

    public DataTable GetAll()
    {
        return dal.GetAll();
    }

    public DataTable GetByDanhMuc(int maDanhMuc)
    {
        return dal.GetByDanhMuc(maDanhMuc);
    }

    public void Them(MonAnDTO m)
    {
        Validate(m);
        dal.Insert(m);
    }

    public void Sua(MonAnDTO m)
    {
        if (m.MaMon <= 0) throw new ArgumentException("Vui lòng chọn món cần sửa");
        Validate(m);
        dal.Update(m);
    }

    public void Xoa(int maMon)
    {
        if (maMon <= 0) throw new ArgumentException("Vui lòng chọn món cần xóa");
        dal.Delete(maMon);
    }

    private static void Validate(MonAnDTO m)
    {
        if (string.IsNullOrWhiteSpace(m.TenMon)) throw new ArgumentException("Tên món không được để trống");
        if (m.DonGia < 0) throw new ArgumentException("Đơn giá không được âm");
        if (m.MaDanhMuc <= 0) throw new ArgumentException("Vui lòng chọn danh mục");
    }
}
