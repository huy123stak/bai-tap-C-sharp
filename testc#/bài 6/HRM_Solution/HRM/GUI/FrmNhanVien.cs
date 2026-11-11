using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HRM.BUS;
using HRM.DTO;

namespace HRM.GUI
{
    public class FrmNhanVien : Form
    {
        private readonly TextBox txtHoTen, txtCCCD, txtLuong, txtHeSo, txtTim;
        private readonly DateTimePicker dtpNgaySinh;
        private readonly ComboBox cbPB, cbLoc;
        private readonly DataGridView grid;
        private readonly NhanVienBUS bus = new NhanVienBUS();
        private readonly PhongBanBUS pbBus = new PhongBanBUS();
        private int selId;

        public FrmNhanVien()
        {
            Text = "Quản lý Hồ sơ Nhân viên";
            Width = 1000;
            Height = 670;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Họ và tên", 20, 22);
            txtHoTen = Ui.Txt(this, 130, 18, 200);
            Ui.Lbl(this, "Ngày sinh", 20, 57);
            dtpNgaySinh = new DateTimePicker { Left = 130, Top = 53, Width = 200, Format = DateTimePickerFormat.Short };
            Controls.Add(dtpNgaySinh);
            Ui.Lbl(this, "Số CCCD", 20, 92);
            txtCCCD = Ui.Txt(this, 130, 88, 200);

            Ui.Lbl(this, "Lương cơ bản", 350, 22);
            txtLuong = Ui.Txt(this, 470, 18, 180);
            Ui.Lbl(this, "Hệ số lương", 350, 57);
            txtHeSo = Ui.Txt(this, 470, 53, 180);
            Ui.Lbl(this, "Phòng ban", 350, 92);
            cbPB = new ComboBox { Left = 470, Top = 88, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "TenPB", ValueMember = "MaPB" };
            Controls.Add(cbPB);

            Ui.Btn(this, "Thêm", 690, 14, 130, (s, e) => Ui.Run(() =>
            {
                bus.Insert(Read());
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Sửa", 830, 14, 130, (s, e) => Ui.Run(() =>
            {
                bus.Update(Read());
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Xóa", 690, 52, 130, (s, e) => Ui.Run(() =>
            {
                if (selId <= 0) throw new Exception("Vui lòng chọn nhân viên cần xóa.");
                if (!Ui.Confirm("Bạn có chắc muốn xóa nhân viên này?")) return;
                bus.Delete(selId);
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Làm mới", 830, 52, 130, (s, e) =>
            {
                txtTim.Clear();
                cbLoc.SelectedIndex = 0;
                Ui.Run(Reload);
                Clear();
            });
            Ui.Btn(this, "Chuyển phòng ban cho nhân viên đã chọn", 690, 90, 270, (s, e) => Ui.Run(() =>
            {
                bus.ChuyenPhongBan(selId, cbPB.SelectedValue == null ? 0 : Convert.ToInt32(cbPB.SelectedValue));
                Reload();
            }));

            Ui.Lbl(this, "Tìm theo tên / CCCD / phòng ban", 20, 136);
            txtTim = Ui.Txt(this, 260, 132, 200);
            Ui.Lbl(this, "Lọc phòng ban", 480, 136);
            cbLoc = new ComboBox { Left = 590, Top = 132, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "TenPB", ValueMember = "MaPB" };
            Controls.Add(cbLoc);
            Ui.Btn(this, "Tìm kiếm", 790, 128, 170, (s, e) => Ui.Run(Reload));

            grid = Ui.Grid(this, 20, 180, 940, 440);
            grid.SelectionChanged += (s, e) =>
            {
                var r = grid.CurrentRow;
                if (r == null || r.DataBoundItem == null) return;
                selId = Convert.ToInt32(r.Cells["MaNV"].Value);
                txtHoTen.Text = Convert.ToString(r.Cells["HoTen"].Value);
                object ns = r.Cells["NgaySinh"].Value;
                dtpNgaySinh.Value = ns is DateTime ? (DateTime)ns : DateTime.Today;
                txtCCCD.Text = Convert.ToString(r.Cells["CCCD"].Value);
                txtLuong.Text = Convert.ToDecimal(r.Cells["LuongCoBan"].Value).ToString("0.##");
                txtHeSo.Text = Convert.ToString(r.Cells["HeSoLuong"].Value);
                cbPB.SelectedValue = Convert.ToInt32(r.Cells["MaPB"].Value);
            };

            Load += (s, e) => Ui.Run(() =>
            {
                var dt = pbBus.GetAll();
                cbPB.DataSource = dt.Copy();
                var loc = dt.Copy();
                var row = loc.NewRow();
                row["MaPB"] = 0;
                row["TenPB"] = "Tất cả";
                loc.Rows.InsertAt(row, 0);
                cbLoc.DataSource = loc;
                cbLoc.SelectedIndex = 0;
                Reload();
            });
        }

        private void Reload()
        {
            int pb = cbLoc.SelectedValue == null ? 0 : Convert.ToInt32(cbLoc.SelectedValue);
            grid.DataSource = bus.Search(txtTim.Text, pb);
            if (grid.Columns.Contains("MaPB")) grid.Columns["MaPB"].Visible = false;
            if (grid.Columns.Contains("NgaySinh")) grid.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (grid.Columns.Contains("LuongCoBan")) grid.Columns["LuongCoBan"].DefaultCellStyle.Format = "N0";
        }

        private void Clear()
        {
            selId = 0;
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtLuong.Clear();
            txtHeSo.Text = "1.0";
            dtpNgaySinh.Value = DateTime.Today;
        }

        private NhanVienDTO Read()
        {
            decimal luong;
            double heso;
            if (!decimal.TryParse(txtLuong.Text.Trim(), out luong)) throw new Exception("Lương cơ bản không hợp lệ.");
            if (!double.TryParse(txtHeSo.Text.Trim(), out heso)) throw new Exception("Hệ số lương không hợp lệ.");
            return new NhanVienDTO
            {
                MaNV = selId,
                HoTen = txtHoTen.Text,
                NgaySinh = dtpNgaySinh.Value.Date,
                CCCD = txtCCCD.Text,
                MaPB = cbPB.SelectedValue == null ? 0 : Convert.ToInt32(cbPB.SelectedValue),
                LuongCoBan = luong,
                HeSoLuong = heso
            };
        }
    }
}
