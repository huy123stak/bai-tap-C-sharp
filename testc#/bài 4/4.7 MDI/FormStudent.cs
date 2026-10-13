using System.ComponentModel;

namespace Bai4_7
{
    public class FormStudent : ChildFormBase
    {
        private readonly BindingList<Student> _list = new();
        private readonly BindingSource _bs = new();
        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true,
            AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly TextBox txtId = new() { Left = 70, Top = 15, Width = 90 };
        private readonly TextBox txtName = new() { Left = 250, Top = 15, Width = 180 };
        private readonly TextBox txtAge = new() { Left = 70, Top = 50, Width = 90 };
        private readonly ComboBox cboGender = new() { Left = 250, Top = 50, Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };

        public FormStudent()
        {
            Text = "Quản lý Sinh viên";
            Size = new Size(720, 450);

            foreach (var (p, h) in new[] { ("StudentId", "Mã SV"), ("FullName", "Họ tên"), ("Age", "Tuổi"), ("Gender", "Giới tính") })
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = p, HeaderText = h });
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGender.SelectedIndex = 0;

            var top = new Panel { Dock = DockStyle.Top, Height = 95 };
            var btnAdd = new Button { Text = "Thêm", Left = 470, Top = 12, Width = 90 };
            var btnDel = new Button { Text = "Xóa", Left = 470, Top = 48, Width = 90 };
            btnAdd.Click += (s, e) => Add();
            btnDel.Click += (s, e) => RemoveSelected();
            top.Controls.AddRange(new Control[] {
                L("Mã SV", 10, 18), txtId, L("Họ tên", 190, 18), txtName,
                L("Tuổi", 10, 53), txtAge, L("Giới tính", 190, 53), cboGender, btnAdd, btnDel });

            Controls.Add(dgv);
            Controls.Add(top);

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Xóa dòng", null, (s, e) => RemoveSelected());
            ctx.Items.Add("Làm mới dữ liệu", null, (s, e) => RefreshData());
            ctx.Items.Add("Xem chi tiết", null, (s, e) => ShowDetail());
            dgv.ContextMenuStrip = ctx;
            dgv.CellMouseDown += (s, e) =>   
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    dgv.ClearSelection();
                    dgv.Rows[e.RowIndex].Selected = true;
                    dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[0];
                }
            };

            Seed();
            _bs.DataSource = _list;
            dgv.DataSource = _bs;
            Load += (s, e) => ReportStatus($"Xin chào {CurrentUser} - đã mở Quản lý Sinh viên ({_list.Count} bản ghi)");
        }

        private void Seed()
        {
            _list.Clear();
            _list.Add(new Student { StudentId = "SV001", FullName = "Nguyễn Văn An", Age = 20, Gender = "Nam" });
            _list.Add(new Student { StudentId = "SV002", FullName = "Trần Thị Bình", Age = 21, Gender = "Nữ" });
            _list.Add(new Student { StudentId = "SV003", FullName = "Lê Hoàng Cường", Age = 22, Gender = "Nam" });
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtName.Text) || !int.TryParse(txtAge.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Vui lòng nhập Mã SV, Họ tên và Tuổi hợp lệ.", "Thông báo");
                return;
            }
            if (_list.Any(x => x.StudentId == txtId.Text.Trim()))
            {
                MessageBox.Show("Mã SV đã tồn tại.", "Thông báo");
                return;
            }
            _list.Add(new Student { StudentId = txtId.Text.Trim(), FullName = txtName.Text.Trim(), Age = age, Gender = cboGender.Text });
            ReportStatus($"Thêm sinh viên {txtName.Text.Trim()} thành công");
            txtId.Clear(); txtName.Clear(); txtAge.Clear();
        }

        private void RemoveSelected()
        {
            if (_bs.Current is not Student st) return;
            if (MessageBox.Show($"Xóa {st.FullName}?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            _bs.RemoveCurrent();
            ReportStatus($"Đã xóa sinh viên {st.FullName}");
        }

        private void ShowDetail()
        {
            if (_bs.Current is not Student st) return;
            MessageBox.Show($"Mã SV: {st.StudentId}\nHọ tên: {st.FullName}\nTuổi: {st.Age}\nGiới tính: {st.Gender}",
                "Chi tiết sinh viên");
        }

        public override void RefreshData()
        {
            Seed();
            ReportStatus("Đã làm mới dữ liệu Sinh viên");
        }

        private static Label L(string t, int x, int y) => new() { Text = t, Left = x, Top = y, AutoSize = true };
    }
}
