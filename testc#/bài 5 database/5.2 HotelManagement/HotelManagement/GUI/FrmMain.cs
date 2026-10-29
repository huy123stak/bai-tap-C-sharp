using System.Data;
using HotelManagement.BUS;
using HotelManagement.DTO;

namespace HotelManagement.GUI;

public class FrmMain : Form
{
    private readonly PhongBUS phongBus = new();
    private readonly KhachHangBUS khBus = new();
    private readonly DichVuBUS dvBus = new();
    private readonly PhieuBUS phieuBus = new();

    private readonly DataGridView dgvPhong = UI.Grid();
    private readonly ComboBox cbLoai = UI.Cmb(90, 12, 140);
    private readonly ComboBox cbTrangThai = UI.Cmb(340, 12, 140);
    private DataView dvPhong;
    private bool loading;

    private readonly TextBox txtCCCD = UI.Txt(110, 20, 200);
    private readonly TextBox txtHoTen = UI.Txt(110, 60, 200);
    private readonly TextBox txtSDT = UI.Txt(110, 100, 200);
    private readonly ComboBox cbPhongTrong = UI.Cmb(110, 140, 200);
    private readonly Label lblKhach = new() { Left = 450, Top = 26, AutoSize = true };

    private readonly ComboBox cbPhongDV = UI.Cmb(110, 12, 140);
    private readonly ComboBox cbDichVu = UI.Cmb(340, 12, 160);
    private readonly NumericUpDown nudSL = new() { Left = 600, Top = 12, Width = 60, Minimum = 1, Maximum = 100, Value = 1 };
    private readonly DataGridView dgvDV = UI.Grid();

    private readonly ComboBox cbPhongOut = UI.Cmb(110, 20, 160);
    private readonly Label lblBill = new() { Left = 20, Top = 70, Width = 600, Height = 220, Font = new Font("Consolas", 11) };
    private BillDTO billHienTai;

    public FrmMain()
    {
        Text = "Quản lý khách sạn";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;

        cbPhongTrong.DisplayMember = "SoPhong";
        cbPhongTrong.ValueMember = "SoPhong";
        cbPhongDV.DisplayMember = "SoPhong";
        cbPhongDV.ValueMember = "SoPhong";
        cbPhongOut.DisplayMember = "SoPhong";
        cbPhongOut.ValueMember = "SoPhong";
        cbDichVu.DisplayMember = "TenDV";
        cbDichVu.ValueMember = "MaDV";

        var tabs = new TabControl { Dock = DockStyle.Fill };
        var tabPhong = new TabPage("Danh sách phòng");
        var tabIn = new TabPage("Check-in");
        var tabDV = new TabPage("Gọi dịch vụ");
        var tabOut = new TabPage("Check-out");

        var pnlLoc = new Panel { Dock = DockStyle.Top, Height = 90 };
        pnlLoc.Controls.AddRange(new Control[]
        {
            UI.Lbl("Loại phòng", 10, 12), cbLoai,
            UI.Lbl("Trạng thái", 260, 12), cbTrangThai,
            UI.Btn("Bỏ lọc", 520, 10, 80, (s, e) => { cbLoai.SelectedIndex = 0; cbTrangThai.SelectedIndex = 0; }),
            UI.Btn("Dọn xong (chuyển Trống)", 10, 50, 200, BtnDonXong_Click)
        });
        tabPhong.Controls.Add(dgvPhong);
        tabPhong.Controls.Add(pnlLoc);

        tabIn.Controls.AddRange(new Control[]
        {
            UI.Lbl("CCCD", 10, 20), txtCCCD,
            UI.Btn("Tra cứu", 320, 18, 80, BtnTraCuu_Click),
            UI.Lbl("Họ tên", 10, 60), txtHoTen,
            UI.Lbl("Số điện thoại", 10, 100), txtSDT,
            UI.Lbl("Phòng trống", 10, 140), cbPhongTrong,
            UI.Btn("Nhận phòng", 110, 190, 120, BtnCheckIn_Click),
            lblKhach
        });

        var pnlDV = new Panel { Dock = DockStyle.Top, Height = 60 };
        pnlDV.Controls.AddRange(new Control[]
        {
            UI.Lbl("Phòng đang ở", 10, 12), cbPhongDV,
            UI.Lbl("Dịch vụ", 280, 12), cbDichVu,
            UI.Lbl("SL", 530, 12), nudSL,
            UI.Btn("Thêm dịch vụ", 690, 10, 120, BtnThemDV_Click)
        });
        tabDV.Controls.Add(dgvDV);
        tabDV.Controls.Add(pnlDV);

        tabOut.Controls.AddRange(new Control[]
        {
            UI.Lbl("Phòng đang ở", 10, 20), cbPhongOut,
            UI.Btn("Tính tiền", 290, 18, 100, BtnTinh_Click),
            UI.Btn("Xác nhận thanh toán", 400, 18, 170, BtnThanhToan_Click),
            lblBill
        });

        tabs.TabPages.Add(tabPhong);
        tabs.TabPages.Add(tabIn);
        tabs.TabPages.Add(tabDV);
        tabs.TabPages.Add(tabOut);
        Controls.Add(tabs);

        cbLoai.SelectedIndexChanged += (s, e) => ApplyFilter();
        cbTrangThai.SelectedIndexChanged += (s, e) => ApplyFilter();
        cbPhongDV.SelectedIndexChanged += (s, e) => LoadDichVuDaDung();
        tabs.SelectedIndexChanged += (s, e) => RefreshAll();
        Load += (s, e) => RefreshAll();
    }

    private void RefreshAll()
    {
        loading = true;
        var dt = phongBus.GetAll();
        dvPhong = new DataView(dt);
        dgvPhong.DataSource = dvPhong;
        UI.Header(dgvPhong, "SoPhong", "Số phòng");
        UI.Header(dgvPhong, "LoaiPhong", "Loại phòng");
        UI.Header(dgvPhong, "GiaTheoGio", "Giá theo giờ");
        UI.Header(dgvPhong, "TrangThai", "Trạng thái");

        string loai = cbLoai.Items.Count > 0 ? cbLoai.Text : "";
        string tt = cbTrangThai.Items.Count > 0 ? cbTrangThai.Text : "";
        cbLoai.Items.Clear();
        cbLoai.Items.Add("Tất cả");
        foreach (var v in dt.AsEnumerable().Select(r => r["LoaiPhong"].ToString()).Distinct().OrderBy(x => x))
            cbLoai.Items.Add(v);
        cbTrangThai.Items.Clear();
        cbTrangThai.Items.Add("Tất cả");
        foreach (var v in new[] { "Trống", "Đang ở", "Đang dọn dẹp" })
            cbTrangThai.Items.Add(v);
        int iL = cbLoai.Items.IndexOf(loai);
        int iT = cbTrangThai.Items.IndexOf(tt);
        cbLoai.SelectedIndex = iL < 0 ? 0 : iL;
        cbTrangThai.SelectedIndex = iT < 0 ? 0 : iT;
        loading = false;
        ApplyFilter();

        cbPhongTrong.DataSource = phongBus.GetTrong();
        cbPhongDV.DataSource = phongBus.GetDangO();
        cbPhongOut.DataSource = phongBus.GetDangO();
        cbDichVu.DataSource = dvBus.GetAll();
        LoadDichVuDaDung();
    }

    private static string Esc(string s)
    {
        return s.Replace("'", "''");
    }

    private void ApplyFilter()
    {
        if (loading || dvPhong == null) return;
        var parts = new List<string>();
        if (cbLoai.SelectedIndex > 0) parts.Add("LoaiPhong = '" + Esc(cbLoai.Text) + "'");
        if (cbTrangThai.SelectedIndex > 0) parts.Add("TrangThai = '" + Esc(cbTrangThai.Text) + "'");
        dvPhong.RowFilter = string.Join(" AND ", parts);
    }

    private void Run(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnDonXong_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (dgvPhong.CurrentRow == null) throw new ArgumentException("Vui lòng chọn phòng");
            string so = dgvPhong.CurrentRow.Cells["SoPhong"].Value.ToString();
            string tt = dgvPhong.CurrentRow.Cells["TrangThai"].Value.ToString();
            if (tt != "Đang dọn dẹp") throw new InvalidOperationException("Chỉ phòng đang dọn dẹp mới chuyển về Trống");
            phongBus.DonXong(so);
            RefreshAll();
        });
    }

    private void BtnTraCuu_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            var kh = khBus.TraCuuCCCD(txtCCCD.Text);
            if (kh == null)
            {
                txtHoTen.Clear();
                txtSDT.Clear();
                lblKhach.Text = "Khách mới - vui lòng nhập thông tin";
                lblKhach.ForeColor = Color.DarkOrange;
            }
            else
            {
                txtHoTen.Text = kh.HoTen;
                txtSDT.Text = kh.SoDienThoai;
                lblKhach.Text = "Khách cũ - đã có trong hệ thống";
                lblKhach.ForeColor = Color.Green;
            }
        });
    }

    private void BtnCheckIn_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            var kh = new KhachHangDTO
            {
                CCCD = txtCCCD.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                SoDienThoai = txtSDT.Text.Trim()
            };
            string so = cbPhongTrong.SelectedValue == null ? "" : cbPhongTrong.SelectedValue.ToString();
            int ma = phieuBus.CheckIn(so, kh);
            MessageBox.Show("Nhận phòng thành công. Mã phiếu: " + ma, "Check-in");
            txtCCCD.Clear();
            txtHoTen.Clear();
            txtSDT.Clear();
            lblKhach.Text = "";
            RefreshAll();
        });
    }

    private void LoadDichVuDaDung()
    {
        if (cbPhongDV.SelectedValue == null)
        {
            dgvDV.DataSource = null;
            return;
        }
        dgvDV.DataSource = phieuBus.GetDichVuDaDung(cbPhongDV.SelectedValue.ToString());
        UI.Header(dgvDV, "TenDV", "Dịch vụ");
        UI.Header(dgvDV, "SoLuong", "Số lượng");
        UI.Header(dgvDV, "DonGia", "Đơn giá");
        UI.Header(dgvDV, "ThanhTien", "Thành tiền");
        UI.Header(dgvDV, "ThoiGian", "Thời gian");
    }

    private void BtnThemDV_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (cbPhongDV.SelectedValue == null) throw new ArgumentException("Không có phòng nào đang ở");
            if (cbDichVu.SelectedValue is not int maDV) throw new ArgumentException("Vui lòng chọn dịch vụ");
            phieuBus.ThemDichVu(cbPhongDV.SelectedValue.ToString(), maDV, (int)nudSL.Value);
            LoadDichVuDaDung();
        });
    }

    private void BtnTinh_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (cbPhongOut.SelectedValue == null) throw new ArgumentException("Không có phòng nào đang ở");
            billHienTai = phieuBus.TinhHoaDon(cbPhongOut.SelectedValue.ToString());
            var b = billHienTai;
            lblBill.Text =
                "HÓA ĐƠN PHÒNG " + b.SoPhong + Environment.NewLine +
                "Khách hàng   : " + b.HoTen + Environment.NewLine +
                "Nhận phòng   : " + b.NgayNhan.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine +
                "Trả phòng    : " + b.NgayTra.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine +
                "Số giờ       : " + b.SoGio + " x " + b.GiaTheoGio.ToString("N0") + Environment.NewLine +
                "Tiền phòng   : " + b.TienPhong.ToString("N0") + Environment.NewLine +
                "Tiền dịch vụ : " + b.TienDichVu.ToString("N0") + Environment.NewLine +
                "TỔNG CỘNG    : " + b.TongTien.ToString("N0") + " VNĐ";
        });
    }

    private void BtnThanhToan_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (billHienTai == null) throw new InvalidOperationException("Vui lòng bấm Tính tiền trước");
            if (MessageBox.Show("Xác nhận thanh toán " + billHienTai.TongTien.ToString("N0") + " VNĐ?", "Check-out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            phieuBus.CheckOut(billHienTai);
            billHienTai = null;
            lblBill.Text = "Thanh toán thành công. Phòng chuyển sang trạng thái Đang dọn dẹp.";
            RefreshAll();
        });
    }
}
