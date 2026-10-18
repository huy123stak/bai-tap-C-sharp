using System.ComponentModel;
using System.Text.RegularExpressions;
using static GymMemberManager.UiHelper;

namespace GymMemberManager;

public class frmGymMemberManager : Form
{
    static readonly string[] Packages = { "Theo tháng", "6 tháng", "1 năm" };

    readonly BindingList<MemberModel> members = new();
    readonly BindingSource bs = new();
    readonly ErrorProvider errorProvider = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    MemberModel selected;

    TextBox txtMemberId, txtFullName, txtPhone, txtFee;
    ComboBox cboPackage;
    DateTimePicker dtpStartDate;
    DataGridView dgvMembers;

    public frmGymMemberManager()
    {
        Text = "Quản lý hội viên phòng Gym";
        MinimumSize = new Size(850, 520);
        Size = new Size(980, 600);
        StartPosition = FormStartPosition.CenterScreen;

        BuildUi();

        members.Add(new MemberModel { MemberId = "HV001", FullName = "Nguyễn Văn An", Phone = "0912345678", Package = Packages[0], StartDate = DateTime.Today.AddDays(-20), Fee = 500000 });
        members.Add(new MemberModel { MemberId = "HV002", FullName = "Trần Thị Bình", Phone = "0987654321", Package = Packages[1], StartDate = DateTime.Today.AddDays(-60), Fee = 2700000 });
        members.Add(new MemberModel { MemberId = "HV003", FullName = "Lê Hoàng Cường", Phone = "0901112233", Package = Packages[2], StartDate = DateTime.Today.AddDays(-5), Fee = 5000000 });

        bs.DataSource = members;
        dgvMembers.DataSource = bs;
        dgvMembers.SelectionChanged += dgvMembers_SelectionChanged;
        Shown += (s, e) => ResetForm();
    }

    void BuildUi()
    {
        txtMemberId = new TextBox { ReadOnly = true };
        txtFullName = new TextBox();
        txtPhone = new TextBox { MaxLength = 10 };
        txtFee = new TextBox();
        cboPackage = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cboPackage.Items.AddRange(Packages);
        dtpStartDate = new DateTimePicker { Format = DateTimePickerFormat.Short };

        var f = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4 };
        f.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        f.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        f.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        f.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        void Row(int r, string l1, Control c1, string l2, Control c2)
        {
            f.Controls.Add(Lbl(l1), 0, r);
            c1.Dock = DockStyle.Fill;
            f.Controls.Add(c1, 1, r);
            f.Controls.Add(Lbl(l2), 2, r);
            c2.Dock = DockStyle.Fill;
            f.Controls.Add(c2, 3, r);
        }

        Row(0, "Mã hội viên:", txtMemberId, "Họ và tên:", txtFullName);
        Row(1, "Số điện thoại:", txtPhone, "Gói tập:", cboPackage);
        Row(2, "Ngày đăng ký:", dtpStartDate, "Phí dịch vụ:", txtFee);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        buttons.Controls.Add(Btn("Thêm mới", (s, e) => AddMember()));
        buttons.Controls.Add(Btn("Sửa thông tin", (s, e) => EditMember(), 110));
        buttons.Controls.Add(Btn("Xóa hội viên", (s, e) => DeleteMember(), 110));
        buttons.Controls.Add(Btn("Làm mới", (s, e) => ResetForm()));
        f.Controls.Add(buttons, 0, 3);
        f.SetColumnSpan(buttons, 4);

        var group = new GroupBox { Text = "Thông tin hội viên", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        group.Controls.Add(f);

        dgvMembers = NewGrid();
        dgvMembers.Columns.Add(Col(nameof(MemberModel.MemberId), "Mã HV", 10));
        dgvMembers.Columns.Add(Col(nameof(MemberModel.FullName), "Tên hội viên", 24));
        dgvMembers.Columns.Add(Col(nameof(MemberModel.Phone), "SĐT", 14));
        dgvMembers.Columns.Add(Col(nameof(MemberModel.Package), "Gói tập", 14));
        dgvMembers.Columns.Add(Col(nameof(MemberModel.StartDate), "Ngày ĐK", 14, "dd/MM/yyyy"));
        var colFee = Col(nameof(MemberModel.Fee), "Phí dịch vụ", 16, "N0");
        colFee.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvMembers.Columns.Add(colFee);

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(group, 0, 0);
        root.Controls.Add(dgvMembers, 0, 1);
        Controls.Add(root);
    }

    void dgvMembers_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvMembers.SelectedRows.Count == 0) return;
        if (dgvMembers.SelectedRows[0].DataBoundItem is not MemberModel m) return;
        selected = m;
        txtMemberId.Text = m.MemberId;
        txtFullName.Text = m.FullName;
        txtPhone.Text = m.Phone;
        cboPackage.SelectedItem = m.Package;
        dtpStartDate.Value = m.StartDate;
        txtFee.Text = m.Fee.ToString();
        errorProvider.Clear();
    }

    string NextId()
    {
        int max = members
            .Select(m => int.TryParse(m.MemberId.Replace("HV", ""), out int n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();
        return $"HV{max + 1:000}";
    }

    bool ValidateInput(out int fee)
    {
        errorProvider.Clear();
        bool ok = true;
        fee = 0;

        string name = txtFullName.Text.Trim();
        if (name.Length == 0)
        {
            errorProvider.SetError(txtFullName, "Tên hội viên không được để trống");
            ok = false;
        }
        else if (!Regex.IsMatch(name, @"^[\p{L}\s]+$"))
        {
            errorProvider.SetError(txtFullName, "Tên hội viên không được chứa ký tự đặc biệt hoặc chữ số");
            ok = false;
        }

        if (!Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{10}$"))
        {
            errorProvider.SetError(txtPhone, "Số điện thoại phải gồm đúng 10 chữ số");
            ok = false;
        }

        if (cboPackage.SelectedIndex < 0)
        {
            errorProvider.SetError(cboPackage, "Hãy chọn gói tập");
            ok = false;
        }

        if (!int.TryParse(txtFee.Text.Trim(), out fee) || fee <= 0)
        {
            errorProvider.SetError(txtFee, "Phí dịch vụ phải là số nguyên dương");
            ok = false;
        }

        return ok;
    }

    void AddMember()
    {
        if (!ValidateInput(out int fee)) return;
        members.Add(new MemberModel
        {
            MemberId = txtMemberId.Text,
            FullName = txtFullName.Text.Trim(),
            Phone = txtPhone.Text.Trim(),
            Package = cboPackage.SelectedItem as string,
            StartDate = dtpStartDate.Value.Date,
            Fee = fee
        });
        ResetForm();
    }

    void EditMember()
    {
        if (selected == null)
        {
            MessageBox.Show("Hãy chọn một hội viên trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ValidateInput(out int fee)) return;
        selected.FullName = txtFullName.Text.Trim();
        selected.Phone = txtPhone.Text.Trim();
        selected.Package = cboPackage.SelectedItem as string;
        selected.StartDate = dtpStartDate.Value.Date;
        selected.Fee = fee;
        MessageBox.Show("Đã cập nhật thông tin hội viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    void DeleteMember()
    {
        if (selected == null)
        {
            MessageBox.Show("Hãy chọn một hội viên trong danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var result = MessageBox.Show($"Bạn có chắc muốn xóa hội viên {selected.FullName}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes) return;
        members.Remove(selected);
        ResetForm();
    }

    void ResetForm()
    {
        selected = null;
        errorProvider.Clear();
        dgvMembers.ClearSelection();
        txtMemberId.Text = NextId();
        txtFullName.Clear();
        txtPhone.Clear();
        txtFee.Clear();
        cboPackage.SelectedIndex = 0;
        dtpStartDate.Value = DateTime.Today;
        txtFullName.Focus();
    }
}
