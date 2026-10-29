using System.Data;
using CafePOS.DTO;

namespace CafePOS.DAL;

public class BanAnDAL
{
    public List<BanAnDTO> GetAll()
    {
        var dt = DatabaseHelper.ExecuteQuery("SELECT MaBan, TenBan, TrangThai FROM BanAn ORDER BY MaBan");
        var list = new List<BanAnDTO>();
        foreach (DataRow r in dt.Rows)
        {
            list.Add(new BanAnDTO
            {
                MaBan = Convert.ToInt32(r["MaBan"]),
                TenBan = r["TenBan"].ToString(),
                TrangThai = r["TrangThai"].ToString()
            });
        }
        return list;
    }
}
