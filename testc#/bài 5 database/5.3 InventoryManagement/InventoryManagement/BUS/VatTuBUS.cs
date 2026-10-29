using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS;

public class VatTuBUS
{
    private readonly VatTuDAL dal = new();

    public DataTable GetAll()
    {
        return dal.GetAll();
    }

    public DataTable Search(string keyword)
    {
        return string.IsNullOrWhiteSpace(keyword) ? dal.GetAll() : dal.Search(keyword.Trim());
    }

    public void Them(VatTuDTO v)
    {
        Validate(v);
        dal.Insert(v);
    }

    public void Sua(VatTuDTO v)
    {
        Validate(v);
        dal.Update(v);
    }

    public void Xoa(string ma)
    {
        if (string.IsNullOrWhiteSpace(ma)) throw new ArgumentException("Vui lòng chọn vật tư cần xóa");
        dal.Delete(ma);
    }

    public DataTable GetDuoiDinhMuc(decimal nguong)
    {
        if (nguong < 0) throw new ArgumentException("Ngưỡng định mức không được âm");
        return dal.GetDuoiDinhMuc(nguong);
    }

    public decimal GetTongGiaTriTon()
    {
        return dal.GetTongGiaTriTon();
    }

    private static void Validate(VatTuDTO v)
    {
        if (string.IsNullOrWhiteSpace(v.MaVatTu)) throw new ArgumentException("Mã vật tư không được để trống");
        if (string.IsNullOrWhiteSpace(v.TenVatTu)) throw new ArgumentException("Tên vật tư không được để trống");
        if (v.SoLuongTon < 0) throw new ArgumentException("Số lượng tồn không được âm");
    }
}
