using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS;

public class BanAnBUS
{
    private readonly BanAnDAL dal = new();

    public List<BanAnDTO> GetAll()
    {
        return dal.GetAll();
    }
}
