using System;
using System.Drawing;
using System.Windows.Forms;

namespace HRM.GUI
{
    public class FrmMain : Form
    {
        public FrmMain()
        {
            Text = "Hệ thống quản lý nhân sự & tính lương";
            Width = 460;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 11);

            var title = new Label
            {
                Text = "HRM & PAYROLL SYSTEM",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Left = 0,
                Top = 20,
                Width = ClientSize.Width,
                Height = 50
            };
            Controls.Add(title);

            Ui.Btn(this, "Quản lý Hồ sơ Nhân viên", 80, 90, 280, (s, e) => new FrmNhanVien().ShowDialog());
            Ui.Btn(this, "Chấm công tháng", 80, 150, 280, (s, e) => new FrmChamCong().ShowDialog());
            Ui.Btn(this, "Tính lương & Chốt lương", 80, 210, 280, (s, e) => new FrmTinhLuong().ShowDialog());
            Ui.Btn(this, "Báo cáo - Thống kê", 80, 270, 280, (s, e) => new FrmBaoCao().ShowDialog());
            Ui.Btn(this, "Thoát", 80, 330, 280, (s, e) => Close());

            foreach (Control c in Controls)
            {
                if (c is Button) c.Height = 44;
            }
        }
    }
}
