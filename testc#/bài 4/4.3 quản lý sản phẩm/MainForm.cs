using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bai4_3_ProductManager
{
    /// <summary>
    /// BAI 4.3: Minh hoa lien ket du lieu 2 chieu (Two-way Data Binding)
    /// giua BindingList&lt;ProductModel&gt; va DataGridView.
    /// </summary>
    public class MainForm : Form
    {
        private DataGridView dgvProducts;
        private BindingList<ProductModel> _products;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnRefreshPrice;

        public MainForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quan ly san pham - DataGridView 2 chieu";
            this.Size = new Size(650, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvProducts = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(610, 320),
                AutoGenerateColumns = false, // TAT tinh nang tu dong sinh cot
                AllowUserToAddRows = false
            };

            // Tu thiet lap cac cot tuong ung voi thuoc tinh cua ProductModel
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Ma SP",
                Width = 60
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Ten san pham",
                Width = 220
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Don gia",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "So luong",
                Width = 100
            });

            btnAdd = new Button { Text = "Them san pham", Location = new Point(10, 345), Width = 150 };
            btnAdd.Click += btnAdd_Click;

            btnDelete = new Button { Text = "Xoa dong dang chon", Location = new Point(170, 345), Width = 150 };
            btnDelete.Click += btnDelete_Click;

            btnRefreshPrice = new Button { Text = "Tang gia 10% (dong dau)", Location = new Point(330, 345), Width = 190 };
            btnRefreshPrice.Click += btnRefreshPrice_Click;

            this.Controls.Add(dgvProducts);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefreshPrice);
        }

        private void LoadData()
        {
            // Su dung BindingList thay vi List thong thuong
            _products = new BindingList<ProductModel>
            {
                new ProductModel { Id = 1, Name = "Ban phim co", Price = 850000, Quantity = 15 },
                new ProductModel { Id = 2, Name = "Chuot khong day", Price = 320000, Quantity = 30 },
                new ProductModel { Id = 3, Name = "Man hinh 24 inch", Price = 3200000, Quantity = 8 }
            };

            // Gan BindingList lam nguon du lieu (DataSource) cho DataGridView
            // => Tu day, giao dien va du lieu duoc dong bo 2 chieu
            dgvProducts.DataSource = _products;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int newId = _products.Count == 0 ? 1 : _products[^1].Id + 1;
            // Them vao BindingList -> DataGridView TU DONG hien them 1 dong moi
            _products.Add(new ProductModel { Id = newId, Name = "San pham moi", Price = 0, Quantity = 0 });
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null &&
                dgvProducts.CurrentRow.DataBoundItem is ProductModel item)
            {
                // Xoa khoi BindingList -> DataGridView TU DONG mat dong tuong ung
                _products.Remove(item);
            }
        }

        private void btnRefreshPrice_Click(object sender, EventArgs e)
        {
            // Minh hoa chieu con lai cua binding: sua du lieu TU CODE,
            // DataGridView se tu cap nhat gia tri hien thi ngay lap tuc.
            if (_products.Count > 0)
            {
                _products[0].Price *= 1.1m;
            }
        }
    }
}
