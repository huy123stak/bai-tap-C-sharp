namespace HotelManagement.DTO;

public class BillDTO
{
    public int MaPhieu { get; set; }
    public string SoPhong { get; set; }
    public string HoTen { get; set; }
    public DateTime NgayNhan { get; set; }
    public DateTime NgayTra { get; set; }
    public int SoGio { get; set; }
    public decimal GiaTheoGio { get; set; }
    public decimal TienPhong { get; set; }
    public decimal TienDichVu { get; set; }
    public decimal TongTien { get; set; }
}
