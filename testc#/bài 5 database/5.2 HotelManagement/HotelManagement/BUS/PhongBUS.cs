using System.Data;
using HotelManagement.DAL;

namespace HotelManagement.BUS;

public class PhongBUS
{
    private readonly PhongDAL dal = new();

    public DataTable GetAll()
    {
        return dal.GetAll();
    }

    public DataTable GetTrong()
    {
        return dal.GetByTrangThai("Trống");
    }

    public DataTable GetDangO()
    {
        return dal.GetByTrangThai("Đang ở");
    }

    public void DonXong(string soPhong)
    {
        if (string.IsNullOrWhiteSpace(soPhong)) throw new ArgumentException("Vui lòng chọn phòng");
        dal.UpdateTrangThai(soPhong, "Trống");
    }
}
