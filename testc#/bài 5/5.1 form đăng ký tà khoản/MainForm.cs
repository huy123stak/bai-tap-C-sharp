namespace App;

public class MainForm : Form
{
    private TextBox txtUsername, txtPassword, txtConfirm;
    private DateTimePicker dtpBirth;
    private RadioButton rdoMale, rdoFemale, rdoOther;
    private CheckBox chkTerms;
    private Button btnRegister, btnReset;
    private ErrorProvider epCheck;

    public MainForm()
    {
        Text = "Đăng ký tài khoản";
        ClientSize = new Size(460, 470);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10F);

        epCheck = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

        var grpPersonal = new GroupBox { Text = "Thông tin cá nhân", Location = new Point(15, 15), Size = new Size(430, 170) };
        AddLabel(grpPersonal, "Tên đăng nhập:", 15, 35);
        AddLabel(grpPersonal, "Mật khẩu:", 15, 80);
        AddLabel(grpPersonal, "Xác nhận mật khẩu:", 15, 125);
        txtUsername = new TextBox { Location = new Point(170, 32), Width = 220 };
        txtPassword = new TextBox { Location = new Point(170, 77), Width = 220, UseSystemPasswordChar = true };
        txtConfirm = new TextBox { Location = new Point(170, 122), Width = 220, UseSystemPasswordChar = true };
        grpPersonal.Controls.AddRange(new Control[] { txtUsername, txtPassword, txtConfirm });

        var grpExtra = new GroupBox { Text = "Thông tin bổ sung", Location = new Point(15, 200), Size = new Size(430, 190) };
        AddLabel(grpExtra, "Ngày sinh:", 15, 35);
        AddLabel(grpExtra, "Giới tính:", 15, 80);
        dtpBirth = new DateTimePicker
        {
            Location = new Point(170, 32),
            Width = 220,
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy",
            MaxDate = DateTime.Today,
            Value = new DateTime(2000, 1, 1)
        };
        rdoMale = new RadioButton { Text = "Nam", Location = new Point(170, 78), AutoSize = true, Checked = true };
        rdoFemale = new RadioButton { Text = "Nữ", Location = new Point(240, 78), AutoSize = true };
        rdoOther = new RadioButton { Text = "Khác", Location = new Point(300, 78), AutoSize = true };
        chkTerms = new CheckBox { Text = "Tôi đồng ý với Điều khoản dịch vụ", Location = new Point(15, 130), AutoSize = true };
        grpExtra.Controls.AddRange(new Control[] { dtpBirth, rdoMale, rdoFemale, rdoOther, chkTerms });

        btnRegister = new Button { Text = "Đăng Ký", Location = new Point(110, 410), Size = new Size(110, 38) };
        btnReset = new Button { Text = "Làm Mới", Location = new Point(240, 410), Size = new Size(110, 38) };
        btnRegister.Click += BtnRegister_Click;
        btnReset.Click += BtnReset_Click;

        Controls.AddRange(new Control[] { grpPersonal, grpExtra, btnRegister, btnReset });
        AcceptButton = btnRegister;
    }

    private static void AddLabel(Control parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });
    }

    private static int CalculateAge(DateTime birth)
    {
        var today = DateTime.Today;
        int age = today.Year - birth.Year;
        if (birth.Date > today.AddYears(-age)) age--;
        return age;
    }

    private bool ValidateInput()
    {
        epCheck.Clear();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            epCheck.SetError(txtUsername, "Tên đăng nhập không được để trống");
            valid = false;
        }

        if (string.IsNullOrEmpty(txtPassword.Text))
        {
            epCheck.SetError(txtPassword, "Mật khẩu không được để trống");
            valid = false;
        }

        if (txtConfirm.Text != txtPassword.Text)
        {
            epCheck.SetError(txtConfirm, "Mật khẩu xác nhận không khớp");
            valid = false;
        }

        if (CalculateAge(dtpBirth.Value) < 18)
        {
            epCheck.SetError(dtpBirth, "Bạn phải từ 18 tuổi trở lên");
            valid = false;
        }

        if (!chkTerms.Checked)
        {
            epCheck.SetError(chkTerms, "Bạn phải đồng ý với Điều khoản dịch vụ");
            valid = false;
        }

        return valid;
    }

    private void BtnRegister_Click(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnReset_Click(object sender, EventArgs e)
    {
        epCheck.Clear();
        txtUsername.Clear();
        txtPassword.Clear();
        txtConfirm.Clear();
        dtpBirth.Value = new DateTime(2000, 1, 1);
        rdoMale.Checked = true;
        chkTerms.Checked = false;
        txtUsername.Focus();
    }
}
