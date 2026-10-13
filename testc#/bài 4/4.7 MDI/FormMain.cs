namespace Bai4_7
{
    public class FormMain : Form
    {
        private readonly MenuStrip menu = new();
        private readonly ToolStrip tool = new();
        private readonly StatusStrip status = new();
        private readonly ToolStripStatusLabel lblUser = new();
        private readonly ToolStripStatusLabel lblMessage = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft, Text = "Sẵn sàng" };
        private readonly ToolStripStatusLabel lblTime = new();
        private readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };

        public string CurrentUser { get; }
        public bool LoggedOut { get; private set; }

        public FormMain(string user)
        {
            CurrentUser = user;
            Text = "Bài 4.7 - Ứng dụng MDI";
            IsMdiContainer = true;               
            WindowState = FormWindowState.Maximized;

            BuildMenu();
            BuildToolStrip();
            BuildStatus();

            Controls.Add(status);
            Controls.Add(tool);
            Controls.Add(menu);
            MainMenuStrip = menu;

            lblUser.Text = "Người dùng: " + CurrentUser;
            timer.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
            lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void BuildMenu()
        {
            var mSys = new ToolStripMenuItem("Hệ thống");
            mSys.DropDownItems.Add("Đăng xuất", null, (s, e) => { LoggedOut = true; Close(); });
            mSys.DropDownItems.Add(new ToolStripSeparator());
            mSys.DropDownItems.Add("Thoát", null, (s, e) => { LoggedOut = false; Close(); });

            var mCat = new ToolStripMenuItem("Danh mục");
            mCat.DropDownItems.Add("Quản lý Sinh viên", null, (s, e) => OpenChild<FormStudent>());
            mCat.DropDownItems.Add("Quản lý Sản phẩm", null, (s, e) => OpenChild<FormProduct>());

            var mWin = new ToolStripMenuItem("Cửa sổ");
            mWin.DropDownItems.Add("Cascade", null, (s, e) => LayoutMdi(MdiLayout.Cascade));
            mWin.DropDownItems.Add("Tile Horizontal", null, (s, e) => LayoutMdi(MdiLayout.TileHorizontal));
            mWin.DropDownItems.Add("Tile Vertical", null, (s, e) => LayoutMdi(MdiLayout.TileVertical));
            mWin.DropDownItems.Add(new ToolStripSeparator());
            mWin.DropDownItems.Add("Làm mới tất cả cửa sổ", null, (s, e) =>
            {
                foreach (var c in MdiChildren.OfType<ChildFormBase>()) c.RefreshData();   
            });

            menu.Items.AddRange(new ToolStripItem[] { mSys, mCat, mWin });
            menu.MdiWindowListItem = mWin;  
        }

        private void BuildToolStrip()
        {
            tool.Items.Add(new ToolStripButton("Sinh viên", SystemIcons.Information.ToBitmap(), (s, e) => OpenChild<FormStudent>())
            { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });
            tool.Items.Add(new ToolStripButton("Sản phẩm", SystemIcons.Application.ToBitmap(), (s, e) => OpenChild<FormProduct>())
            { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });
            tool.Items.Add(new ToolStripSeparator());
            tool.Items.Add(new ToolStripButton("Thoát", SystemIcons.Error.ToBitmap(), (s, e) => Close())
            { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });
        }

        private void BuildStatus()
        {
            status.Items.AddRange(new ToolStripItem[] { lblUser, lblMessage, lblTime });
        }

        private void OpenChild<T>() where T : ChildFormBase, new()
        {
            var existing = MdiChildren.OfType<T>().FirstOrDefault();
            if (existing != null)
            {
                if (existing.WindowState == FormWindowState.Minimized)
                    existing.WindowState = FormWindowState.Normal;
                existing.Activate();
                existing.BringToFront();
                lblMessage.Text = $"Đã kích hoạt cửa sổ: {existing.Text}";
                return;
            }

            var child = new T { MdiParent = this, CurrentUser = CurrentUser };
            child.StatusChanged += msg => lblMessage.Text = msg;   
            child.Show();
            lblMessage.Text = $"Đã mở cửa sổ: {child.Text}";
        }
    }
}
