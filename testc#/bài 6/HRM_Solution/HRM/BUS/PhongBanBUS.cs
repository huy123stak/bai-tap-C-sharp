using System.Data;
using HRM.DAL;

namespace HRM.BUS
{
    public class PhongBanBUS
    {
        private readonly PhongBanDAL dal = new PhongBanDAL();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }
    }
}
