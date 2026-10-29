using System.Data;
using CafePOS.BUS;
using CafePOS.DTO;

namespace CafePOS.GUI;

public class FrmMain : Form
{
    private readonly BanAnBUS banBus = new();
    private readonly DanhMucBUS dmBus = new();
    private readonly MonAnBUS monBus = new();
    private readonly HoaDonBUS hdBus = new();

    private readonly FlowLayoutPanel flpBan = new() { Dock = DockStyle.Left, Width = 440, AutoScroll = true, Padding = new Padding(8) };
    private readonly DataGridView dgv = UI.Grid();
    private readonly ComboBox cbDanhMuc = UI.Cmb(110, 12, 160);
    private readonly ComboBox cbMon = UI.Cmb(110, 48, 160);
    private readonly ComboBox cbBanMoi = UI.Cmb(110, 92, 160);
    private readonly NumericUpDown nudSL = new() { Left = 350, Top = 48, Width = 60, Minimum = 1, Maximum = 100, Value = 1 };
    private readonly Label lblBan = new() { Dock = DockStyle.Top, Height = 36, Text = "Chọn một bàn để gọi món", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
    private readonly Label lblTong = new() { Left = 290, Top = 16, AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold), Text = "Tổng tiền: 0" };
    private int maBanChon;
    private string tenBanChon;

    public FrmMain()
    {
        Text = "Quản lý quán cafe - POS";
        Size = new Size(1180, 680);
        StartPosition = FormStartPosition.CenterScreen;

        cbDanhMuc.DisplayMember = "TenDanhMuc";
        cbDanhMuc.ValueMember = "MaDanhMuc";
        cbMon.DisplayMember = "TenMon";
        cbMon.ValueMember = "MaMon";
        cbBanMoi.DisplayMember = "TenBan";
        cbBanMoi.ValueMember = "MaBan";
        cbDanhMuc.SelectedIndexChanged += (s, e) => LoadMon();

        var right = new Panel { Dock = DockStyle.Fill };
        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 140 };
        right.Controls.Add(dgv);
        right.Controls.Add(lblBan);
        right.Controls.Add(bottom);

        bottom.Controls.AddRange(new Control[]
        {
            UI.Lbl("Danh mục", 10, 12), cbDanhMuc,
            UI.Lbl("Món", 10, 48), cbMon,
            UI.Lbl("Số lượng", 280, 48), nudSL,
            UI.Btn("Thêm món", 430, 46, 100, BtnThem_Click),
            UI.Lbl("Đổi sang", 10, 92), cbBanMoi,
            UI.Btn("Đổi bàn", 280, 90, 90, BtnDoiBan_Click),
            UI.Btn("Thanh toán", 560, 90, 110, BtnThanhToan_Click),
            UI.Btn("Quản lý thực đơn", 530, 10, 140, BtnThucDon_Click),
            lblTong
        });

        Controls.Add(right);
        Controls.Add(flpBan);

        Load += (s, e) =>
        {
            LoadBan();
            LoadDanhMuc();
        };
    }

    private void LoadBan()
    {
        var ds = banBus.GetAll();
        flpBan.Controls.Clear();
        foreach (var b in ds)
        {
            var btn = new Button
            {
                Width = 125,
                Height = 85,
                Margin = new Padding(6),
                Text = b.TenBan + Environment.NewLine + b.TrangThai,
                BackColor = b.TrangThai == "Trống" ? Color.LightGreen : Color.IndianRed,
                FlatStyle = FlatStyle.Flat,
                Tag = b
            };
            if (b.MaBan == maBanChon)
            {
                btn.FlatAppearance.BorderSize = 3;
                btn.FlatAppearance.BorderColor = Color.Black;
            }
            btn.Click += BanClick;
            flpBan.Controls.Add(btn);
        }
        cbBanMoi.DataSource = ds.Where(x => x.TrangThai == "Trống" && x.MaBan != maBanChon).ToList();
    }

    private void BanClick(object sender, EventArgs e)
    {
        var ban = (BanAnDTO)((Button)sender).Tag;
        maBanChon = ban.MaBan;
        tenBanChon = ban.TenBan;
        lblBan.Text = ban.TenBan + " - " + ban.TrangThai;
        LoadBan();
        LoadChiTiet();
    }

    private void LoadChiTiet()
    {
        var dt = hdBus.GetChiTietByBan(maBanChon);
        dgv.DataSource = dt;
        UI.Header(dgv, "MaMon", "Mã món");
        UI.Header(dgv, "TenMon", "Tên món");
        UI.Header(dgv, "SoLuong", "Số lượng");
        UI.Header(dgv, "DonGia", "Đơn giá");
        UI.Header(dgv, "ThanhTien", "Thành tiền");
        decimal tong = dt.Rows.Cast<DataRow>().Sum(r => Convert.ToDecimal(r["ThanhTien"]));
        lblTong.Text = "Tổng tiền: " + tong.ToString("N0");
    }

    private void LoadDanhMuc()
    {
        cbDanhMuc.DataSource = dmBus.GetAll();
        LoadMon();
    }

    private void LoadMon()
    {
        if (cbDanhMuc.SelectedValue is int ma) cbMon.DataSource = monBus.GetByDanhMuc(ma);
    }

    private void BtnThem_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbMon.SelectedValue is not int maMon) throw new ArgumentException("Vui lòng chọn món");
            hdBus.ThemMon(maBanChon, maMon, (int)nudSL.Value);
            LoadBan();
            LoadChiTiet();
            lblBan.Text = tenBanChon + " - Có khách";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnThanhToan_Click(object sender, EventArgs e)
    {
        try
        {
            if (maBanChon <= 0) throw new ArgumentException("Vui lòng chọn bàn");
            if (MessageBox.Show("Xác nhận thanh toán " + tenBanChon + "?", "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            decimal tong = hdBus.ThanhToan(maBanChon);
            MessageBox.Show("Đã thanh toán " + tong.ToString("N0") + " VNĐ", "Thành công");
            LoadBan();
            LoadChiTiet();
            lblBan.Text = tenBanChon + " - Trống";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi giao dịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDoiBan_Click(object sender, EventArgs e)
    {
        try
        {
            if (cbBanMoi.SelectedValue is not int banMoi) throw new ArgumentException("Không có bàn trống để chuyển");
            hdBus.DoiBan(maBanChon, banMoi);
            maBanChon = banMoi;
            var ban = banBus.GetAll().First(x => x.MaBan == banMoi);
            tenBanChon = ban.TenBan;
            lblBan.Text = ban.TenBan + " - " + ban.TrangThai;
            LoadBan();
            LoadChiTiet();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnThucDon_Click(object sender, EventArgs e)
    {
        using var f = new FrmThucDon();
        f.ShowDialog(this);
        LoadDanhMuc();
    }
}
