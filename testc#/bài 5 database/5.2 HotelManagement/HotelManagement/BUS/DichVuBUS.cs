using System.Data;
using HotelManagement.DAL;

namespace HotelManagement.BUS;

public class DichVuBUS
{
    private readonly ServiceDAL dal = new();

    public DataTable GetAll()
    {
        return dal.GetAll();
    }
}
