namespace Bai4_7
{
    public class FormLogin : Form
    {
        private readonly TextBox txtUser = new() { Left = 100, Top = 20, Width = 170, Text = "admin" };
        public string UserName => txtUser.Text.Trim();

        public FormLogin()
        {
            Text = "Đăng nhập";
            ClientSize = new Size(300, 110);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            var btn = new Button { Text = "Đăng nhập", Left = 100, Top = 60, Width = 100 };
            btn.Click += (s, e) =>
            {
                if (UserName.Length == 0) { MessageBox.Show("Nhập tên người dùng."); return; }
                DialogResult = DialogResult.OK;
            };
            Controls.Add(new Label { Text = "Tài khoản", Left = 20, Top = 24, AutoSize = true });
            Controls.Add(txtUser);
            Controls.Add(btn);
            AcceptButton = btn;
        }
    }
}
