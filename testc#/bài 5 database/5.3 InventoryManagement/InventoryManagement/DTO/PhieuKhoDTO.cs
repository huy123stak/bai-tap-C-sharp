namespace InventoryManagement.DTO;

public class PhieuKhoDTO
{
    public int MaPhieu { get; set; }
    public string LoaiPhieu { get; set; }
    public DateTime NgayLap { get; set; }
    public int? MaNCC { get; set; }
    public decimal TongTien { get; set; }
}
