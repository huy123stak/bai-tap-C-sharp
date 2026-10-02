using System.Collections.Generic;

namespace TechMart
{
    public class DanhMuc
    {
        public int Ma { get; set; }
        public string Ten { get; set; }

        public static List<DanhMuc> DanhSach = new List<DanhMuc>
        {
            new DanhMuc { Ma = 1, Ten = "Điện thoại" },
            new DanhMuc { Ma = 2, Ten = "Laptop" },
            new DanhMuc { Ma = 3, Ten = "Phụ kiện" }
        };

        public static string LayTen(int ma)
        {
            foreach (DanhMuc dm in DanhSach)
            {
                if (dm.Ma == ma)
                    return dm.Ten;
            }
            return "";
        }
    }

    public class SanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int MaDanhMuc { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public string DuongDanAnh { get; set; }

        public string TenDanhMuc
        {
            get { return DanhMuc.LayTen(MaDanhMuc); }
        }
    }
}
