namespace HospitalMdi;

public class frmHospitalMain : Form
{
    readonly MenuStrip menu = new();
    readonly StatusStrip status = new();
    readonly ToolStripStatusLabel lblDuty = new();
    readonly ToolStripStatusLabel lblMessage = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft, Text = "Sẵn sàng" };
    readonly ToolStripProgressBar progressBar = new() { Width = 150, Minimum = 0, Maximum = 100, Visible = false };
    bool reloading;

    public bool LoggedOut { get; private set; }

    public frmHospitalMain(string dutyName)
    {
        Text = "Hệ thống quản lý bệnh viện";
        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;

        lblDuty.Text = "Bác sĩ trực: " + dutyName;

        var mSys = new ToolStripMenuItem("Hệ thống");
        mSys.DropDownItems.Add("Đăng xuất", null, (s, e) => Logout());
        mSys.DropDownItems.Add("Nạp lại dữ liệu", null, async (s, e) => await ReloadAsync());
        mSys.DropDownItems.Add(new ToolStripSeparator());
        mSys.DropDownItems.Add("Thoát", null, (s, e) => { LoggedOut = false; Close(); });

        var mManage = new ToolStripMenuItem("Quản lý");
        mManage.DropDownItems.Add("Hồ sơ Bệnh nhân", null, (s, e) => OpenChild<frmPatientProfiles>());
        mManage.DropDownItems.Add("Lịch trực Bác sĩ", null, (s, e) => OpenChild<frmDoctorSchedule>());
        mManage.DropDownItems.Add("Danh mục Bác sĩ", null, (s, e) => OpenChild<frmDoctorCatalog>());
        mManage.DropDownItems.Add("Danh mục Khoa/Phòng", null, (s, e) => OpenChild<frmDepartmentCatalog>());

        var mWindow = new ToolStripMenuItem("Cửa sổ");
        mWindow.DropDownItems.Add("Xếp tầng (Cascade)", null, (s, e) => LayoutMdi(MdiLayout.Cascade));
        mWindow.DropDownItems.Add("Ngang (Tile Horizontal)", null, (s, e) => LayoutMdi(MdiLayout.TileHorizontal));
        mWindow.DropDownItems.Add("Dọc (Tile Vertical)", null, (s, e) => LayoutMdi(MdiLayout.TileVertical));
        mWindow.DropDownItems.Add(new ToolStripSeparator());
        mWindow.DropDownItems.Add("Đóng tất cả", null, (s, e) => CloseAllChildren());

        menu.Items.AddRange(new ToolStripItem[] { mSys, mManage, mWindow });
        menu.MdiWindowListItem = mWindow;

        status.Items.AddRange(new ToolStripItem[] { lblDuty, lblMessage, progressBar });

        Controls.Add(status);
        Controls.Add(menu);
        MainMenuStrip = menu;
    }

    void OpenChild<T>() where T : Form, new()
    {
        foreach (Form f in MdiChildren)
        {
            if (f is T)
            {
                if (f.WindowState == FormWindowState.Minimized) f.WindowState = FormWindowState.Normal;
                f.Activate();
                lblMessage.Text = "Đã kích hoạt: " + f.Text;
                return;
            }
        }
        var child = new T { MdiParent = this };
        child.Show();
        lblMessage.Text = "Đã mở: " + child.Text;
    }

    void CloseAllChildren()
    {
        foreach (var f in MdiChildren.ToArray()) f.Close();
        lblMessage.Text = "Đã đóng tất cả cửa sổ";
    }

    void Logout()
    {
        var result = MessageBox.Show("Bạn có muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes) return;
        LoggedOut = true;
        Close();
    }

    async Task ReloadAsync()
    {
        if (reloading) return;
        reloading = true;
        progressBar.Value = 0;
        progressBar.Visible = true;
        lblMessage.Text = "Đang nạp lại dữ liệu...";
        try
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(60);
                progressBar.Value = i * 5;
            }
            DataStore.Load();
            foreach (var c in MdiChildren.OfType<ListFormBase>()) c.RefreshData();
            lblMessage.Text = "Đã nạp lại dữ liệu";
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Lỗi: " + ex.Message;
        }
        finally
        {
            progressBar.Visible = false;
            reloading = false;
        }
    }
}
