using System.Data;

namespace HotelManagement.DAL;

public class ServiceDAL
{
    public DataTable GetAll()
    {
        return DatabaseHelper.ExecuteQuery("SELECT MaDV, TenDV, DonGia FROM DichVu ORDER BY TenDV");
    }
}
