namespace Bai4_8
{
    public class FormMain : Form
    {
        private readonly MenuStrip menu = new();
        private readonly ToolStrip tool = new();
        private readonly StatusStrip status = new();
        private readonly ToolStripStatusLabel lblUser = new() { Text = "Người dùng: admin" };
        private readonly ToolStripStatusLabel lblMessage = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft, Text = "Sẵn sàng" };
        private readonly ToolStripProgressBar statusBar = new() { Width = 120, Visible = false };
        private readonly ToolStripStatusLabel lblPercent = new() { Visible = false };
        private readonly ToolStripStatusLabel lblTime = new();
        private readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };

        public FormMain()
        {
            Text = "Bài 4.8 - MDI + Async/Await + ProgressBar";
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;

            var mSys = new ToolStripMenuItem("Hệ thống");
            mSys.DropDownItems.Add("Thoát", null, (s, e) => Close());
            var mCat = new ToolStripMenuItem("Danh mục");
            mCat.DropDownItems.Add("Quản lý Sinh viên", null, (s, e) => OpenChild<FormStudent>());
            mCat.DropDownItems.Add("Quản lý Sản phẩm", null, (s, e) => OpenChild<FormProduct>());
            var mWin = new ToolStripMenuItem("Cửa sổ");
            mWin.DropDownItems.Add("Cascade", null, (s, e) => LayoutMdi(MdiLayout.Cascade));
            mWin.DropDownItems.Add("Tile Horizontal", null, (s, e) => LayoutMdi(MdiLayout.TileHorizontal));
            mWin.DropDownItems.Add("Tile Vertical", null, (s, e) => LayoutMdi(MdiLayout.TileVertical));
            menu.Items.AddRange(new ToolStripItem[] { mSys, mCat, mWin });
            menu.MdiWindowListItem = mWin;

            tool.Items.Add(new ToolStripButton("Sinh viên", SystemIcons.Information.ToBitmap(), (s, e) => OpenChild<FormStudent>())
            { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });
            tool.Items.Add(new ToolStripButton("Sản phẩm", SystemIcons.Application.ToBitmap(), (s, e) => OpenChild<FormProduct>())
            { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText });

            status.Items.AddRange(new ToolStripItem[] { lblUser, lblMessage, statusBar, lblPercent, lblTime });

            Controls.Add(status);
            Controls.Add(tool);
            Controls.Add(menu);
            MainMenuStrip = menu;

            timer.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
            lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void OpenChild<T>() where T : ChildFormBase, new()
        {
            var existing = MdiChildren.OfType<T>().FirstOrDefault();
            if (existing != null)
            {
                if (existing.WindowState == FormWindowState.Minimized) existing.WindowState = FormWindowState.Normal;
                existing.Activate();
                return;
            }

            var child = new T { MdiParent = this };
            child.StatusChanged += msg => lblMessage.Text = msg;
            child.ProgressChanged += p =>
            {
                bool show = p >= 0 && p < 100;
                statusBar.Visible = lblPercent.Visible = show;
                if (p >= 0) { statusBar.Value = Math.Min(p, 100); lblPercent.Text = p + "%"; }
            };
            child.Show();
        }
    }
}
