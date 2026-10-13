using System.ComponentModel;

namespace Bai4_7
{
    public class FormProduct : ChildFormBase
    {
        private readonly BindingList<Product> _list = new();
        private readonly BindingSource _bs = new();
        private readonly DataGridView dgv = new() { Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true,
            AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly TextBox txtId = new() { Left = 80, Top = 15, Width = 90 };
        private readonly TextBox txtName = new() { Left = 260, Top = 15, Width = 160 };
        private readonly TextBox txtPrice = new() { Left = 80, Top = 50, Width = 90 };

        public FormProduct()
        {
            Text = "Quản lý Sản phẩm";
            Size = new Size(650, 420);

            foreach (var (p, h) in new[] { ("ProductId", "Mã SP"), ("ProductName", "Tên sản phẩm"), ("Price", "Đơn giá") })
                dgv.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = p, HeaderText = h });
            dgv.Columns[2].DefaultCellStyle.Format = "N0";

            var top = new Panel { Dock = DockStyle.Top, Height = 95 };
            var btnAdd = new Button { Text = "Thêm", Left = 460, Top = 12, Width = 90 };
            var btnDel = new Button { Text = "Xóa", Left = 460, Top = 48, Width = 90 };
            btnAdd.Click += (s, e) => Add();
            btnDel.Click += (s, e) => Remove();
            top.Controls.AddRange(new Control[] {
                L("Mã SP", 10, 18), txtId, L("Tên SP", 190, 18), txtName, L("Đơn giá", 10, 53), txtPrice, btnAdd, btnDel });
            Controls.Add(dgv);
            Controls.Add(top);

            var ctx = new ContextMenuStrip();
            ctx.Items.Add("Xóa dòng", null, (s, e) => Remove());
            ctx.Items.Add("Làm mới dữ liệu", null, (s, e) => RefreshData());
            ctx.Items.Add("Xem chi tiết", null, (s, e) =>
            {
                if (_bs.Current is Product p) MessageBox.Show($"Mã: {p.ProductId}\nTên: {p.ProductName}\nGiá: {p.Price:N0}", "Chi tiết sản phẩm");
            });
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
            Load += (s, e) => ReportStatus($"Xin chào {CurrentUser} - đã mở Quản lý Sản phẩm ({_list.Count} bản ghi)");
        }

        private void Seed()
        {
            _list.Clear();
            _list.Add(new Product { ProductId = "SP001", ProductName = "Bút bi", Price = 5000 });
            _list.Add(new Product { ProductId = "SP002", ProductName = "Vở 200 trang", Price = 18000 });
            _list.Add(new Product { ProductId = "SP003", ProductName = "Thước kẻ", Price = 7000 });
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin hợp lệ.", "Thông báo");
                return;
            }
            _list.Add(new Product { ProductId = txtId.Text.Trim(), ProductName = txtName.Text.Trim(), Price = price });
            ReportStatus($"Thêm sản phẩm {txtName.Text.Trim()} thành công");
            txtId.Clear(); txtName.Clear(); txtPrice.Clear();
        }

        private void Remove()
        {
            if (_bs.Current is not Product p) return;
            _bs.RemoveCurrent();
            ReportStatus($"Đã xóa sản phẩm {p.ProductName}");
        }

        public override void RefreshData()
        {
            Seed();
            ReportStatus("Đã làm mới dữ liệu Sản phẩm");
        }

        private static Label L(string t, int x, int y) => new() { Text = t, Left = x, Top = y, AutoSize = true };
    }
}
