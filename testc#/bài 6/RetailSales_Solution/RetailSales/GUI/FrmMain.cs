using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailSales.GUI
{
    public class FrmMain : Form
    {
        public FrmMain()
        {
            Text = "Hệ thống quản lý bán hàng - Siêu thị mini";
            Width = 460;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 11);

            var title = new Label
            {
                Text = "RETAIL SALES SYSTEM",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Left = 0,
                Top = 20,
                Width = ClientSize.Width,
                Height = 50
            };
            Controls.Add(title);

            Ui.Btn(this, "Bán hàng / Lập hóa đơn", 80, 90, 280, (s, e) => new FrmBanHang().ShowDialog());
            Ui.Btn(this, "Quản lý Sản phẩm", 80, 150, 280, (s, e) => new FrmSanPham().ShowDialog());
            Ui.Btn(this, "Quản lý Khách hàng", 80, 210, 280, (s, e) => new FrmKhachHang().ShowDialog());
            Ui.Btn(this, "Quản lý Nhân viên", 80, 270, 280, (s, e) => new FrmNhanVien().ShowDialog());
            Ui.Btn(this, "Báo cáo - Thống kê", 80, 330, 280, (s, e) => new FrmBaoCao().ShowDialog());
            Ui.Btn(this, "Thoát", 80, 390, 280, (s, e) => Close());

            foreach (Control c in Controls)
            {
                if (c is Button) c.Height = 44;
            }
        }
    }
}
