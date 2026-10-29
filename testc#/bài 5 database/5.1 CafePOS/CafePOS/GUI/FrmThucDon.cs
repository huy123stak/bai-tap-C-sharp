using System.Data;
using System.Data.SqlClient;
using CafePOS.BUS;
using CafePOS.DTO;

namespace CafePOS.GUI;

public class FrmThucDon : Form
{
    private readonly DanhMucBUS dmBus = new();
    private readonly MonAnBUS monBus = new();
    private readonly DataGridView dgvDM = UI.Grid();
    private readonly DataGridView dgvMon = UI.Grid();
    private readonly TextBox txtTenDM = UI.Txt(110, 10, 220);
    private readonly TextBox txtTenMon = UI.Txt(80, 10, 180);
    private readonly TextBox txtGia = UI.Txt(580, 10, 100);
    private readonly ComboBox cbDM = UI.Cmb(350, 10, 140);

    public FrmThucDon()
    {
        Text = "Quản lý danh mục và thực đơn";
        Size = new Size(860, 560);
        StartPosition = FormStartPosition.CenterParent;

        cbDM.DisplayMember = "TenDanhMuc";
        cbDM.ValueMember = "MaDanhMuc";

        var tabs = new TabControl { Dock = DockStyle.Fill };
        var tabDM = new TabPage("Danh mục");
        var tabMon = new TabPage("Thực đơn");

        var pnlDM = new Panel { Dock = DockStyle.Top, Height = 50 };
        pnlDM.Controls.AddRange(new Control[]
        {
            UI.Lbl("Tên danh mục", 10, 10), txtTenDM,
            UI.Btn("Thêm", 350, 8, 70, BtnThemDM_Click),
            UI.Btn("Sửa", 430, 8, 70, BtnSuaDM_Click),
            UI.Btn("Xóa", 510, 8, 70, BtnXoaDM_Click)
        });
        tabDM.Controls.Add(dgvDM);
        tabDM.Controls.Add(pnlDM);

        var pnlMon = new Panel { Dock = DockStyle.Top, Height = 90 };
        pnlMon.Controls.AddRange(new Control[]
        {
            UI.Lbl("Tên món", 10, 10), txtTenMon,
            UI.Lbl("Danh mục", 280, 10), cbDM,
            UI.Lbl("Đơn giá", 510, 10), txtGia,
            UI.Btn("Thêm", 10, 48, 80, BtnThemMon_Click),
            UI.Btn("Sửa", 100, 48, 80, BtnSuaMon_Click),
            UI.Btn("Xóa", 190, 48, 80, BtnXoaMon_Click)
        });
        tabMon.Controls.Add(dgvMon);
        tabMon.Controls.Add(pnlMon);

        tabs.TabPages.Add(tabDM);
        tabs.TabPages.Add(tabMon);
        Controls.Add(tabs);

        dgvDM.CellClick += (s, e) => ChonDM();
        dgvMon.CellClick += (s, e) => ChonMon();
        Load += (s, e) =>
        {
            LoadDM();
            LoadMon();
        };
    }

    private void LoadDM()
    {
        dgvDM.DataSource = dmBus.GetAll();
        UI.Header(dgvDM, "MaDanhMuc", "Mã");
        UI.Header(dgvDM, "TenDanhMuc", "Tên danh mục");
        cbDM.DataSource = dmBus.GetAll();
        txtTenDM.Clear();
    }

    private void LoadMon()
    {
        dgvMon.DataSource = monBus.GetAll();
        dgvMon.Columns["MaDanhMuc"].Visible = false;
        UI.Header(dgvMon, "MaMon", "Mã");
        UI.Header(dgvMon, "TenMon", "Tên món");
        UI.Header(dgvMon, "TenDanhMuc", "Danh mục");
        UI.Header(dgvMon, "DonGia", "Đơn giá");
        txtTenMon.Clear();
        txtGia.Clear();
    }

    private void ChonDM()
    {
        if (dgvDM.CurrentRow == null) return;
        txtTenDM.Text = dgvDM.CurrentRow.Cells["TenDanhMuc"].Value.ToString();
    }

    private void ChonMon()
    {
        if (dgvMon.CurrentRow == null) return;
        var r = dgvMon.CurrentRow;
        txtTenMon.Text = r.Cells["TenMon"].Value.ToString();
        txtGia.Text = Convert.ToDecimal(r.Cells["DonGia"].Value).ToString("0.##");
        cbDM.SelectedValue = Convert.ToInt32(r.Cells["MaDanhMuc"].Value);
    }

    private void Run(Action action)
    {
        try
        {
            action();
        }
        catch (SqlException)
        {
            MessageBox.Show("Thao tác không thực hiện được do dữ liệu đang được sử dụng hoặc không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private int IdDM()
    {
        return dgvDM.CurrentRow == null ? 0 : Convert.ToInt32(dgvDM.CurrentRow.Cells["MaDanhMuc"].Value);
    }

    private int IdMon()
    {
        return dgvMon.CurrentRow == null ? 0 : Convert.ToInt32(dgvMon.CurrentRow.Cells["MaMon"].Value);
    }

    private MonAnDTO ReadMon(int ma)
    {
        if (!decimal.TryParse(txtGia.Text.Trim(), out decimal gia)) throw new ArgumentException("Đơn giá phải là số");
        return new MonAnDTO
        {
            MaMon = ma,
            TenMon = txtTenMon.Text.Trim(),
            MaDanhMuc = cbDM.SelectedValue is int d ? d : 0,
            DonGia = gia
        };
    }

    private void BtnThemDM_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            dmBus.Them(new DanhMucDTO { TenDanhMuc = txtTenDM.Text.Trim() });
            LoadDM();
        });
    }

    private void BtnSuaDM_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            dmBus.Sua(new DanhMucDTO { MaDanhMuc = IdDM(), TenDanhMuc = txtTenDM.Text.Trim() });
            LoadDM();
            LoadMon();
        });
    }

    private void BtnXoaDM_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (MessageBox.Show("Xóa danh mục đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            dmBus.Xoa(IdDM());
            LoadDM();
        });
    }

    private void BtnThemMon_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            monBus.Them(ReadMon(0));
            LoadMon();
        });
    }

    private void BtnSuaMon_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            monBus.Sua(ReadMon(IdMon()));
            LoadMon();
        });
    }

    private void BtnXoaMon_Click(object sender, EventArgs e)
    {
        Run(() =>
        {
            if (MessageBox.Show("Xóa món đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            monBus.Xoa(IdMon());
            LoadMon();
        });
    }
}
