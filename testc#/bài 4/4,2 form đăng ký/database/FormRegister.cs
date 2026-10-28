using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Bai4_2_StudentRegister
{
    /// <summary>
    /// BAI 4.2 (NANG CAP): Form dang ky hoc vien co validation (ErrorProvider)
    /// va luu vao CSDL MySQL (XAMPP) bang ADO.NET.
    /// - Hop le + email chua ton tai -> INSERT vao bang Students.
    /// - DataGridView hien thi danh sach hoc vien doc tu CSDL.
    /// - Chon 1 dong + nut "Xoa hoc vien" de xoa khoi CSDL.
    /// </summary>
    public class FormRegister : Form
    {
        private TextBox txtHoTen;
        private TextBox txtEmail;
        private TextBox txtTuoi;
        private TextBox txtSoDienThoai;
        private Button btnSubmit;
        private Button btnDelete;
        private DataGridView dgvStudents;

        private ErrorProvider errorProvider1;

        // Lop truy cap CSDL
        private StudentDb _db;

        public FormRegister()
        {
            InitializeComponent();

            try
            {
                _db = new StudentDb();
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Khong the ket noi MySQL. Hay bat MySQL trong XAMPP Control Panel!\n\n" + ex.Message,
                    "Loi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Dang ky hoc vien (MySQL - XAMPP)";
            this.Size = new Size(640, 560);
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
                Location = new Point(150, 195),
                Width = 100
            };
            btnSubmit.Click += btnSubmit_Click;

            btnDelete = new Button
            {
                Text = "Xoa hoc vien",
                Location = new Point(270, 195),
                Width = 100
            };
            btnDelete.Click += btnDelete_Click;

            dgvStudents = new DataGridView
            {
                Location = new Point(20, 245),
                Size = new Size(590, 265),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

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
            this.Controls.Add(btnDelete);
            this.Controls.Add(dgvStudents);
        }

        // ======================================================
        // CSDL: doc danh sach
        // ======================================================
        private void LoadStudents()
        {
            dgvStudents.DataSource = _db.GetAll();

            // Doi ten cot hien thi
            dgvStudents.Columns["Id"].FillWeight = 30;
            dgvStudents.Columns["HoTen"].HeaderText = "Ho ten";
            dgvStudents.Columns["Tuoi"].FillWeight = 30;
            dgvStudents.Columns["SoDienThoai"].HeaderText = "So dien thoai";
            dgvStudents.Columns["CreatedAt"].HeaderText = "Ngay dang ky";
            dgvStudents.Columns["CreatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Buoc 1: XOA cac loi cu truoc khi kiem tra lai tu dau
            errorProvider1.Clear();

            bool isValid = true;
            int tuoi = 0;
            string hoTen = txtHoTen.Text.Trim();
            string email = txtEmail.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();

            // ----- Kiem tra Ho ten -----
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                errorProvider1.SetError(txtHoTen, "Ho ten khong duoc de trong.");
                isValid = false;
            }

            // ----- Kiem tra Email -----
            if (string.IsNullOrWhiteSpace(email))
            {
                errorProvider1.SetError(txtEmail, "Email khong duoc de trong.");
                isValid = false;
            }
            else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
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
            else if (!int.TryParse(txtTuoi.Text.Trim(), out tuoi) || tuoi <= 0 || tuoi > 100)
            {
                errorProvider1.SetError(txtTuoi, "Tuoi phai la so nguyen hop le (1-100).");
                isValid = false;
            }

            // ----- Kiem tra So dien thoai -----
            if (string.IsNullOrWhiteSpace(sdt))
            {
                errorProvider1.SetError(txtSoDienThoai, "So dien thoai khong duoc de trong.");
                isValid = false;
            }
            else if (!Regex.IsMatch(sdt, @"^[0-9]{9,11}$"))
            {
                errorProvider1.SetError(txtSoDienThoai, "So dien thoai phai gom 9-11 chu so.");
                isValid = false;
            }

            if (!isValid) return;

            if (_db == null)
            {
                MessageBox.Show("Chua ket noi duoc CSDL. Hay bat MySQL trong XAMPP roi mo lai ung dung.",
                    "Loi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===== LUU VAO CSDL =====
            try
            {
                // Kiem tra trung email (rang buoc nghiep vu, can truy van CSDL)
                if (_db.EmailExists(email))
                {
                    errorProvider1.SetError(txtEmail, "Email nay da duoc dang ky.");
                    return;
                }

                _db.AddStudent(hoTen, email, tuoi, sdt);

                MessageBox.Show("Dang ky thanh cong!", "Thong bao",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearInputs();
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the luu vao CSDL: " + ex.Message,
                    "Loi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_db == null || dgvStudents.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["Id"].Value);
            string ten = Convert.ToString(dgvStudents.CurrentRow.Cells["HoTen"].Value);

            var confirm = MessageBox.Show($"Xoa hoc vien \"{ten}\"?", "Xac nhan",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _db.DeleteStudent(id);
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the xoa: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtHoTen.Clear();
            txtEmail.Clear();
            txtTuoi.Clear();
            txtSoDienThoai.Clear();
            txtHoTen.Focus();
        }
    }
}
