namespace HospitalMdi;

public class frmLogin : Form
{
    readonly ComboBox cboDuty = new() { Left = 120, Top = 18, Width = 220, DropDownStyle = ComboBoxStyle.DropDown };

    public string DutyName => cboDuty.Text.Trim();

    public frmLogin()
    {
        Text = "Đăng nhập trực";
        ClientSize = new Size(370, 110);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        foreach (var d in DataStore.Doctors) cboDuty.Items.Add(d.FullName);
        cboDuty.SelectedIndex = 0;

        var btn = new Button { Text = "Đăng nhập", Left = 120, Top = 60, Width = 110 };
        btn.Click += (s, e) =>
        {
            if (DutyName.Length == 0)
            {
                MessageBox.Show("Hãy nhập tên bác sĩ trực.", "Thông báo");
                return;
            }
            DialogResult = DialogResult.OK;
        };

        Controls.Add(new Label { Text = "Bác sĩ trực:", Left = 20, Top = 22, AutoSize = true });
        Controls.Add(cboDuty);
        Controls.Add(btn);
        AcceptButton = btn;
    }
}
