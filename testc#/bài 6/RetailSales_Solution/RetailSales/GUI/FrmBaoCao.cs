using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSales.BUS;

namespace RetailSales.GUI
{
    public class FrmBaoCao : Form
    {
        private readonly DateTimePicker dtpNgay;
        private readonly NumericUpDown nudThang, nudNam;
        private readonly Label lblNgay, lblThang;
        private readonly DataGridView grid;
        private readonly BaoCaoBUS bus = new BaoCaoBUS();

        public FrmBaoCao()
        {
            Text = "Báo cáo - Thống kê";
            Width = 860;
            Height = 560;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Doanh thu theo ngày", 20, 22).Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dtpNgay = new DateTimePicker { Left = 20, Top = 50, Width = 160, Format = DateTimePickerFormat.Short };
            Controls.Add(dtpNgay);
            Ui.Btn(this, "Xem", 190, 47, 80, (s, e) => Ui.Run(() =>
            {
                lblNgay.Text = bus.DoanhThuNgay(dtpNgay.Value).ToString("N0") + " VNĐ";
            }));
            lblNgay = Ui.Lbl(this, "", 290, 53);
            lblNgay.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            Ui.Lbl(this, "Doanh thu theo tháng", 20, 100).Font = new Font("Segoe UI", 10, FontStyle.Bold);
            nudThang = new NumericUpDown { Left = 20, Top = 128, Width = 60, Minimum = 1, Maximum = 12, Value = DateTime.Now.Month };
            nudNam = new NumericUpDown { Left = 100, Top = 128, Width = 80, Minimum = 2000, Maximum = 2100, Value = DateTime.Now.Year };
            Controls.Add(nudThang);
            Controls.Add(nudNam);
            Ui.Btn(this, "Xem", 190, 125, 80, (s, e) => Ui.Run(() =>
            {
                lblThang.Text = bus.DoanhThuThang((int)nudThang.Value, (int)nudNam.Value).ToString("N0") + " VNĐ";
            }));
            lblThang = Ui.Lbl(this, "", 290, 131);
            lblThang.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            Ui.Lbl(this, "Top 5 sản phẩm bán chạy nhất", 20, 185).Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Ui.Btn(this, "Làm mới", 250, 179, 100, (s, e) => Ui.Run(LoadTop));
            grid = Ui.Grid(this, 20, 220, 805, 280);

            Load += (s, e) => Ui.Run(() =>
            {
                lblNgay.Text = bus.DoanhThuNgay(dtpNgay.Value).ToString("N0") + " VNĐ";
                lblThang.Text = bus.DoanhThuThang((int)nudThang.Value, (int)nudNam.Value).ToString("N0") + " VNĐ";
                LoadTop();
            });
        }

        private void LoadTop()
        {
            grid.DataSource = bus.Top5BanChay();
        }
    }
}
