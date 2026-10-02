using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TechMart
{
    public partial class Form1 : Form
    {
        private BindingList<SanPham> dsSanPham = new BindingList<SanPham>();
        private BindingSource bindingSource = new BindingSource();

        private SanPham spDangChon = null;
        private string duongDanAnh = "";
        private bool dangLoc = false;
        private int demMa = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboCategory.DisplayMember = "Ten";
            cboCategory.ValueMember = "Ma";
            cboCategory.DataSource = DanhMuc.DanhSach;
            TaoCotGrid();

            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.DataSource = bindingSource;

            dsSanPham.Add(new SanPham { MaSP = "SP001", TenSP = "iPhone 15", MaDanhMuc = 1, DonGia = 22000000, SoLuong = 10 });
            dsSanPham.Add(new SanPham { MaSP = "SP002", TenSP = "Laptop Asus", MaDanhMuc = 2, DonGia = 18000000, SoLuong = 5 });
            dsSanPham.Add(new SanPham { MaSP = "SP003", TenSP = "Tai nghe Sony", MaDanhMuc = 3, DonGia = 1500000, SoLuong = 30 });

            TimKiem();
        }

        private void TaoCotGrid()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            colMaSP = new DataGridViewTextBoxColumn
            {
                Name = "colMaSP",
                HeaderText = "Ma SP",
                DataPropertyName = "MaSP",
                FillWeight = 15
            };

            colTenSP = new DataGridViewTextBoxColumn
            {
                Name = "colTenSP",
                HeaderText = "Ten SP",
                DataPropertyName = "TenSP",
                FillWeight = 35
            };

            colDanhMuc = new DataGridViewTextBoxColumn
            {
                Name = "colDanhMuc",
                HeaderText = "Danh Muc",
                DataPropertyName = "TenDanhMuc",
                FillWeight = 18
            };

            colDonGia = new DataGridViewTextBoxColumn
            {
                Name = "colDonGia",
                HeaderText = "Don Gia (VND)",
                DataPropertyName = "DonGia",
                FillWeight = 20
            };
            colDonGia.DefaultCellStyle.Format = "N0";
            colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            colSoLuong = new DataGridViewTextBoxColumn
            {
                Name = "colSoLuong",
                HeaderText = "So Luong",
                DataPropertyName = "SoLuong",
                FillWeight = 12
            };
            colSoLuong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvProducts.Columns.AddRange(new DataGridViewColumn[]
            {
                colMaSP, colTenSP, colDanhMuc, colDonGia, colSoLuong
            });
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CapNhatStatus()
        {
            lblTongSP.Text = "Tong so san pham: " + dsSanPham.Count;
        }

        private void TimKiem()
        {
            dangLoc = true;

            string tuKhoa = txtSearch.Text.Trim().ToLower();
            if (tuKhoa == "")
            {
                bindingSource.DataSource = dsSanPham;
            }
            else
            {
                var ketQua = dsSanPham.Where(x => x.TenSP.ToLower().Contains(tuKhoa)).ToList();
                bindingSource.DataSource = new BindingList<SanPham>(ketQua);
            }

            dgvProducts.ClearSelection();
            spDangChon = null;

            dangLoc = false;
            CapNhatStatus();
        }

        private string TaoMaMoi()
        {
            string ma;
            do
            {
                ma = "SP" + demMa.ToString("000");
                demMa++;
            }
            while (dsSanPham.Any(x => x.MaSP == ma));
            return ma;
        }

        private void HienAnh(string path)
        {
            if (picAvatar.Image != null)
            {
                picAvatar.Image.Dispose();
                picAvatar.Image = null;
            }

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;

            try
            {
                using (Bitmap tam = new Bitmap(path))
                {
                    picAvatar.Image = new Bitmap(tam);
                }
            }
            catch
            {
                MessageBox.Show("Khong doc duoc file anh!", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LamMoi()
        {
            txtProductId.Clear();
            txtProductId.ReadOnly = false;
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = 0;
            duongDanAnh = "";
            HienAnh("");
            errorProvider.Clear();
            spDangChon = null;
            dgvProducts.ClearSelection();
            txtProductName.Focus();
        }

        // Chuyen "25,000,000" / "25.000.000" / "25000000" thanh so, bat ke may dung dinh dang nao
        private static string LamSachSo(string s)
        {
            return s.Trim().Replace(",", "").Replace(".", "").Replace(" ", "");
        }

        private bool KiemTraHopLe(out decimal donGia, out int soLuong)
        {
            errorProvider.Clear();
            bool hopLe = true;
            donGia = 0;
            soLuong = 0;

            if (txtProductName.Text.Trim() == "")
            {
                errorProvider.SetError(txtProductName, "Ten san pham khong duoc de trong");
                hopLe = false;
            }

            if (!decimal.TryParse(LamSachSo(txtUnitPrice.Text), out donGia) || donGia <= 0)
            {
                errorProvider.SetError(txtUnitPrice, "Don gia phai la so lon hon 0");
                hopLe = false;
            }

            // De trong so luong thi coi nhu 0
            string sl = LamSachSo(txtQuantity.Text);
            if (sl != "")
            {
                if (!int.TryParse(sl, out soLuong) || soLuong < 0)
                {
                    errorProvider.SetError(txtQuantity, "So luong phai la so nguyen >= 0");
                    hopLe = false;
                }
            }

            return hopLe;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraHopLe(out decimal donGia, out int soLuong))
                return;

            string ma;
            if (spDangChon != null)
            {
                // Dang chon 1 dong tren grid (o Ma SP bi khoa) => them san pham MOI voi ma tu sinh,
                // tranh loi "ma da ton tai" khien khong them duoc.
                ma = TaoMaMoi();
            }
            else
            {
                ma = txtProductId.Text.Trim();
                if (ma == "")
                {
                    ma = TaoMaMoi();
                }
                else if (dsSanPham.Any(x => x.MaSP == ma))
                {
                    errorProvider.SetError(txtProductId, "Ma san pham da ton tai");
                    return;
                }
            }

            SanPham sp = new SanPham();
            sp.MaSP = ma;
            sp.TenSP = txtProductName.Text.Trim();
            sp.MaDanhMuc = (int)cboCategory.SelectedValue;
            sp.DonGia = donGia;
            sp.SoLuong = soLuong;
            sp.DuongDanAnh = duongDanAnh ?? "";   // anh KHONG bat buoc

            dsSanPham.Add(sp);
            TimKiem();
            LamMoi();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (spDangChon == null)
            {
                MessageBox.Show("Hay chon mot san pham tren bang de cap nhat!", "Thong bao",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!KiemTraHopLe(out decimal donGia, out int soLuong))
                return;

            spDangChon.TenSP = txtProductName.Text.Trim();
            spDangChon.MaDanhMuc = (int)cboCategory.SelectedValue;
            spDangChon.DonGia = donGia;
            spDangChon.SoLuong = soLuong;
            spDangChon.DuongDanAnh = duongDanAnh ?? "";

            SanPham dangSua = spDangChon;
            dangLoc = true;
            bindingSource.ResetBindings(false);
            dangLoc = false;

            int vt = bindingSource.IndexOf(dangSua);
            if (vt >= 0)
            {
                dgvProducts.ClearSelection();
                dgvProducts.Rows[vt].Selected = true;
            }
            spDangChon = dangSua;

            MessageBox.Show("Cap nhat thanh cong!", "Thong bao",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (spDangChon == null)
            {
                MessageBox.Show("Hay chon mot san pham de xoa!", "Thong bao",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult kq = MessageBox.Show("Ban co chac muon xoa san pham \"" + spDangChon.TenSP + "\"?",
                                              "Xac nhan xoa",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                dsSanPham.Remove(spDangChon);
                TimKiem();
                LamMoi();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chon anh san pham";
                ofd.Filter = "File anh|*.png;*.jpg;*.jpeg;*.bmp;*.gif|Tat ca file|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    duongDanAnh = ofd.FileName;
                    HienAnh(duongDanAnh);
                }
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dangLoc)
                return;
            if (dgvProducts.SelectedRows.Count == 0)
                return;

            SanPham sp = dgvProducts.SelectedRows[0].DataBoundItem as SanPham;
            if (sp == null)
                return;

            spDangChon = sp;
            txtProductId.Text = sp.MaSP;
            txtProductId.ReadOnly = true;
            txtProductName.Text = sp.TenSP;
            txtUnitPrice.Text = sp.DonGia.ToString("0");
            txtQuantity.Text = sp.SoLuong.ToString();
            cboCategory.SelectedValue = sp.MaDanhMuc;
            duongDanAnh = sp.DuongDanAnh ?? "";
            HienAnh(duongDanAnh);
            errorProvider.Clear();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private string Csv(string s)
        {
            if (s == null)
                return "";
            if (s.Contains(",") || s.Contains("\""))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        private void XuatCsv()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuat danh sach ra CSV";
                sfd.Filter = "File CSV (*.csv)|*.csv";
                sfd.FileName = "DanhSachSanPham.csv";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        sw.WriteLine("Ma SP,Ten SP,Danh Muc,Don Gia,So Luong");
                        foreach (SanPham sp in dsSanPham)
                        {
                            sw.WriteLine(Csv(sp.MaSP) + "," + Csv(sp.TenSP) + "," + Csv(sp.TenDanhMuc) + ","
                                         + sp.DonGia.ToString("0") + "," + sp.SoLuong);
                        }
                    }
                    MessageBox.Show("Xuat file thanh cong!", "Thong bao",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khong ghi duoc file: " + ex.Message, "Loi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            XuatCsv();
        }

        private void mnuExportCsv_Click(object sender, EventArgs e)
        {
            XuatCsv();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
