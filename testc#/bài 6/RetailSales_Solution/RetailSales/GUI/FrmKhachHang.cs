using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FrmKhachHang : Form
    {
        private readonly TextBox txtTen, txtSdt, txtDiem, txtTim;
        private readonly DataGridView grid;
        private readonly KhachHangBUS bus = new KhachHangBUS();
        private int selId;

        public FrmKhachHang()
        {
            Text = "Quản lý Khách hàng";
            Width = 920;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Tên khách hàng", 20, 22);
            txtTen = Ui.Txt(this, 150, 18, 220);
            Ui.Lbl(this, "Điện thoại", 20, 57);
            txtSdt = Ui.Txt(this, 150, 53, 220);
            Ui.Lbl(this, "Điểm tích lũy", 20, 92);
            txtDiem = Ui.Txt(this, 150, 88, 220);
            txtDiem.ReadOnly = true;

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
                if (selId <= 0) throw new Exception("Vui lòng chọn khách hàng cần xóa.");
                if (!Ui.Confirm("Bạn có chắc muốn xóa khách hàng này?")) return;
                bus.Delete(selId);
                Reload();
                Clear();
            }));
            Ui.Btn(this, "Làm mới", 750, 16, 100, (s, e) =>
            {
                txtTim.Clear();
                Reload();
                Clear();
            });

            Ui.Lbl(this, "Tìm theo tên / điện thoại", 420, 70);
            txtTim = Ui.Txt(this, 420, 95, 320);
            Ui.Btn(this, "Tìm kiếm", 750, 91, 100, (s, e) => Ui.Run(Reload));

            grid = Ui.Grid(this, 20, 170, 865, 395);
            grid.SelectionChanged += (s, e) =>
            {
                var r = grid.CurrentRow;
                if (r == null || r.DataBoundItem == null) return;
                selId = Convert.ToInt32(r.Cells["MaKH"].Value);
                txtTen.Text = Convert.ToString(r.Cells["TenKH"].Value);
                txtSdt.Text = Convert.ToString(r.Cells["DienThoai"].Value);
                txtDiem.Text = Convert.ToString(r.Cells["DiemTichLuy"].Value);
            };

            Load += (s, e) => Ui.Run(Reload);
        }

        private void Reload()
        {
            grid.DataSource = bus.Search(txtTim.Text);
        }

        private void Clear()
        {
            selId = 0;
            txtTen.Clear();
            txtSdt.Clear();
            txtDiem.Clear();
        }

        private KhachHangDTO Read()
        {
            return new KhachHangDTO { MaKH = selId, TenKH = txtTen.Text, DienThoai = txtSdt.Text };
        }
    }
}
