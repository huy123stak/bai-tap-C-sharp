using System.Data;
using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS;

public class PhieuBUS
{
    private readonly PhieuDAL phieuDal = new();
    private readonly PhongDAL phongDal = new();

    public int CheckIn(string soPhong, KhachHangDTO kh)
    {
        if (string.IsNullOrWhiteSpace(soPhong)) throw new ArgumentException("Vui lòng chọn phòng trống");
        if (string.IsNullOrWhiteSpace(kh.CCCD)) throw new ArgumentException("CCCD không được để trống");
        if (string.IsNullOrWhiteSpace(kh.HoTen)) throw new ArgumentException("Họ tên không được để trống");
        return phieuDal.CheckIn(soPhong, kh);
    }

    public PhieuDatPhongDTO GetPhieuDangO(string soPhong)
    {
        if (string.IsNullOrWhiteSpace(soPhong)) return null;
        return phieuDal.GetPhieuDangO(soPhong);
    }

    public void ThemDichVu(string soPhong, int maDV, int soLuong)
    {
        if (soLuong <= 0) throw new ArgumentException("Số lượng phải lớn hơn 0");
        if (maDV <= 0) throw new ArgumentException("Vui lòng chọn dịch vụ");
        var phieu = phieuDal.GetPhieuDangO(soPhong);
        if (phieu == null) throw new InvalidOperationException("Phòng chưa có khách đang ở");
        phieuDal.AddService(new SuDungDichVuDTO { MaPhieu = phieu.MaPhieu, MaDV = maDV, SoLuong = soLuong });
    }

    public DataTable GetDichVuDaDung(string soPhong)
    {
        var phieu = phieuDal.GetPhieuDangO(soPhong);
        if (phieu == null) return new DataTable();
        return phieuDal.GetServicesByPhieu(phieu.MaPhieu);
    }

    public BillDTO TinhHoaDon(string soPhong)
    {
        var phieu = phieuDal.GetPhieuDangO(soPhong);
        if (phieu == null) throw new InvalidOperationException("Phòng chưa có khách đang ở");
        decimal gia = phongDal.GetGia(soPhong);
        DateTime tra = DateTime.Now;
        int gio = (int)Math.Ceiling((tra - phieu.NgayNhanPhong).TotalHours);
        if (gio < 1) gio = 1;
        decimal tienPhong = gio * gia;
        decimal tienDV = phieuDal.GetTongTienDichVu(phieu.MaPhieu);
        return new BillDTO
        {
            MaPhieu = phieu.MaPhieu,
            SoPhong = soPhong,
            HoTen = phieuDal.GetTenKhach(phieu.MaKH),
            NgayNhan = phieu.NgayNhanPhong,
            NgayTra = tra,
            SoGio = gio,
            GiaTheoGio = gia,
            TienPhong = tienPhong,
            TienDichVu = tienDV,
            TongTien = tienPhong + tienDV
        };
    }

    public void CheckOut(BillDTO bill)
    {
        if (bill == null) throw new ArgumentException("Chưa có hóa đơn để thanh toán");
        phieuDal.CheckOut(bill.MaPhieu, bill.SoPhong, bill.NgayTra, bill.TongTien);
    }
}
