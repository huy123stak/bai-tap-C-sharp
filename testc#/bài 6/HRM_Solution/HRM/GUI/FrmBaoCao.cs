using System;
using System.Drawing;
using System.Windows.Forms;
using HRM.BUS;

namespace HRM.GUI
{
    public class FrmBaoCao : Form
    {
        private readonly NumericUpDown nudThang, nudNam;
        private readonly Label lblTong;
        private readonly DataGridView gridPB, gridCT;
        private readonly BaoCaoBUS bus = new BaoCaoBUS();

        public FrmBaoCao()
        {
            Text = "Báo cáo - Thống kê lương";
            Width = 900;
            Height = 640;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Tháng", 20, 24);
            nudThang = new NumericUpDown { Left = 75, Top = 20, Width = 60, Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };
            Ui.Lbl(this, "Năm", 150, 24);
            nudNam = new NumericUpDown { Left = 190, Top = 20, Width = 80, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            Controls.Add(nudThang);
            Controls.Add(nudNam);
            Ui.Btn(this, "Xem báo cáo", 290, 16, 140, (s, e) => Ui.Run(LoadData));

            lblTong = Ui.Lbl(this, "", 450, 24);
            lblTong.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            Ui.Lbl(this, "Tổng lương theo phòng ban", 20, 65).Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridPB = Ui.Grid(this, 20, 90, 845, 170);
            Ui.Lbl(this, "Chi tiết bảng lương đã chốt", 20, 275).Font = new Font("Segoe UI", 10, FontStyle.Bold);
            gridCT = Ui.Grid(this, 20, 300, 845, 285);

            Load += (s, e) => Ui.Run(LoadData);
        }

        private void LoadData()
        {
            int thang = (int)nudThang.Value, nam = (int)nudNam.Value;
            lblTong.Text = "Tổng quỹ lương: " + bus.TongLuongThang(thang, nam).ToString("N0") + " VNĐ";
            gridPB.DataSource = bus.LuongTheoPhongBan(thang, nam);
            gridCT.DataSource = bus.ChiTietLuong(thang, nam);
            if (gridPB.Columns.Contains("TongLuong")) gridPB.Columns["TongLuong"].DefaultCellStyle.Format = "N0";
            if (gridCT.Columns.Contains("ThucLinh")) gridCT.Columns["ThucLinh"].DefaultCellStyle.Format = "N0";
        }
    }
}
