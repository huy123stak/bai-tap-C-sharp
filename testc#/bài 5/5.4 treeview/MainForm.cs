using System.Drawing.Drawing2D;

namespace App;

public class Employee
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public string Department { get; set; }
    public string Group { get; set; }
}

public enum NodeLevel
{
    Company,
    Department,
    Group
}

public class NodeInfo
{
    public NodeLevel Level { get; set; }
    public string Name { get; set; }
}

public class MainForm : Form
{
    private SplitContainer splitMain;
    private TreeView tvDepartments;
    private ListView lsvEmployees;
    private ComboBox cboViewMode;
    private Label lblStatus;
    private ImageList imgSmall, imgLarge;
    private readonly List<Employee> employees = new();

    private readonly (string Text, View Mode)[] viewModes =
    {
        ("Details", View.Details),
        ("SmallIcon", View.SmallIcon),
        ("LargeIcon", View.LargeIcon),
        ("Tile", View.Tile)
    };

    public MainForm()
    {
        Text = "Quản lý nhân sự theo cơ cấu tổ chức";
        ClientSize = new Size(960, 600);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10F);

        imgSmall = CreateImageList(16);
        imgLarge = CreateImageList(32);

        var topPanel = new Panel { Dock = DockStyle.Top, Height = 48 };
        topPanel.Controls.Add(new Label { Text = "Chế độ xem:", Location = new Point(12, 14), AutoSize = true });
        cboViewMode = new ComboBox { Location = new Point(105, 10), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        cboViewMode.Items.AddRange(viewModes.Select(v => (object)v.Text).ToArray());
        cboViewMode.SelectedIndexChanged += CboViewMode_SelectedIndexChanged;
        topPanel.Controls.Add(cboViewMode);

        lblStatus = new Label { Dock = DockStyle.Bottom, Height = 28, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(8, 0, 0, 0), BorderStyle = BorderStyle.Fixed3D };

        splitMain = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 280 };

        tvDepartments = new TreeView
        {
            Name = "tvDepartments",
            Dock = DockStyle.Fill,
            ImageList = imgSmall,
            HideSelection = false,
            ShowLines = true
        };
        tvDepartments.AfterSelect += TvDepartments_AfterSelect;
        splitMain.Panel1.Controls.Add(tvDepartments);

        lsvEmployees = new ListView
        {
            Name = "lsvEmployees",
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            SmallImageList = imgSmall,
            LargeImageList = imgLarge,
            HideSelection = false,
            TileSize = new Size(220, 48)
        };
        lsvEmployees.Columns.Add("Mã NV", 90);
        lsvEmployees.Columns.Add("Họ Tên", 200);
        lsvEmployees.Columns.Add("Chức vụ", 160);
        lsvEmployees.Columns.Add("Ngày vào làm", 120);
        splitMain.Panel2.Controls.Add(lsvEmployees);

        Controls.Add(splitMain);
        Controls.Add(topPanel);
        Controls.Add(lblStatus);
        splitMain.BringToFront();

        SeedEmployees();
        BuildTree();
        cboViewMode.SelectedIndex = 0;
        tvDepartments.SelectedNode = tvDepartments.Nodes[0];
    }

    private static ImageList CreateImageList(int size)
    {
        var list = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(size, size) };
        list.Images.Add(DrawIcon(size, 0));
        list.Images.Add(DrawIcon(size, 1));
        list.Images.Add(DrawIcon(size, 2));
        list.Images.Add(DrawIcon(size, 3));
        return list;
    }

    private static Bitmap DrawIcon(int size, int kind)
    {
        var bmp = new Bitmap(size, size);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);
        float u = size / 16f;

        switch (kind)
        {
            case 0:
                using (var body = new SolidBrush(Color.SteelBlue))
                using (var win = new SolidBrush(Color.White))
                {
                    g.FillRectangle(body, 2 * u, 2 * u, 12 * u, 13 * u);
                    for (int r = 0; r < 3; r++)
                        for (int c = 0; c < 3; c++)
                            g.FillRectangle(win, (4 + c * 3.5f) * u, (4 + r * 3.5f) * u, 2 * u, 2 * u);
                }
                break;
            case 1:
                using (var tab = new SolidBrush(Color.Goldenrod))
                using (var body = new SolidBrush(Color.Gold))
                {
                    g.FillRectangle(tab, 1 * u, 3 * u, 6 * u, 3 * u);
                    g.FillRectangle(body, 1 * u, 5 * u, 14 * u, 9 * u);
                }
                break;
            case 2:
                using (var head = new SolidBrush(Color.SeaGreen))
                {
                    g.FillEllipse(head, 1 * u, 3 * u, 6 * u, 6 * u);
                    g.FillEllipse(head, 9 * u, 3 * u, 6 * u, 6 * u);
                    g.FillEllipse(head, 4 * u, 8 * u, 8 * u, 7 * u);
                }
                break;
            default:
                using (var brush = new SolidBrush(Color.SlateGray))
                {
                    g.FillEllipse(brush, 5 * u, 1 * u, 6 * u, 6 * u);
                    g.FillEllipse(brush, 2 * u, 8 * u, 12 * u, 9 * u);
                }
                break;
        }
        return bmp;
    }

    private void SeedEmployees()
    {
        void Add(string id, string name, string pos, DateTime date, string dept, string group)
        {
            employees.Add(new Employee { Id = id, FullName = name, Position = pos, StartDate = date, Department = dept, Group = group });
        }

        Add("NV001", "Nguyễn Văn An", "Trưởng nhóm", new DateTime(2019, 3, 12), "Phòng Kỹ thuật", "Nhóm Backend");
        Add("NV002", "Trần Thị Bình", "Lập trình viên", new DateTime(2021, 6, 1), "Phòng Kỹ thuật", "Nhóm Backend");
        Add("NV003", "Lê Quang Cường", "Lập trình viên", new DateTime(2022, 9, 5), "Phòng Kỹ thuật", "Nhóm Backend");
        Add("NV004", "Phạm Thu Dung", "Trưởng nhóm", new DateTime(2020, 1, 20), "Phòng Kỹ thuật", "Nhóm Frontend");
        Add("NV005", "Hoàng Minh Đức", "Lập trình viên", new DateTime(2023, 2, 15), "Phòng Kỹ thuật", "Nhóm Frontend");
        Add("NV006", "Vũ Thị Hà", "Chuyên viên tuyển dụng", new DateTime(2020, 7, 8), "Phòng Nhân sự", "Nhóm Tuyển dụng");
        Add("NV007", "Đặng Văn Hùng", "Trưởng nhóm", new DateTime(2018, 11, 30), "Phòng Nhân sự", "Nhóm Tuyển dụng");
        Add("NV008", "Bùi Thị Lan", "Chuyên viên đào tạo", new DateTime(2021, 4, 19), "Phòng Nhân sự", "Nhóm Đào tạo");
        Add("NV009", "Ngô Quốc Khánh", "Trưởng nhóm", new DateTime(2019, 8, 2), "Phòng Kinh doanh", "Nhóm Miền Bắc");
        Add("NV010", "Đỗ Thị Mai", "Nhân viên kinh doanh", new DateTime(2022, 12, 12), "Phòng Kinh doanh", "Nhóm Miền Bắc");
        Add("NV011", "Lý Văn Nam", "Trưởng nhóm", new DateTime(2018, 5, 25), "Phòng Kinh doanh", "Nhóm Miền Nam");
        Add("NV012", "Trịnh Thị Oanh", "Nhân viên kinh doanh", new DateTime(2023, 10, 9), "Phòng Kinh doanh", "Nhóm Miền Nam");
    }

    private void BuildTree()
    {
        tvDepartments.BeginUpdate();
        tvDepartments.Nodes.Clear();

        var root = new TreeNode("Công ty ABC", 0, 0)
        {
            Tag = new NodeInfo { Level = NodeLevel.Company, Name = "Công ty ABC" }
        };

        var structure = employees
            .GroupBy(e => e.Department)
            .Select(d => new { Department = d.Key, Groups = d.Select(e => e.Group).Distinct().ToList() });

        foreach (var dept in structure)
        {
            var deptNode = new TreeNode(dept.Department, 1, 1)
            {
                Tag = new NodeInfo { Level = NodeLevel.Department, Name = dept.Department }
            };
            foreach (var group in dept.Groups)
            {
                deptNode.Nodes.Add(new TreeNode(group, 2, 2)
                {
                    Tag = new NodeInfo { Level = NodeLevel.Group, Name = group }
                });
            }
            root.Nodes.Add(deptNode);
        }

        tvDepartments.Nodes.Add(root);
        root.Expand();
        foreach (TreeNode child in root.Nodes) child.Expand();
        tvDepartments.EndUpdate();
    }

    private void TvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not NodeInfo info) return;

        IEnumerable<Employee> query = info.Level switch
        {
            NodeLevel.Department => employees.Where(x => x.Department == info.Name),
            NodeLevel.Group => employees.Where(x => x.Group == info.Name),
            _ => employees
        };

        var result = query.ToList();

        lsvEmployees.BeginUpdate();
        lsvEmployees.Items.Clear();
        foreach (var emp in result)
        {
            var item = new ListViewItem(emp.Id) { ImageIndex = 3 };
            item.SubItems.Add(emp.FullName);
            item.SubItems.Add(emp.Position);
            item.SubItems.Add(emp.StartDate.ToString("dd/MM/yyyy"));
            lsvEmployees.Items.Add(item);
        }
        lsvEmployees.EndUpdate();

        lblStatus.Text = $"{e.Node.Text}: {result.Count} nhân viên";
    }

    private void CboViewMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboViewMode.SelectedIndex >= 0)
            lsvEmployees.View = viewModes[cboViewMode.SelectedIndex].Mode;
    }
}
