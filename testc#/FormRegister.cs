using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Bai4_2_StudentRegister
{
    /// <summary>
    /// BAI 4.2: Form dang ky hoc vien co kiem tra du lieu dau vao (validation)
    /// va hien thi canh bao truc quan bang ErrorProvider.
    /// </summary>
    public class FormRegister : Form
    {
        private TextBox txtHoTen;
        private TextBox txtEmail;
        private TextBox txtTuoi;
        private TextBox txtSoDienThoai;
        private Button btnSubmit;

        // ErrorProvider duoc keo tha vao Form (o day tao bang code)
        private ErrorProvider errorProvider1;

        public FormRegister()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Dang ky hoc vien";
            this.Size = new Size(420, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblHoTen = new Label { Text = "Ho va ten:", Location = new Point(20, 30), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(150, 27), Width = 220 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 70), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(150, 67), Width = 220 };

            Label lblTuoi = new Label { Text = "Tuoi:", Location = new Point(20, 110), AutoSize = true };
            txtTuoi = new TextBox { Location = new Point(150, 107), Width = 220 };

            Label lblSdt = new Label { Text = "So dien thoai:", Location = new Point(20, 150), AutoSize = true };
            txtSoDienThoai = new TextBox { Location = new Point(150, 147), Width = 220 };

            btnSubmit = new Button
            {
                Text = "Submit",
                Location = new Point(150, 200),
                Width = 100
            };
            btnSubmit.Click += btnSubmit_Click;

            errorProvider1 = new ErrorProvider
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };

            this.Controls.Add(lblHoTen);
            this.Controls.Add(txtHoTen);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblTuoi);
            this.Controls.Add(txtTuoi);
            this.Controls.Add(lblSdt);
            this.Controls.Add(txtSoDienThoai);
            this.Controls.Add(btnSubmit);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Buoc 1: XOA cac loi cu truoc khi kiem tra lai tu dau
            errorProvider1.Clear();

            bool isValid = true;

            // ----- Kiem tra Ho ten -----
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errorProvider1.SetError(txtHoTen, "Ho ten khong duoc de trong.");
                isValid = false;
            }

            // ----- Kiem tra Email -----
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Email khong duoc de trong.");
                isValid = false;
            }
            else if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorProvider1.SetError(txtEmail, "Dinh dang email khong hop le (vd: abc@gmail.com).");
                isValid = false;
            }

            // ----- Kiem tra Tuoi -----
            if (string.IsNullOrWhiteSpace(txtTuoi.Text))
            {
                errorProvider1.SetError(txtTuoi, "Tuoi khong duoc de trong.");
                isValid = false;
            }
            else if (!int.TryParse(txtTuoi.Text, out int tuoi) || tuoi <= 0 || tuoi > 100)
            {
                errorProvider1.SetError(txtTuoi, "Tuoi phai la so nguyen hop le (1-100).");
                isValid = false;
            }

            // ----- Kiem tra So dien thoai -----
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                errorProvider1.SetError(txtSoDienThoai, "So dien thoai khong duoc de trong.");
                isValid = false;
            }
            else if (!Regex.IsMatch(txtSoDienThoai.Text, @"^[0-9]{9,11}$"))
            {
                errorProvider1.SetError(txtSoDienThoai, "So dien thoai phai gom 9-11 chu so.");
                isValid = false;
            }

            if (isValid)
            {
                MessageBox.Show("Dang ky thanh cong!", "Thong bao",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
