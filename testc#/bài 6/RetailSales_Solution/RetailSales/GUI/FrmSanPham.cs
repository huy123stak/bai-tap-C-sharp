using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FrmSanPham : Form
    {
        private readonly TextBox txtMa, txtTen, txtGia, txtTon, txtTim;
        private readonly DataGridView grid;
        private readonly SanPhamBUS bus = new SanPhamBUS();

        public FrmSanPham()
        {
            Text = "Quản lý Sản phẩm";
            Width = 920;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Mã SP / Mã vạch", 20, 22);
            txtMa = Ui.Txt(this, 150, 18, 220);
            Ui.Lbl(this, "Tên sản phẩm", 20, 57);
            txtTen = Ui.Txt(this, 150, 53, 220);
            Ui.Lbl(this, "Giá bán", 20, 92);
            txtGia = Ui.Txt(this, 150, 88, 220);
            Ui.Lbl(this, "Số lượng tồn", 20, 127);
            txtTon = Ui.Txt(this, 150, 123, 220);

            Ui.Btn(this, "Thêm", 420, 16, 100, (s, e) => Ui.Run(() =>
            {
                bus.Insert(Read());
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Sửa", 530, 16, 100, (s, e) => Ui.Run(() =>
            {
                bus.Update(Read());
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Xóa", 640, 16, 100, (s, e) => Ui.Run(() =>
            {
                if (!Ui.Confirm("Bạn có chắc muốn xóa sản phẩm này?")) return;
                bus.Delete(txtMa.Text.Trim());
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Làm mới", 750, 16, 100, (s, e) =>
            {
                txtTim.Clear();
                Reload();
                Clear();
            });

            Ui.Lbl(this, "Tìm theo tên / mã vạch", 420, 70);
            txtTim = Ui.Txt(this, 420, 95, 320);
            Ui.Btn(this, "Tìm kiếm", 750, 91, 100, (s, e) => Ui.Run(Reload));

            grid = Ui.Grid(this, 20, 170, 865, 395);
            grid.SelectionChanged += (s, e) =>
            {
                var r = grid.CurrentRow;
                if (r == null || r.DataBoundItem == null) return;
                txtMa.Text = Convert.ToString(r.Cells["MaSP"].Value);
                txtTen.Text = Convert.ToString(r.Cells["TenSP"].Value);
                txtGia.Text = Convert.ToDecimal(r.Cells["GiaBan"].Value).ToString("0.##");
                txtTon.Text = Convert.ToString(r.Cells["SoLuongTon"].Value);
            };

            Load += (s, e) => Ui.Run(Reload);
        }

        private void Reload()
        {
            grid.DataSource = bus.Search(txtTim.Text);
        }

        private void Clear()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtGia.Clear();
            txtTon.Clear();
        }

        private SanPhamDTO Read()
        {
            decimal gia;
            int ton;
            if (!decimal.TryParse(txtGia.Text, out gia)) throw new Exception("Giá bán không hợp lệ.");
            if (!int.TryParse(txtTon.Text, out ton)) throw new Exception("Số lượng tồn không hợp lệ.");
            return new SanPhamDTO { MaSP = txtMa.Text, TenSP = txtTen.Text, GiaBan = gia, SoLuongTon = ton };
        }
    }
}
