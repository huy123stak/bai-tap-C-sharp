using System;
using System.Drawing;
using System.Windows.Forms;
using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FrmNhanVien : Form
    {
        private readonly TextBox txtTen, txtSdt, txtTim;
        private readonly ComboBox cbChucVu;
        private readonly DataGridView grid;
        private readonly NhanVienBUS bus = new NhanVienBUS();
        private int selId;

        public FrmNhanVien()
        {
            Text = "Quản lý Nhân viên";
            Width = 920;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            Ui.Lbl(this, "Tên nhân viên", 20, 22);
            txtTen = Ui.Txt(this, 150, 18, 220);
            Ui.Lbl(this, "Số điện thoại", 20, 57);
            txtSdt = Ui.Txt(this, 150, 53, 220);
            Ui.Lbl(this, "Chức vụ", 20, 92);
            cbChucVu = new ComboBox { Left = 150, Top = 88, Width = 220, DropDownStyle = ComboBoxStyle.DropDown };
            cbChucVu.Items.AddRange(new object[] { "Thu ngân", "Quản lý", "Thủ kho", "Bảo vệ" });
            Controls.Add(cbChucVu);

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
                if (selId <= 0) throw new Exception("Vui lòng chọn nhân viên cần xóa.");
                if (!Ui.Confirm("Bạn có chắc muốn xóa nhân viên này?")) return;
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
                selId = Convert.ToInt32(r.Cells["MaNV"].Value);
                txtTen.Text = Convert.ToString(r.Cells["TenNV"].Value);
                txtSdt.Text = Convert.ToString(r.Cells["SoDienThoai"].Value);
                cbChucVu.Text = Convert.ToString(r.Cells["ChucVu"].Value);
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
            cbChucVu.Text = "";
        }

        private NhanVienDTO Read()
        {
            return new NhanVienDTO { MaNV = selId, TenNV = txtTen.Text, SoDienThoai = txtSdt.Text, ChucVu = cbChucVu.Text };
        }
    }
}
