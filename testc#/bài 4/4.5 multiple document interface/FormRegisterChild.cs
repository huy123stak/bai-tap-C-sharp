using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Bai4_5_MdiShell
{
    /// <summary>
    /// Form con: Dang ky hoc vien (phien ban rut gon cua Bai 4.2),
    /// duoc mo ben trong FormMainMdi thong qua MdiParent.
    /// </summary>
    public class FormRegisterChild : Form
    {
        private TextBox txtHoTen;
        private TextBox txtEmail;
        private Button btnSubmit;
        private ErrorProvider errorProvider1;

        public FormRegisterChild()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Dang ky hoc vien (Form con)";
            this.Size = new Size(380, 250);

            Label lblHoTen = new Label { Text = "Ho va ten:", Location = new Point(20, 30), AutoSize = true };
            txtHoTen = new TextBox { Location = new Point(130, 27), Width = 200 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 70), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(130, 67), Width = 200 };

            btnSubmit = new Button { Text = "Submit", Location = new Point(130, 110), Width = 100 };
            btnSubmit.Click += btnSubmit_Click;

            errorProvider1 = new ErrorProvider();

            this.Controls.Add(lblHoTen);
            this.Controls.Add(txtHoTen);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnSubmit);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errorProvider1.SetError(txtHoTen, "Ho ten khong duoc de trong.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                !Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorProvider1.SetError(txtEmail, "Email khong hop le.");
                isValid = false;
            }

            if (isValid)
                MessageBox.Show("Dang ky thanh cong!");
        }
    }
}
