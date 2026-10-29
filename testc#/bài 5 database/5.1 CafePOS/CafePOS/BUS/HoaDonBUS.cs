using System.Data;
using CafePOS.DAL;

namespace CafePOS.BUS;

public class HoaDonBUS
{
    private readonly HoaDonDAL dal = new();

    public DataTable GetChiTietByBan(int maBan)
    {
        return dal.GetChiTietByBan(maBan);
    }

    public void ThemMon(int maBan, int maMon, int soLuong)
    {
        if (maBan <= 0) throw new ArgumentException("Vui lòng chọn bàn");
        if (maMon <= 0) throw new ArgumentException("Vui lòng chọn món");
        if (soLuong <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
        dal.ThemMon(maBan, maMon, soLuong);
    }

    public decimal ThanhToan(int maBan)
    {
        if (maBan <= 0) throw new ArgumentException("Vui lòng chọn bàn");
        return dal.ThanhToan(maBan);
    }

    public void DoiBan(int banCu, int banMoi)
    {
        if (banCu <= 0) throw new ArgumentException("Vui lòng chọn bàn cần chuyển");
        if (banMoi <= 0) throw new ArgumentException("Vui lòng chọn bàn muốn chuyển đến");
        if (banCu == banMoi) throw new ArgumentException("Hai bàn phải khác nhau");
        dal.DoiBan(banCu, banMoi);
    }
}
