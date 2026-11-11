using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HRM.BUS;

namespace HRM.GUI
{
    public class FrmTinhLuong : Form
    {
        private readonly NumericUpDown nudThang, nudNam;
        private readonly Label lblTrangThai, lblTong;
        private readonly DataGridView grid;
        private readonly LuongBUS bus = new LuongBUS();
        private readonly ChamCongBUS ccBus = new ChamCongBUS();

        public FrmTinhLuong()
        {
            Text = "Tính lương & Chốt lương";
            Width = 1000;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Tháng", 20, 24);
            nudThang = new NumericUpDown { Left = 75, Top = 20, Width = 60, Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };
            Ui.Lbl(this, "Năm", 150, 24);
            nudNam = new NumericUpDown { Left = 190, Top = 20, Width = 80, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            Controls.Add(nudThang);
            Controls.Add(nudNam);

            Ui.Btn(this, "Tính lương tạm tính", 290, 16, 180, (s, e) => Ui.Run(LoadData));
            Ui.Btn(this, "Chốt lương tháng", 480, 16, 170, (s, e) => Ui.Run(Chot));
            lblTrangThai = Ui.Lbl(this, "", 670, 24);
            lblTrangThai.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            Ui.Lbl(this, "ThucLinh = (LuongCoBan * HeSoLuong / 22) * SoNgayCong + (SoGioOT * 50,000)", 20, 62);
            lblTong = Ui.Lbl(this, "", 20, 90);
            lblTong.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            grid = Ui.Grid(this, 20, 125, 945, 450);

            Load += (s, e) => Ui.Run(LoadData);
        }

        private void LoadData()
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value;
            var dt = bus.TamTinh(thang, nam);
            grid.DataSource = dt;
            foreach (var c in new[] { "LuongCoBan", "ThucLinh" })
                if (grid.Columns.Contains(c)) grid.Columns[c].DefaultCellStyle.Format = "N0";

            object tong = dt.Compute("SUM(ThucLinh)", "");
            lblTong.Text = "Tổng thực lĩnh: " + (tong == DBNull.Value ? 0m : Convert.ToDecimal(tong)).ToString("N0") + " VNĐ  (" + dt.Rows.Count + " nhân viên)";

            bool chot = ccBus.DaChot(thang, nam);
            lblTrangThai.Text = chot ? "Đã chốt lương" : "Chưa chốt lương";
            lblTrangThai.ForeColor = chot ? Color.Firebrick : Color.SeaGreen;
        }

        private void Chot()
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value;
            if (!Ui.Confirm("Chốt lương tháng " + ChamCongBUS.ThangNam(thang, nam) + "? Dữ liệu chấm công sẽ bị khóa.")) return;
            int n = bus.ChotLuong(thang, nam);
            Ui.Info("Đã chốt lương cho " + n + " nhân viên.");
            LoadData();
        }
    }
}
