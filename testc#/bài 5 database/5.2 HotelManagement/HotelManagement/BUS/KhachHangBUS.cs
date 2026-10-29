using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS;

public class KhachHangBUS
{
    private readonly KhachHangDAL dal = new();

    public KhachHangDTO TraCuuCCCD(string cccd)
    {
        if (string.IsNullOrWhiteSpace(cccd)) throw new ArgumentException("Vui lòng nhập CCCD");
        return dal.GetByCCCD(cccd.Trim());
    }
}
