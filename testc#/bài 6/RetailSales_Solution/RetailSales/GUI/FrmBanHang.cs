using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using RetailSales.BUS;
using RetailSales.DTO;

namespace RetailSales.GUI
{
    public class FrmBanHang : Form
    {
        private readonly ComboBox cbNV, cbKH, cbSP;
        private readonly TextBox txtMaVach;
        private readonly NumericUpDown nudSL;
        private readonly DataGridView grid;
        private readonly Label lblTong, lblDiem;
        private readonly DataTable cart = new DataTable();
        private readonly SanPhamBUS spBus = new SanPhamBUS();
        private readonly KhachHangBUS khBus = new KhachHangBUS();
        private readonly NhanVienBUS nvBus = new NhanVienBUS();
        private readonly HoaDonBUS hdBus = new HoaDonBUS();

        public FrmBanHang()
        {
            Text = "Bán hàng / Lập hóa đơn";
            Width = 1020;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10);

            cart.Columns.Add("MaSP", typeof(string));
            cart.Columns.Add("TenSP", typeof(string));
            cart.Columns.Add("SoLuong", typeof(int));
            cart.Columns.Add("DonGia", typeof(decimal));
            cart.Columns.Add("ThanhTien", typeof(decimal));

            Ui.Lbl(this, "Nhân viên", 20, 22);
            cbNV = new ComboBox { Left = 100, Top = 18, Width = 230, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cbNV);

            Ui.Lbl(this, "Khách hàng", 360, 22);
            cbKH = new ComboBox { Left = 450, Top = 18, Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            cbKH.SelectedIndexChanged += (s, e) => CapNhatTong();
            Controls.Add(cbKH);

            Ui.Lbl(this, "Mã vạch", 20, 67);
            txtMaVach = Ui.Txt(this, 100, 63, 160);
            txtMaVach.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                Ui.Run(() =>
                {
                    string ma = txtMaVach.Text.Trim();
                    if (ma.Length == 0) return;
                    cbSP.SelectedValue = ma;
                    if (cbSP.SelectedIndex < 0 || !string.Equals(Convert.ToString(cbSP.SelectedValue), ma, StringComparison.OrdinalIgnoreCase))
                        throw new Exception("Không tìm thấy sản phẩm có mã " + ma + ".");
                    ThemGio();
                    txtMaVach.Clear();
                });
            };

            Ui.Lbl(this, "Sản phẩm", 290, 67);
            cbSP = new ComboBox { Left = 370, Top = 63, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };
            Controls.Add(cbSP);

            Ui.Lbl(this, "Số lượng", 710, 67);
            nudSL = new NumericUpDown { Left = 785, Top = 63, Width = 60, Minimum = 1, Maximum = 10000, Value = 1 };
            Controls.Add(nudSL);

            Ui.Btn(this, "Thêm vào giỏ", 860, 60, 120, (s, e) => Ui.Run(ThemGio));

            grid = Ui.Grid(this, 20, 110, 960, 400);
            grid.DataSource = cart;

            lblTong = Ui.Lbl(this, "Tổng tiền: 0 VNĐ", 20, 535);
            lblTong.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblDiem = Ui.Lbl(this, "Điểm tích lũy được cộng: 0", 20, 575);
            lblDiem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            var b1 = Ui.Btn(this, "Xóa dòng", 560, 540, 120, (s, e) => Ui.Run(XoaDong));
            var b2 = Ui.Btn(this, "Làm mới giỏ", 690, 540, 120, (s, e) =>
            {
                cart.Rows.Clear();
                CapNhatTong();
            });
            var b3 = Ui.Btn(this, "Thanh toán", 820, 540, 160, (s, e) => Ui.Run(ThanhToan));
            foreach (var b in new[] { b1, b2, b3 })
            {
                b.Height = 44;
                b.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            }

            Load += (s, e) => Ui.Run(() =>
            {
                LoadNV();
                LoadKH();
                LoadSP();
            });
        }

        private void LoadNV()
        {
            cbNV.DataSource = nvBus.GetAll();
            cbNV.DisplayMember = "TenNV";
            cbNV.ValueMember = "MaNV";
        }

        private void LoadKH()
        {
            DataTable dt = khBus.GetAll();
            DataRow r = dt.NewRow();
            r["MaKH"] = 0;
            r["TenKH"] = "Khách lẻ";
            dt.Rows.InsertAt(r, 0);
            cbKH.DataSource = dt;
            cbKH.DisplayMember = "TenKH";
            cbKH.ValueMember = "MaKH";
            cbKH.SelectedIndex = 0;
        }

        private void LoadSP()
        {
            cbSP.DataSource = spBus.GetAll();
            cbSP.DisplayMember = "TenSP";
            cbSP.ValueMember = "MaSP";
        }

        private int? MaKHChon()
        {
            if (cbKH.SelectedValue == null) return null;
            int ma = Convert.ToInt32(cbKH.SelectedValue);
            if (ma == 0) return null;
            return ma;
        }

        private decimal TongTien()
        {
            decimal tong = 0;
            foreach (DataRow r in cart.Rows) tong += Convert.ToDecimal(r["ThanhTien"]);
            return tong;
        }

        private void CapNhatTong()
        {
            decimal tong = TongTien();
            lblTong.Text = "Tổng tiền: " + tong.ToString("N0") + " VNĐ";
            if (MaKHChon().HasValue)
                lblDiem.Text = "Điểm tích lũy được cộng: " + HoaDonBUS.TinhDiem(tong);
            else
                lblDiem.Text = "Khách lẻ: không tích điểm";
        }

        private void ThemGio()
        {
            if (cbSP.SelectedValue == null) throw new Exception("Vui lòng chọn sản phẩm.");
            string ma = Convert.ToString(cbSP.SelectedValue);
            int sl = (int)nudSL.Value;
            SanPhamDTO sp = spBus.GetById(ma);
            if (sp == null) throw new Exception("Sản phẩm không tồn tại.");

            DataRow row = null;
            foreach (DataRow r in cart.Rows)
            {
                if ((string)r["MaSP"] == ma)
                {
                    row = r;
                    break;
                }
            }

            int daCo = row == null ? 0 : (int)row["SoLuong"];
            if (daCo + sl > sp.SoLuongTon)
                throw new Exception("Không đủ tồn kho. Sản phẩm " + sp.TenSP + " chỉ còn " + sp.SoLuongTon + ".");

            if (row == null)
            {
                cart.Rows.Add(ma, sp.TenSP, sl, sp.GiaBan, sp.GiaBan * sl);
            }
            else
            {
                row["SoLuong"] = daCo + sl;
                row["DonGia"] = sp.GiaBan;
                row["ThanhTien"] = sp.GiaBan * (daCo + sl);
            }
            CapNhatTong();
        }

        private void XoaDong()
        {
            if (grid.CurrentRow == null) throw new Exception("Vui lòng chọn dòng cần xóa.");
            int idx = grid.CurrentRow.Index;
            if (idx < 0 || idx >= cart.Rows.Count) return;
            cart.Rows.RemoveAt(idx);
            CapNhatTong();
        }

        private void ThanhToan()
        {
            if (cart.Rows.Count == 0) throw new Exception("Giỏ hàng đang trống.");
            if (cbNV.SelectedValue == null) throw new Exception("Vui lòng chọn nhân viên lập hóa đơn.");
            if (!Ui.Confirm("Xác nhận thanh toán " + TongTien().ToString("N0") + " VNĐ?")) return;

            var hd = new HoaDonDTO
            {
                MaNV = Convert.ToInt32(cbNV.SelectedValue),
                MaKH = MaKHChon()
            };
            var ds = new List<ChiTietHoaDonDTO>();
            foreach (DataRow r in cart.Rows)
            {
                ds.Add(new ChiTietHoaDonDTO
                {
                    MaSP = (string)r["MaSP"],
                    SoLuong = (int)r["SoLuong"],
                    DonGia = (decimal)r["DonGia"]
                });
            }

            try
            {
                int maHD = hdBus.ThanhToan(hd, ds);
                Ui.Info("Thanh toán thành công. Mã hóa đơn: " + maHD);
                cart.Rows.Clear();
                LoadKH();
                LoadSP();
                CapNhatTong();
            }
            catch (Exception ex)
            {
                Ui.Err("Giao dịch thất bại, toàn bộ thao tác đã được Rollback.\n" + ex.Message);
                LoadSP();
            }
        }
    }
}
