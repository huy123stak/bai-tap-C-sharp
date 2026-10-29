using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS;

public class NhaCungCapBUS
{
    private readonly NhaCungCapDAL dal = new();

    public DataTable GetAll()
    {
        return dal.GetAll();
    }

    public DataTable Search(string keyword)
    {
        return string.IsNullOrWhiteSpace(keyword) ? dal.GetAll() : dal.Search(keyword.Trim());
    }

    public void Them(NhaCungCapDTO n)
    {
        Validate(n);
        dal.Insert(n);
    }

    public void Sua(NhaCungCapDTO n)
    {
        if (n.MaNCC <= 0) throw new ArgumentException("Vui lòng chọn nhà cung cấp cần sửa");
        Validate(n);
        dal.Update(n);
    }

    public void Xoa(int ma)
    {
        if (ma <= 0) throw new ArgumentException("Vui lòng chọn nhà cung cấp cần xóa");
        dal.Delete(ma);
    }

    private static void Validate(NhaCungCapDTO n)
    {
        if (string.IsNullOrWhiteSpace(n.TenNCC)) throw new ArgumentException("Tên nhà cung cấp không được để trống");
    }
}
