namespace App;

public class Product
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
}

public class MainForm : Form
{
    private TextBox txtProductId, txtProductName, txtSearch;
    private NumericUpDown numUnitPrice, numQuantity;
    private ComboBox cboCategory;
    private Button btnAdd, btnEdit, btnDelete, btnSearch, btnClear;
    private DataGridView dgvProducts;
    private readonly BindingSource bindingSource = new();
    private readonly List<Product> products = new();
    private string keyword = string.Empty;

    public MainForm()
    {
        Text = "Quản lý danh sách sản phẩm";
        ClientSize = new Size(900, 640);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10F);

        var grpInfo = new GroupBox { Text = "Thông tin sản phẩm", Location = new Point(15, 10), Size = new Size(870, 150) };
        AddLabel(grpInfo, "Mã SP:", 15, 35);
        AddLabel(grpInfo, "Tên SP:", 15, 80);
        AddLabel(grpInfo, "Đơn giá:", 440, 35);
        AddLabel(grpInfo, "Số lượng:", 440, 80);
        AddLabel(grpInfo, "Danh mục:", 15, 118);

        txtProductId = new TextBox { Location = new Point(100, 32), Width = 300 };
        txtProductName = new TextBox { Location = new Point(100, 77), Width = 300 };
        numUnitPrice = new NumericUpDown
        {
            Location = new Point(530, 32),
            Width = 300,
            Maximum = 1000000000,
            ThousandsSeparator = true,
            Increment = 1000
        };
        numQuantity = new NumericUpDown { Location = new Point(530, 77), Width = 300, Maximum = 1000000 };
        cboCategory = new ComboBox { Location = new Point(100, 112), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
        cboCategory.Items.AddRange(new object[] { "Điện tử", "Gia dụng", "Thời trang", "Thực phẩm", "Văn phòng phẩm" });
        cboCategory.SelectedIndex = 0;
        grpInfo.Controls.AddRange(new Control[] { txtProductId, txtProductName, numUnitPrice, numQuantity, cboCategory });

        var grpActions = new GroupBox { Text = "Chức năng", Location = new Point(15, 170), Size = new Size(870, 80) };
        btnAdd = new Button { Text = "Thêm", Location = new Point(15, 28), Size = new Size(90, 34) };
        btnEdit = new Button { Text = "Sửa", Location = new Point(115, 28), Size = new Size(90, 34) };
        btnDelete = new Button { Text = "Xóa", Location = new Point(215, 28), Size = new Size(90, 34) };
        btnClear = new Button { Text = "Làm mới", Location = new Point(315, 28), Size = new Size(90, 34) };
        txtSearch = new TextBox { Location = new Point(560, 32), Width = 190 };
        btnSearch = new Button { Text = "Tìm kiếm", Location = new Point(760, 28), Size = new Size(95, 34) };
        grpActions.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, btnClear, txtSearch, btnSearch });
        grpActions.Controls.Add(new Label { Text = "Tên SP:", Location = new Point(490, 36), AutoSize = true });

        dgvProducts = new DataGridView
        {
            Location = new Point(15, 260),
            Size = new Size(870, 365),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AutoGenerateColumns = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = SystemColors.Window,
            RowHeadersVisible = false
        };
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductId", HeaderText = "Mã SP", FillWeight = 15 });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductName", HeaderText = "Tên SP", FillWeight = 35 });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "UnitPrice",
            HeaderText = "Đơn giá",
            FillWeight = 20,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Quantity",
            HeaderText = "Số lượng",
            FillWeight = 12,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "Danh mục", FillWeight = 18 });
        dgvProducts.DataSource = bindingSource;
        dgvProducts.CellClick += DgvProducts_CellClick;

        btnAdd.Click += BtnAdd_Click;
        btnEdit.Click += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;
        btnClear.Click += (s, e) => ClearInputs();
        btnSearch.Click += BtnSearch_Click;
        txtSearch.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSearch_Click(s, e);
            }
        };

        Controls.AddRange(new Control[] { grpInfo, grpActions, dgvProducts });

        products.Add(new Product { ProductId = "SP001", ProductName = "Laptop Dell Inspiron", UnitPrice = 15000000, Quantity = 10, Category = "Điện tử" });
        products.Add(new Product { ProductId = "SP002", ProductName = "Nồi cơm điện", UnitPrice = 1200000, Quantity = 25, Category = "Gia dụng" });
        products.Add(new Product { ProductId = "SP003", ProductName = "Áo thun nam", UnitPrice = 180000, Quantity = 80, Category = "Thời trang" });
        RefreshGrid();
    }

    private static void AddLabel(Control parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });
    }

    private void RefreshGrid()
    {
        var data = products
            .Where(p => string.IsNullOrEmpty(keyword) || p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
        bindingSource.DataSource = data;
        bindingSource.ResetBindings(false);
        dgvProducts.ClearSelection();
    }

    private Product GetSelectedProduct()
    {
        return dgvProducts.SelectedRows.Count > 0 ? dgvProducts.SelectedRows[0].DataBoundItem as Product : null;
    }

    private void ClearInputs()
    {
        txtProductId.Clear();
        txtProductName.Clear();
        numUnitPrice.Value = 0;
        numQuantity.Value = 0;
        cboCategory.SelectedIndex = 0;
        txtProductId.ReadOnly = false;
        dgvProducts.ClearSelection();
        txtProductId.Focus();
    }

    private bool ValidateInputs(bool checkDuplicate)
    {
        if (string.IsNullOrWhiteSpace(txtProductId.Text))
        {
            MessageBox.Show("Mã sản phẩm không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtProductId.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtProductName.Text))
        {
            MessageBox.Show("Tên sản phẩm không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtProductName.Focus();
            return false;
        }
        if (checkDuplicate && products.Any(p => p.ProductId.Equals(txtProductId.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã sản phẩm đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtProductId.Focus();
            return false;
        }
        return true;
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs(true)) return;

        products.Add(new Product
        {
            ProductId = txtProductId.Text.Trim(),
            ProductName = txtProductName.Text.Trim(),
            UnitPrice = numUnitPrice.Value,
            Quantity = (int)numQuantity.Value,
            Category = cboCategory.SelectedItem.ToString()
        });
        RefreshGrid();
        ClearInputs();
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        var product = GetSelectedProduct();
        if (product == null || !txtProductId.ReadOnly)
        {
            MessageBox.Show("Hãy chọn một sản phẩm trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ValidateInputs(false)) return;

        product.ProductName = txtProductName.Text.Trim();
        product.UnitPrice = numUnitPrice.Value;
        product.Quantity = (int)numQuantity.Value;
        product.Category = cboCategory.SelectedItem.ToString();
        RefreshGrid();
        ClearInputs();
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        var product = GetSelectedProduct();
        if (product == null)
        {
            MessageBox.Show("Hãy chọn một sản phẩm trong danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Bạn có chắc muốn xóa sản phẩm \"{product.ProductName}\"?",
            "Xác nhận xóa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            products.Remove(product);
            RefreshGrid();
            ClearInputs();
        }
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        keyword = txtSearch.Text.Trim();
        RefreshGrid();
    }

    private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgvProducts.Rows[e.RowIndex].DataBoundItem is not Product product) return;

        txtProductId.Text = product.ProductId;
        txtProductName.Text = product.ProductName;
        numUnitPrice.Value = Math.Min(product.UnitPrice, numUnitPrice.Maximum);
        numQuantity.Value = Math.Min(product.Quantity, numQuantity.Maximum);
        cboCategory.SelectedItem = product.Category;
        txtProductId.ReadOnly = true;
    }
}
