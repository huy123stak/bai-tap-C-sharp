using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai4_5_MdiShell
{
    /// <summary>
    /// BAI 4.5: Form chinh dong vai tro MDI Parent, chua cac Form con
    /// (FormRegisterChild, FormProductChild) va co MenuStrip dieu huong.
    /// </summary>
    public class FormMainMdi : Form
    {
        private MenuStrip menuStrip1;

        public FormMainMdi()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Ung dung quan ly - MDI Parent";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Bien Form chinh thanh vung chua cac cua so con
            this.IsMdiContainer = true;

            menuStrip1 = new MenuStrip();

            ToolStripMenuItem menuHeThong = new ToolStripMenuItem("He thong");

            ToolStripMenuItem menuDangKy = new ToolStripMenuItem("Dang ky hoc vien");
            menuDangKy.Click += menuDangKy_Click;

            ToolStripMenuItem menuSanPham = new ToolStripMenuItem("Quan ly san pham");
            menuSanPham.Click += menuSanPham_Click;

            ToolStripMenuItem menuThoat = new ToolStripMenuItem("Thoat");
            menuThoat.Click += (s, e) => this.Close();

            menuHeThong.DropDownItems.Add(menuDangKy);
            menuHeThong.DropDownItems.Add(menuSanPham);
            menuHeThong.DropDownItems.Add(new ToolStripSeparator());
            menuHeThong.DropDownItems.Add(menuThoat);

            ToolStripMenuItem menuCuaSo = new ToolStripMenuItem("Cua so");

            ToolStripMenuItem menuCascade = new ToolStripMenuItem("Sap xep tang (Cascade)");
            menuCascade.Click += (s, e) => this.LayoutMdi(MdiLayout.Cascade);

            ToolStripMenuItem menuTileH = new ToolStripMenuItem("Sap xep ngang (Tile Horizontal)");
            menuTileH.Click += (s, e) => this.LayoutMdi(MdiLayout.TileHorizontal);

            menuCuaSo.DropDownItems.Add(menuCascade);
            menuCuaSo.DropDownItems.Add(menuTileH);

            menuStrip1.Items.Add(menuHeThong);
            menuStrip1.Items.Add(menuCuaSo);

            this.MainMenuStrip = menuStrip1;
            this.Controls.Add(menuStrip1);
        }

        private void menuDangKy_Click(object sender, EventArgs e)
        {
            // Khoi tao Form con
            FormRegisterChild child = new FormRegisterChild();
            // Khai bao Form cha cho no
            child.MdiParent = this;
            // Hien thi len man hinh, luon nam gon trong khung Form chinh
            child.Show();
        }

        private void menuSanPham_Click(object sender, EventArgs e)
        {
            FormProductChild child = new FormProductChild();
            child.MdiParent = this;
            child.Show();
        }
    }
}
