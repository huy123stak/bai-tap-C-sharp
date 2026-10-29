using System.Data;
using System.Data.SqlClient;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement.GUI;

public class FrmMain : Form
{
    private readonly VatTuBUS vtBus = new();
    private readonly NhaCungCapBUS nccBus = new();
    private readonly PhieuKhoBUS pkBus = new();

    private readonly DataGridView dgvVT = UI.Grid();
    private readonly TextBox txtMaVT = UI.Txt(90, 10, 120);
    private readonly TextBox txtTenVT = UI.Txt(300, 10, 200);
    private readonly TextBox txtDVT = UI.Txt(570, 10, 80);
    private readonly TextBox txtTon = UI.Txt(740, 10, 80);
    private readonly TextBox txtTimVT = UI.Txt(90, 50, 200);

    private readonly DataGridView dgvNCC = UI.Grid();
    private readonly TextBox txtTenNCC = UI.Txt(90, 10, 220);
    private readonly TextBox txtDT = UI.Txt(400, 10, 120);
    private readonly TextBox txtDiaChi = UI.Txt(610, 10, 220);
    private readonly TextBox txtTimNCC = UI.Txt(90, 50, 200);

    private readonly ComboBox cbLoai = UI.Cmb(90, 12, 100);
    private readonly ComboBox cbNCC = UI.Cmb(320, 12, 220);
    private readonly ComboBox cbVatTu = UI.Cmb(90, 52, 260);
    private readonly NumericUpDown nudSL = new() { Left = 440, Top = 52, Width = 90, DecimalPlaces = 2, Minimum = 0.01m, Maximum = 1000000, Value = 1 };
    private readonly NumericUpDown nudGia = new() { Left = 610, Top = 52, Width = 110, DecimalPlaces = 2, Minimum = 0, Maximum = 1000000000, Value = 0 };
    private readonly DataGridView dgvPhieu = UI.Grid();
    private readonly Label lblTongPhieu = new() { Left = 600, Top = 100, AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), Text = "Tổng tiền: 0" };
    private readonly DataTable dtDong = new();

    private readonly NumericUpDown nudNguong = new() { Left = 130, Top = 12, Width = 90, DecimalPlaces = 2, Minimum = 0, Maximum = 1000000, Value = 20 };
    private readonly DataGridView dgvCanhBao = UI.Grid();
    private readonly DataGridView dgvLichSu = UI.Grid();
    private readonly Label lblGiaTri = new() { Left = 520, Top = 16, AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

    public FrmMain()
    {
        Text = "Quản lý xuất - nhập kho nguyên vật liệu";
        Size = new Size(960, 640);
        StartPosition = FormStartPosition.CenterScreen;

        cbLoai.Items.AddRange(new object[] { "NHAP", "XUAT" });
        cbLoai.SelectedIndex = 0;
        cbLoai.SelectedIndexChanged += (s, e) => cbNCC.Enabled = cbLoai.Text == "NHAP";
        cbNCC.DisplayMember = "TenNCC";
        cbNCC.ValueMember = "MaNCC";
        cbVatTu.DisplayMember = "TenVatTu";
        cbVatTu.ValueMember = "MaVatTu";

        dtDong.Columns.Add("MaVatTu", typeof(string));
        dtDong.Columns.Add("TenVatTu", typeof(string));
        dtDong.Columns.Add("SoLuong", typeof(decimal));
        dtDong.Columns.Add("DonGia", typeof(decimal));
        dtDong.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGia");
        dgvPhieu.DataSource = dtDong;

        var tabs = new TabControl { Dock = DockStyle.Fill };
        var tabVT = new TabPage("Vật tư");
        var tabNCC = new TabPage("Nhà cung cấp");
        var tabPhieu = new TabPage("Lập phiếu kho");
        var tabBC = new TabPage("Cảnh báo & báo cáo");

        var pnlVT = new Panel { Dock = DockStyle.Top, Height = 95 };
        pnlVT.Controls.AddRange(new Control[]
        {
            UI.Lbl("Mã vật tư", 10, 10), txtMaVT,
            UI.Lbl("Tên vật tư", 225, 10), txtTenVT,
            UI.Lbl("ĐVT", 520, 10), txtDVT,
            UI.Lbl("Tồn kho", 670, 10), txtTon,
            UI.Lbl("Tìm kiếm", 10, 50), txtTimVT,
            UI.Btn("Tìm", 300, 48, 60, (s, e) => Run(LoadVT)),
            UI.Btn("Thêm", 380, 48, 70, BtnThemVT_Click),
            UI.Btn("Sửa", 460, 48, 70, BtnSuaVT_Click),
            UI.Btn("Xóa", 540, 48, 70, BtnXoaVT_Click),
            UI.Btn("Làm mới", 620, 48, 80, (s, e) => { txtTimVT.Clear(); Run(LoadVT); })
        });
        tabVT.Controls.Add(dgvVT);
        tabVT.Controls.Add(pnlVT);

        var pnlNCC = new Panel { Dock = DockStyle.Top, Height = 95 };
        pnlNCC.Controls.AddRange(new Control[]
        {
            UI.Lbl("Tên NCC", 10, 10), txtTenNCC,
            UI.Lbl("Điện thoại", 325, 10), txtDT,
            UI.Lbl("Địa chỉ", 540, 10), txtDiaChi,
            UI.Lbl("Tìm kiếm", 10, 50), txtTimNCC,
            UI.Btn("Tìm", 300, 48, 60, (s, e) => Run(LoadNCC)),
            UI.Btn("Thêm", 380, 48, 70, BtnThemNCC_Click),
            UI.Btn("Sửa", 460, 48, 70, BtnSuaNCC_Click),
            UI.Btn("Xóa", 540, 48, 70, BtnXoaNCC_Click),
            UI.Btn("Làm mới", 620, 48, 80, (s, e) => { txtTimNCC.Clear(); Run(LoadNCC); })
        });
        tabNCC.Controls.Add(dgvNCC);
        tabNCC.Controls.Add(pnlNCC);

        var pnlPhieu = new Panel { Dock = DockStyle.Top, Height = 140 };
        pnlPhieu.Controls.AddRange(new Control[]
        {
            UI.Lbl("Loại phiếu", 10, 12), cbLoai,
            UI.Lbl("Nhà cung cấp", 220, 12), cbNCC,
            UI.Lbl("Vật tư", 10, 52), cbVatTu,
            UI.Lbl("Số lượng", 370, 52), nudSL,
            UI.Lbl("Đơn giá", 545, 52), nudGia,
            UI.Btn("Thêm dòng", 740, 50, 90, BtnThemDong_Click),
            UI.Btn("Xóa dòng", 10, 98, 90, BtnXoaDong_Click),
            UI.Btn("Lưu phiếu kho", 110, 98, 130, BtnLuuPhieu_Click),
            lblTongPhieu
        });
        tabPhieu.Controls.Add(dgvPhieu);
        tabPhieu.Controls.Add(pnlPhieu);

        var pnlBC = new Panel { Dock = DockStyle.Top, Height = 55 };
        pnlBC.Controls.AddRange(new Control[]
        {
            UI.Lbl("Ngưỡng an toàn", 10, 12), nudNguong,
            UI.Btn("Lọc cảnh báo", 240, 10, 110, BtnLoc_Click),
            UI.Btn("Tính giá trị tồn", 360, 10, 140, BtnGiaTri_Click),
            lblGiaTri
        });
        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
        split.Panel1.Controls.Add(dgvCanhBao);
        split.Panel2.Controls.Add(dgvLichSu);
        tabBC.Controls.Add(split);
        tabBC.Controls.Add(pnlBC);

        tabs.TabPages.Add(tabVT);
        tabs.TabPages.Add(tabNCC);
        tabs.TabPages.Add(tabPhieu);
        tabs.TabPages.Add(tabBC);
        Controls.Add(tabs);

        dgvVT.CellClick += (s, e) => ChonVT();
        dgvNCC.CellClick += (s, e) => ChonNCC();
        dgvPhieu.DataBindingComplete += (s, e) => CapNhatTongPhieu();
        tabs.SelectedIndexChanged += (s, e) => Run(RefreshAll);
        Load += (s, e) => Run(RefreshAll);
    }

    private void Run(Action action)
    {
        try
        {
            action();
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            MessageBox.Show("Không thể thực hiện vì dữ liệu đang được tham chiếu hoặc vi phạm ràng buộc", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
        {
            MessageBox.Show("Dữ liệu bị trùng (mã hoặc số điện thoại đã tồn tại)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshAll()
    {
        LoadVT();
        LoadNCC();
        cbNCC.DataSource = nccBus.GetAll();
        cbVatTu.DataSource = vtBus.GetAll();
        dgvLichSu.DataSource = pkBus.GetAll();
        UI.Header(dgvLichSu, "MaPhieu", "Mã phiếu");
        UI.Header(dgvLichSu, "LoaiPhieu", "Loại");
        UI.Header(dgvLichSu, "NgayLap", "Ngày lập");
        UI.Header(dgvLichSu, "TenNCC", "Nhà cung cấp");
        UI.Header(dgvLichSu, "TongTien", "Tổng tiền");
    }

    private void LoadVT()
    {
        dgvVT.DataSource = vtBus.Search(txtTimVT.Text);
        UI.Header(dgvVT, "MaVatTu", "Mã vật tư");
        UI.Header(dgvVT, "TenVatTu", "Tên vật tư");
        UI.Header(dgvVT, "DVT", "ĐVT");
        UI.Header(dgvVT, "SoLuongTon", "Tồn kho");
    }

    private void LoadNCC()
    {
        dgvNCC.DataSource = nccBus.Search(txtTimNCC.Text);
        UI.Header(dgvNCC, "MaNCC", "Mã");
        UI.Header(dgvNCC, "TenNCC", "Tên nhà cung cấp");
        UI.Header(dgvNCC, "DienThoai", "Điện thoại");
        UI.Header(dgvNCC, "DiaChi", "Địa chỉ");
    }

    private void ChonVT()
    {
        if (dgvVT.CurrentRow == null) return;
        var r = dgvVT.CurrentRow;
        txtMaVT.Text = r.Cells["MaVatTu"].Value.ToString();
        txtTenVT.Text = r.Cells["TenVatTu"].Value.ToString();
        txtDVT.Text = r.Cells["DVT"].Value.ToString();
        txtTon.Text = Convert.ToDecimal(r.Cells["SoLuongTon"].Value).ToString("0.##");
    }

    private void ChonNCC()
    {
        if (dgvNCC.CurrentRow == null) return;
        var r = dgvNCC.CurrentRow;
        txtTenNCC.Text = r.Cells["TenNCC"].Value.ToString();
        txtDT.Text = r.Cells["DienThoai"].Value.ToString();
        txtDiaChi.Text = r.Cells["DiaChi"].Value.ToString();
    }

    private VatTuDTO ReadVT()
    {
        decimal ton = 0;
        if (!string.IsNullOrWhiteSpace(txtTon.Text) && !decimal.TryParse(txtTon.Text.Trim(), out ton))
            throw new ArgumentException("Số lượng tồn phải là số");
        return new VatTuDTO
        {
            MaVatTu = txtMaVT.Text.Trim(),
            TenVatTu = txtTenVT.Text.Trim(),
            DVT = txtDVT.Text.Trim(),
            SoLuongTon = ton
        };
    }

    private NhaCungCapDTO ReadNCC()
    {
        int ma = dgvNCC.CurrentRow == null ? 0 : Convert.ToInt32(dgvNCC.CurrentRow.Cells["MaNCC"].Value);
        return new NhaCungCapDTO
        {
            MaNCC = ma,
            TenNCC = txtTenNCC.Text.Trim(),
            DienThoai = txtDT.Text.Trim(),
            DiaChi = txtDiaChi.Text.Trim()
        };
    }

    private void BtnThemVT_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            vtBus.Them(ReadVT());
            LoadVT();
        });
    }

    private void BtnSuaVT_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            vtBus.Sua(ReadVT());
            LoadVT();
        });
    }

    private void BtnXoaVT_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (MessageBox.Show("Xóa vật tư " + txtMaVT.Text + "?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            vtBus.Xoa(txtMaVT.Text.Trim());
            LoadVT();
        });
    }

    private void BtnThemNCC_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            nccBus.Them(ReadNCC());
            LoadNCC();
        });
    }

    private void BtnSuaNCC_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            nccBus.Sua(ReadNCC());
            LoadNCC();
        });
    }

    private void BtnXoaNCC_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (MessageBox.Show("Xóa nhà cung cấp đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            nccBus.Xoa(ReadNCC().MaNCC);
            LoadNCC();
        });
    }

    private void CapNhatTongPhieu()
    {
        decimal tong = dtDong.Rows.Cast<DataRow>().Sum(r => Convert.ToDecimal(r["ThanhTien"]));
        lblTongPhieu.Text = "Tổng tiền: " + tong.ToString("N0");
    }

    private void BtnThemDong_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (cbVatTu.SelectedValue == null) throw new ArgumentException("Vui lòng chọn vật tư");
            string ma = cbVatTu.SelectedValue.ToString();
            string ten = ((DataRowView)cbVatTu.SelectedItem)["TenVatTu"].ToString();
            var dong = dtDong.AsEnumerable().FirstOrDefault(r => r["MaVatTu"].ToString() == ma);
            if (dong != null)
            {
                dong["SoLuong"] = Convert.ToDecimal(dong["SoLuong"]) + nudSL.Value;
                dong["DonGia"] = nudGia.Value;
            }
            else
            {
                dtDong.Rows.Add(ma, ten, nudSL.Value, nudGia.Value);
            }
            CapNhatTongPhieu();
        });
    }

    private void BtnXoaDong_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (dgvPhieu.CurrentRow == null) return;
            dtDong.Rows.RemoveAt(dgvPhieu.CurrentRow.Index);
            CapNhatTongPhieu();
        });
    }

    private void BtnLuuPhieu_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            var ds = dtDong.AsEnumerable().Select(r => new ChiTietPhieuKhoDTO
            {
                MaVatTu = r["MaVatTu"].ToString(),
                SoLuong = Convert.ToDecimal(r["SoLuong"]),
                DonGia = Convert.ToDecimal(r["DonGia"])
            }).ToList();
            int? ncc = cbNCC.SelectedValue is int v ? v : null;
            int ma = pkBus.LapPhieu(cbLoai.Text, ncc, ds);
            MessageBox.Show("Đã lưu phiếu kho số " + ma, "Thành công");
            dtDong.Rows.Clear();
            CapNhatTongPhieu();
            RefreshAll();
        });
    }

    private void BtnLoc_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            dgvCanhBao.DataSource = vtBus.GetDuoiDinhMuc(nudNguong.Value);
            UI.Header(dgvCanhBao, "MaVatTu", "Mã vật tư");
            UI.Header(dgvCanhBao, "TenVatTu", "Tên vật tư");
            UI.Header(dgvCanhBao, "DVT", "ĐVT");
            UI.Header(dgvCanhBao, "SoLuongTon", "Tồn kho");
        });
    }

    private void BtnGiaTri_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            lblGiaTri.Text = "Tổng giá trị tồn kho: " + vtBus.GetTongGiaTriTon().ToString("N0") + " VNĐ";
        });
    }
}
