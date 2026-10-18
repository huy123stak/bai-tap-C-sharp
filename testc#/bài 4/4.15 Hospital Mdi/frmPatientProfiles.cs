using System.ComponentModel;
using static HospitalMdi.UiHelper;

namespace HospitalMdi;

public class frmPatientProfiles : ListFormBase
{
    readonly TextBox txtId = new();
    readonly TextBox txtName = new();
    readonly TextBox txtAge = new();
    readonly TextBox txtDiagnosis = new();
    readonly TextBox txtSearch = new() { Width = 220 };
    readonly ComboBox cboDepartment = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    public frmPatientProfiles() : base("Hồ sơ Bệnh nhân")
    {
        AddColumn(nameof(Patient.PatientId), "Mã BN", 10);
        AddColumn(nameof(Patient.FullName), "Họ tên", 24);
        AddColumn(nameof(Patient.Age), "Tuổi", 8);
        AddColumn(nameof(Patient.Diagnosis), "Chẩn đoán", 30);
        AddColumn(nameof(Patient.Department), "Khoa", 22);

        var f = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, Padding = new Padding(6) };
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

        Row(0, "Mã BN:", txtId, "Họ tên:", txtName);
        Row(1, "Tuổi:", txtAge, "Khoa:", cboDepartment);
        f.Controls.Add(Lbl("Chẩn đoán:"), 0, 2);
        txtDiagnosis.Dock = DockStyle.Fill;
        f.Controls.Add(txtDiagnosis, 1, 2);
        f.SetColumnSpan(txtDiagnosis, 3);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        buttons.Controls.Add(Btn("Thêm", (s, e) => AddPatient()));
        buttons.Controls.Add(Btn("Xóa", (s, e) => DeletePatient()));
        buttons.Controls.Add(Lbl("Tìm:"));
        buttons.Controls.Add(txtSearch);
        f.Controls.Add(buttons, 0, 3);
        f.SetColumnSpan(buttons, 4);

        txtSearch.TextChanged += (s, e) => ApplyFilter();
        Controls.Add(f);

        LoadDepartments();
        ApplyFilter();
    }

    void LoadDepartments()
    {
        cboDepartment.Items.Clear();
        foreach (var d in DataStore.Departments) cboDepartment.Items.Add(d.Name);
        if (cboDepartment.Items.Count > 0) cboDepartment.SelectedIndex = 0;
    }

    void ApplyFilter()
    {
        string q = txtSearch.Text.Trim();
        if (q.Length == 0)
        {
            dgv.DataSource = DataStore.Patients;
            return;
        }
        var result = DataStore.Patients.Where(p =>
            p.PatientId.Contains(q, StringComparison.OrdinalIgnoreCase)
            || p.FullName.Contains(q, StringComparison.OrdinalIgnoreCase)
            || p.Diagnosis.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        dgv.DataSource = new BindingList<Patient>(result);
    }

    void AddPatient()
    {
        string id = txtId.Text.Trim();
        if (id.Length == 0 || txtName.Text.Trim().Length == 0)
        {
            MessageBox.Show("Mã BN và Họ tên không được để trống.", "Thông báo");
            return;
        }
        if (!int.TryParse(txtAge.Text.Trim(), out int age) || age < 0 || age > 150)
        {
            MessageBox.Show("Tuổi không hợp lệ.", "Thông báo");
            return;
        }
        if (DataStore.Patients.Any(p => p.PatientId.Equals(id, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã bệnh nhân đã tồn tại.", "Thông báo");
            return;
        }
        DataStore.Patients.Add(new Patient
        {
            PatientId = id,
            FullName = txtName.Text.Trim(),
            Age = age,
            Diagnosis = txtDiagnosis.Text.Trim(),
            Department = cboDepartment.SelectedItem as string
        });
        txtId.Clear();
        txtName.Clear();
        txtAge.Clear();
        txtDiagnosis.Clear();
        ApplyFilter();
    }

    void DeletePatient()
    {
        if (dgv.CurrentRow?.DataBoundItem is not Patient p) return;
        var result = MessageBox.Show($"Xóa hồ sơ {p.FullName}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes) return;
        DataStore.Patients.Remove(p);
        ApplyFilter();
    }

    public override void RefreshData()
    {
        LoadDepartments();
        ApplyFilter();
    }
}
