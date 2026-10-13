using System.ComponentModel;
using System.Text;

namespace Bai4_6
{
    public class FormStudent : Form
    {
        private readonly BindingList<Student> _list = new();
        private readonly BindingSource _bs = new();

        private DataGridView dgv;
        private TextBox txtId, txtName, txtAge;
        private ComboBox cboGender;
        private PictureBox pic;
        private Button btnPick, btnAdd, btnDelete, btnUpdate, btnExport;
        private Panel pnlInput;
        private string _loadedPath;

        public FormStudent()
        {
            Text = "Bài 4.6 - Quản lý Sinh viên (BindingSource, Avatar, CSV)";
            Size = new Size(1000, 560);
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            SeedData();

            _bs.DataSource = _list;           
            dgv.DataSource = _bs;             
            BindControls();                   

            _bs.CurrentItemChanged += (s, e) => LoadAvatar();
            _bs.ListChanged += (s, e) => pnlInput.Enabled = _bs.Count > 0;
            LoadAvatar();
        }

        private void BuildUi()
        {
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.Columns.Add(Col("StudentId", "Mã SV"));
            dgv.Columns.Add(Col("FullName", "Họ tên"));
            dgv.Columns.Add(Col("Age", "Tuổi"));
            dgv.Columns.Add(Col("Gender", "Giới tính"));
            dgv.Columns.Add(Col("AvatarPath", "Đường dẫn ảnh"));

            var flow = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            btnAdd = Btn("Thêm", (s, e) => AddStudent());
            btnDelete = Btn("Xóa", (s, e) => DeleteStudent());
            btnUpdate = Btn("Cập nhật", (s, e) => UpdateStudent());
            btnExport = Btn("Xuất file CSV", (s, e) => ExportCsv());
            flow.Controls.AddRange(new Control[] { btnAdd, btnDelete, btnUpdate, btnExport });

            pnlInput = new Panel { Dock = DockStyle.Right, Width = 340 };
            pic = new PictureBox { Left = 95, Top = 15, Width = 150, Height = 150, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            btnPick = new Button { Left = 95, Top = 172, Width = 150, Text = "Chọn ảnh..." };
            btnPick.Click += (s, e) => PickAvatar();

            txtId = new TextBox { Left = 100, Top = 215, Width = 220 };
            txtName = new TextBox { Left = 100, Top = 255, Width = 220 };
            txtAge = new TextBox { Left = 100, Top = 295, Width = 220 };
            cboGender = new ComboBox { Left = 100, Top = 335, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ" });

            pnlInput.Controls.AddRange(new Control[]
            {
                pic, btnPick,
                Lbl("Mã SV", 10, 218), txtId,
                Lbl("Họ tên", 10, 258), txtName,
                Lbl("Tuổi", 10, 298), txtAge,
                Lbl("Giới tính", 10, 338), cboGender
            });

            // Thứ tự add: control Fill trước, các control Dock còn lại sau
            Controls.Add(dgv);
            Controls.Add(pnlInput);
            Controls.Add(flow);
        }

        private void BindControls()
        {
            // Binding 2 chiều: control <-> thuộc tính của Student đang chọn trong BindingSource
            txtId.DataBindings.Add("Text", _bs, nameof(Student.StudentId), true, DataSourceUpdateMode.OnPropertyChanged);
            txtName.DataBindings.Add("Text", _bs, nameof(Student.FullName), true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", _bs, nameof(Student.Age), true, DataSourceUpdateMode.OnPropertyChanged);
            cboGender.DataBindings.Add("SelectedItem", _bs, nameof(Student.Gender), true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void SeedData()
        {
            _list.Add(new Student { StudentId = "SV001", FullName = "Nguyễn Văn An", Age = 20, Gender = "Nam" });
            _list.Add(new Student { StudentId = "SV002", FullName = "Trần Thị Bình", Age = 21, Gender = "Nữ" });
            _list.Add(new Student { StudentId = "SV003", FullName = "Lê Hoàng Cường", Age = 22, Gender = "Nam" });
        }

        private void AddStudent()
        {
            var st = new Student
            {
                StudentId = $"SV{_list.Count + 1:000}",
                FullName = "Sinh viên mới",
                Age = 18,
                Gender = "Nam"
            };
            _list.Add(st);
            _bs.MoveLast();
            txtName.Focus();
            txtName.SelectAll();
        }

        private void DeleteStudent()
        {
            if (_bs.Current is not Student st) return;
            if (MessageBox.Show($"Xóa sinh viên {st.FullName}?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                _bs.RemoveCurrent();
        }

        private void UpdateStudent()
        {
            if (_bs.Current is not Student st) return;
            _bs.EndEdit();
            if (string.IsNullOrWhiteSpace(st.StudentId) || string.IsNullOrWhiteSpace(st.FullName))
            {
                MessageBox.Show("Mã SV và Họ tên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (st.Age <= 0 || st.Age > 120)
            {
                MessageBox.Show("Tuổi không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_list.Any(x => !ReferenceEquals(x, st) && x.StudentId == st.StudentId))
            {
                MessageBox.Show("Mã SV bị trùng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dgv.Refresh();
            MessageBox.Show("Đã cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PickAvatar()
        {
            if (_bs.Current is not Student st) return;
            using var ofd = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện",
                Filter = "Hình ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            st.AvatarPath = ofd.FileName;
            LoadAvatar();
        }

        private void LoadAvatar()
        {
            string path = (_bs.Current as Student)?.AvatarPath;
            if (path == _loadedPath && pic.Image != null) return;
            _loadedPath = path;

            pic.Image?.Dispose();
            pic.Image = null;
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try { pic.Image = Image.FromStream(new MemoryStream(File.ReadAllBytes(path))); }
                catch {  }
            }
        }

        private void ExportCsv()
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Xuất danh sách sinh viên",
                Filter = "CSV (*.csv)|*.csv",
                FileName = "DanhSachSinhVien.csv"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                const char sep = ',';
                var sb = new StringBuilder();
                sb.AppendLine(string.Join(sep, "StudentId", "FullName", "Age", "Gender", "AvatarPath"));
                foreach (var s in _list)
                    sb.AppendLine(string.Join(sep, Esc(s.StudentId), Esc(s.FullName), s.Age, Esc(s.Gender), Esc(s.AvatarPath)));

                File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Esc(string v)
        {
            v ??= "";
            if (v.IndexOfAny(new[] { ',', ';', '"', '\n', '\r' }) >= 0)
                return "\"" + v.Replace("\"", "\"\"") + "\"";
            return v;
        }

        private static DataGridViewTextBoxColumn Col(string prop, string header)
            => new() { DataPropertyName = prop, HeaderText = header };
        private static Label Lbl(string t, int x, int y) => new() { Text = t, Left = x, Top = y, AutoSize = true };
        private static Button Btn(string t, EventHandler h)
        {
            var b = new Button { Text = t, Width = 110, Height = 30 };
            b.Click += h;
            return b;
        }
    }
}
