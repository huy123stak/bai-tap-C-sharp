using System.ComponentModel;
using System.Globalization;
using static BookManager.UiHelper;

namespace BookManager;

public class frmBookManager : Form
{
    readonly List<BookModel> allBooks = new();
    readonly BindingList<BookModel> books = new();
    readonly BindingSource bs = new();
    readonly ErrorProvider errorProvider = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    readonly System.Windows.Forms.Timer clock = new() { Interval = 1000 };
    BookModel selected;
    bool loading;

    TextBox txtBookId, txtTitle, txtAuthor, txtPrice, txtQuantity, txtSearch;
    PictureBox picBookCover;
    DataGridView dgvBooks;
    ToolStripStatusLabel lblTotalBooks, lblTotalValue, lblTime;
    string coverPath;

    public frmBookManager()
    {
        Text = "Quản lý sách nhà sách";
        MinimumSize = new Size(900, 620);
        Size = new Size(1050, 700);
        StartPosition = FormStartPosition.CenterScreen;

        BuildUi();

        allBooks.Add(new BookModel { BookId = "S001", Title = "Lập trình C# cơ bản", Author = "Nguyễn Văn A", Price = 120000, Quantity = 10 });
        allBooks.Add(new BookModel { BookId = "S002", Title = "Cấu trúc dữ liệu và giải thuật", Author = "Trần Văn B", Price = 150000, Quantity = 5 });
        allBooks.Add(new BookModel { BookId = "S003", Title = "Cơ sở dữ liệu", Author = "Lê Thị C", Price = 98000, Quantity = 8 });

        bs.DataSource = books;
        dgvBooks.DataSource = bs;
        dgvBooks.SelectionChanged += dgvBooks_SelectionChanged;

        clock.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        clock.Start();
        lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        ApplyFilter();
    }

    void BuildUi()
    {
        var menu = new MenuStrip();
        var mFile = new ToolStripMenuItem("File");
        mFile.DropDownItems.Add("Mở file CSV", null, (s, e) => OpenCsv());
        mFile.DropDownItems.Add("Lưu file CSV", null, (s, e) => SaveCsv());
        mFile.DropDownItems.Add(new ToolStripSeparator());
        mFile.DropDownItems.Add("Thoát", null, (s, e) => Close());
        var mEdit = new ToolStripMenuItem("Edit");
        mEdit.DropDownItems.Add("Thêm", null, (s, e) => AddBook());
        mEdit.DropDownItems.Add("Xóa", null, (s, e) => DeleteBook());
        mEdit.DropDownItems.Add("Tìm kiếm", null, (s, e) => { txtSearch.Focus(); txtSearch.SelectAll(); });
        menu.Items.AddRange(new ToolStripItem[] { mFile, mEdit });

        var tool = new ToolStrip();
        tool.Items.Add(new ToolStripButton("Mở file", SystemIcons.Information.ToBitmap(), (s, e) => OpenCsv()) { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });
        tool.Items.Add(new ToolStripButton("Lưu file", SystemIcons.Shield.ToBitmap(), (s, e) => SaveCsv()) { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });
        tool.Items.Add(new ToolStripButton("Thêm mới", SystemIcons.Application.ToBitmap(), (s, e) => AddBook()) { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });

        var status = new StatusStrip();
        lblTotalBooks = new ToolStripStatusLabel();
        lblTotalValue = new ToolStripStatusLabel();
        lblTime = new ToolStripStatusLabel { Spring = true, TextAlign = ContentAlignment.MiddleRight };
        status.Items.AddRange(new ToolStripItem[] { lblTotalBooks, new ToolStripStatusLabel("|"), lblTotalValue, lblTime });

        txtBookId = new TextBox();
        txtTitle = new TextBox();
        txtAuthor = new TextBox();
        txtPrice = new TextBox();
        txtQuantity = new TextBox();

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

        Row(0, "Mã sách:", txtBookId, "Tên sách:", txtTitle);
        Row(1, "Tác giả:", txtAuthor, "Đơn giá:", txtPrice);
        f.Controls.Add(Lbl("Số lượng:"), 0, 2);
        txtQuantity.Dock = DockStyle.Fill;
        f.Controls.Add(txtQuantity, 1, 2);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        buttons.Controls.Add(Btn("Thêm", (s, e) => AddBook()));
        buttons.Controls.Add(Btn("Sửa", (s, e) => EditBook()));
        buttons.Controls.Add(Btn("Xóa", (s, e) => DeleteBook()));
        buttons.Controls.Add(Btn("Làm mới", (s, e) => ClearInputs()));
        f.Controls.Add(buttons, 0, 3);
        f.SetColumnSpan(buttons, 4);

        var group = new GroupBox { Text = "Thông tin sách", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        group.Controls.Add(f);

        picBookCover = new PictureBox { Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
        var picPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        picPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        picPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        picPanel.Controls.Add(picBookCover, 0, 0);
        picPanel.Controls.Add(Btn("Tải ảnh bìa", (s, e) => LoadCover(), 120), 0, 1);
        var picGroup = new GroupBox { Text = "Ảnh bìa", Dock = DockStyle.Fill };
        picGroup.Controls.Add(picPanel);

        var top = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, AutoSize = true };
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        top.Controls.Add(group, 0, 0);
        top.Controls.Add(picGroup, 1, 0);

        txtSearch = new TextBox { Width = 260 };
        txtSearch.TextChanged += (s, e) => ApplyFilter();
        var searchBar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        searchBar.Controls.Add(Lbl("Tìm kiếm (mã / tên / tác giả):"));
        searchBar.Controls.Add(txtSearch);

        dgvBooks = NewGrid();
        dgvBooks.Columns.Add(Col(nameof(BookModel.BookId), "Mã sách", 9));
        dgvBooks.Columns.Add(Col(nameof(BookModel.Title), "Tên sách", 28));
        dgvBooks.Columns.Add(Col(nameof(BookModel.Author), "Tác giả", 18));
        var colPrice = Col(nameof(BookModel.Price), "Đơn giá", 12, "N0");
        colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvBooks.Columns.Add(colPrice);
        dgvBooks.Columns.Add(Col(nameof(BookModel.Quantity), "SL", 6));
        dgvBooks.Columns.Add(Col(nameof(BookModel.CoverFile), "Ảnh bìa", 17));

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(8) };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(top, 0, 0);
        root.Controls.Add(searchBar, 0, 1);
        root.Controls.Add(dgvBooks, 0, 2);

        Controls.Add(root);
        Controls.Add(status);
        Controls.Add(tool);
        Controls.Add(menu);
        MainMenuStrip = menu;
    }

    void dgvBooks_SelectionChanged(object sender, EventArgs e)
    {
        if (loading || dgvBooks.SelectedRows.Count == 0) return;
        if (dgvBooks.SelectedRows[0].DataBoundItem is not BookModel b) return;
        selected = b;
        txtBookId.Text = b.BookId;
        txtTitle.Text = b.Title;
        txtAuthor.Text = b.Author;
        txtPrice.Text = b.Price.ToString("0.##", CultureInfo.CurrentCulture);
        txtQuantity.Text = b.Quantity.ToString();
        coverPath = b.CoverPath;
        ShowCover(coverPath);
        errorProvider.Clear();
    }

    bool ValidateInput(out decimal price, out int quantity)
    {
        errorProvider.Clear();
        bool ok = true;
        price = 0;
        quantity = 0;

        string id = txtBookId.Text.Trim();
        if (id.Length == 0)
        {
            errorProvider.SetError(txtBookId, "Mã sách không được để trống");
            ok = false;
        }
        else if (allBooks.Any(b => b.BookId.Equals(id, StringComparison.OrdinalIgnoreCase) && !ReferenceEquals(b, selected)))
        {
            errorProvider.SetError(txtBookId, "Mã sách đã tồn tại");
            ok = false;
        }

        if (txtTitle.Text.Trim().Length == 0)
        {
            errorProvider.SetError(txtTitle, "Tên sách không được để trống");
            ok = false;
        }

        if (!decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out price) || price < 0)
        {
            errorProvider.SetError(txtPrice, "Đơn giá phải là số không âm");
            ok = false;
        }

        if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) || quantity < 0)
        {
            errorProvider.SetError(txtQuantity, "Số lượng phải là số nguyên không âm");
            ok = false;
        }

        return ok;
    }

    void AddBook()
    {
        selected = null;
        if (!ValidateInput(out decimal price, out int quantity)) return;
        var b = new BookModel
        {
            BookId = txtBookId.Text.Trim(),
            Title = txtTitle.Text.Trim(),
            Author = txtAuthor.Text.Trim(),
            Price = price,
            Quantity = quantity,
            CoverPath = coverPath
        };
        allBooks.Add(b);
        ApplyFilter(b);
    }

    void EditBook()
    {
        if (selected == null)
        {
            MessageBox.Show("Hãy chọn một cuốn sách trong danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ValidateInput(out decimal price, out int quantity)) return;
        selected.BookId = txtBookId.Text.Trim();
        selected.Title = txtTitle.Text.Trim();
        selected.Author = txtAuthor.Text.Trim();
        selected.Price = price;
        selected.Quantity = quantity;
        selected.CoverPath = coverPath;
        UpdateStatus();
    }

    void DeleteBook()
    {
        if (selected == null)
        {
            MessageBox.Show("Hãy chọn một cuốn sách trong danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var result = MessageBox.Show($"Xóa sách \"{selected.Title}\"?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result != DialogResult.Yes) return;
        allBooks.Remove(selected);
        selected = null;
        ClearInputs();
        ApplyFilter();
    }

    void ClearInputs()
    {
        selected = null;
        errorProvider.Clear();
        txtBookId.Clear();
        txtTitle.Clear();
        txtAuthor.Clear();
        txtPrice.Clear();
        txtQuantity.Clear();
        coverPath = null;
        ShowCover(null);
        dgvBooks.ClearSelection();
        txtBookId.Focus();
    }

    void ApplyFilter(BookModel select = null)
    {
        var keep = select ?? selected;
        string q = txtSearch.Text.Trim();
        var result = allBooks.Where(b =>
            q.Length == 0
            || b.BookId.Contains(q, StringComparison.OrdinalIgnoreCase)
            || b.Title.Contains(q, StringComparison.OrdinalIgnoreCase)
            || (b.Author ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        loading = true;
        books.RaiseListChangedEvents = false;
        books.Clear();
        foreach (var b in result) books.Add(b);
        books.RaiseListChangedEvents = true;
        books.ResetBindings();
        loading = false;

        if (keep != null && books.Contains(keep))
        {
            int idx = books.IndexOf(keep);
            dgvBooks.ClearSelection();
            dgvBooks.Rows[idx].Selected = true;
            dgvBooks.CurrentCell = dgvBooks.Rows[idx].Cells[0];
        }
        else
        {
            selected = null;
        }
        UpdateStatus();
    }

    void UpdateStatus()
    {
        lblTotalBooks.Text = $"Tổng số đầu sách: {allBooks.Count}";
        lblTotalValue.Text = $"Tổng giá trị kho: {allBooks.Sum(b => b.TotalValue):N0} đ";
    }

    void LoadCover()
    {
        using var ofd = new OpenFileDialog
        {
            Title = "Chọn ảnh bìa",
            Filter = "Hình ảnh (*.png;*.jpg)|*.png;*.jpg"
        };
        if (ofd.ShowDialog() != DialogResult.OK) return;
        coverPath = ofd.FileName;
        ShowCover(coverPath);
        if (selected != null) selected.CoverPath = coverPath;
    }

    void ShowCover(string path)
    {
        picBookCover.Image?.Dispose();
        picBookCover.Image = null;
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
        try
        {
            picBookCover.Image = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
        }
        catch
        {
            picBookCover.Image = null;
        }
    }

    void SaveCsv()
    {
        using var sfd = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = "DanhSachSach.csv" };
        if (sfd.ShowDialog() != DialogResult.OK) return;
        try
        {
            CsvService.Save(sfd.FileName, allBooks);
            MessageBox.Show($"Đã lưu {allBooks.Count} đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi ghi file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    void OpenCsv()
    {
        using var ofd = new OpenFileDialog { Filter = "CSV (*.csv)|*.csv" };
        if (ofd.ShowDialog() != DialogResult.OK) return;
        try
        {
            var loaded = CsvService.Load(ofd.FileName);
            allBooks.Clear();
            allBooks.AddRange(loaded);
            selected = null;
            txtSearch.Clear();
            ApplyFilter();
            MessageBox.Show($"Đã nạp {loaded.Count} đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
