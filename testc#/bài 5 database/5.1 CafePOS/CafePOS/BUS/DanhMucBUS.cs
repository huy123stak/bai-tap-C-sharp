using System.Data;
using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS;

public class DanhMucBUS
{
    private readonly DanhMucDAL dal = new();

    public DataTable GetAll()
    {
        return dal.GetAll();
    }

    public void Them(DanhMucDTO dm)
    {
        Validate(dm);
        dal.Insert(dm);
    }

    public void Sua(DanhMucDTO dm)
    {
        if (dm.MaDanhMuc <= 0) throw new ArgumentException("Vui lòng chọn danh mục cần sửa");
        Validate(dm);
        dal.Update(dm);
    }

    public void Xoa(int ma)
    {
        if (ma <= 0) throw new ArgumentException("Vui lòng chọn danh mục cần xóa");
        dal.Delete(ma);
    }

    private static void Validate(DanhMucDTO dm)
    {
        if (string.IsNullOrWhiteSpace(dm.TenDanhMuc))
            throw new ArgumentException("Tên danh mục không được để trống");
    }
}
