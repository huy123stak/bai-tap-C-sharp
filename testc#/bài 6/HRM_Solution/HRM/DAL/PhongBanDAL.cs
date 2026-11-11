using System.Data;

namespace HRM.DAL
{
    public class PhongBanDAL
    {
        public DataTable GetAll()
        {
            return DbHelper.GetTable("SELECT MaPB, TenPB, Mota FROM PhongBan ORDER BY TenPB");
        }
    }
}
