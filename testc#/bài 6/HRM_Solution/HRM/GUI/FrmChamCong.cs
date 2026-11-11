using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HRM.BUS;

namespace HRM.GUI
{
    public class FrmChamCong : Form
    {
        private readonly NumericUpDown nudThang, nudNam;
        private readonly Label lblTrangThai;
        private readonly DataGridView grid;
        private readonly ChamCongBUS bus = new ChamCongBUS();
        private DataTable dt;

        public FrmChamCong()
        {
            Text = "Chấm công tháng (Disconnected Model)";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Tháng", 20, 24);
            nudThang = new NumericUpDown { Left = 75, Top = 20, Width = 60, Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };
            Ui.Lbl(this, "Năm", 150, 24);
            nudNam = new NumericUpDown { Left = 190, Top = 20, Width = 80, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            Controls.Add(nudThang);
            Controls.Add(nudNam);

            Ui.Btn(this, "Nạp bảng chấm công", 290, 16, 190, (s, e) => Ui.Run(LoadData));
            Ui.Btn(this, "Lưu chấm công", 490, 16, 160, (s, e) => Ui.Run(SaveData));
            lblTrangThai = Ui.Lbl(this, "", 670, 24);
            lblTrangThai.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            grid = Ui.Grid(this, 20, 70, 845, 480);
            grid.ReadOnly = false;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            grid.CellBeginEdit += (s, e) =>
            {
                var drv = grid.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (drv != null && Convert.ToBoolean(drv.Row["DaKhoa"])) e.Cancel = true;
            };
            grid.DataError += (s, e) =>
            {
                e.ThrowException = false;
                Ui.Err("Giá trị nhập không hợp lệ, vui lòng nhập số.");
            };

            Load += (s, e) => Ui.Run(LoadData);
        }

        private void LoadData()
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value;
            dt = bus.Load(thang, nam);
            grid.DataSource = dt;

            grid.Columns["MaCong"].Visible = false;
            grid.Columns["ThangNam"].Visible = false;
            grid.Columns["MaNV"].HeaderText = "Mã NV";
            grid.Columns["HoTen"].HeaderText = "Họ tên";
            grid.Columns["HoTen"].DisplayIndex = 1;
            grid.Columns["SoNgayCong"].HeaderText = "Số ngày công";
            grid.Columns["SoGioOT"].HeaderText = "Số giờ OT";
            grid.Columns["DaKhoa"].HeaderText = "Đã khóa";
            grid.Columns["MaNV"].ReadOnly = true;
            grid.Columns["HoTen"].ReadOnly = true;
            grid.Columns["DaKhoa"].ReadOnly = true;

            bool chot = bus.DaChot(thang, nam);
            lblTrangThai.Text = chot ? "Đã chốt lương - dữ liệu bị khóa" : "Chưa chốt - có thể chỉnh sửa";
            lblTrangThai.ForeColor = chot ? Color.Firebrick : Color.SeaGreen;
        }

        private void SaveData()
        {
            if (dt == null) throw new Exception("Vui lòng nạp bảng chấm công trước.");
            grid.EndEdit();
            BindingContext[dt].EndCurrentEdit();
            int n = bus.Save(dt, (int)nudThang.Value, (int)nudNam.Value);
            Ui.Info("Đã lưu " + n + " dòng chấm công thay đổi.");
        }
    }
}
