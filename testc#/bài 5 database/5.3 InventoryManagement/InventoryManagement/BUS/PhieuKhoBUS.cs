using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS;

public class PhieuKhoBUS
{
    private readonly PhieuKhoDAL dal = new();

    public int LapPhieu(string loaiPhieu, int? maNCC, List<ChiTietPhieuKhoDTO> chiTiet)
    {
        if (loaiPhieu != "NHAP" && loaiPhieu != "XUAT") throw new ArgumentException("Loại phiếu không hợp lệ");
        if (loaiPhieu == "NHAP" && !maNCC.HasValue) throw new ArgumentException("Phiếu nhập phải chọn nhà cung cấp");
        if (chiTiet == null || chiTiet.Count == 0) throw new ArgumentException("Phiếu chưa có vật tư nào");
        foreach (var ct in chiTiet)
        {
            if (ct.SoLuong <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
            if (ct.DonGia < 0) throw new ArgumentException("Đơn giá không được âm");
        }
        var phieu = new PhieuKhoDTO
        {
            LoaiPhieu = loaiPhieu,
            MaNCC = loaiPhieu == "NHAP" ? maNCC : null,
            TongTien = chiTiet.Sum(x => x.SoLuong * x.DonGia)
        };
        return dal.LapPhieu(phieu, chiTiet);
    }

    public DataTable GetAll()
    {
        return dal.GetAll();
    }
}
