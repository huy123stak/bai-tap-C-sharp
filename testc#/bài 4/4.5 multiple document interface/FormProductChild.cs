using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bai4_5_MdiShell
{
    /// <summary>
    /// Form con: Quan ly san pham (phien ban rut gon cua Bai 4.3),
    /// duoc mo ben trong FormMainMdi thong qua MdiParent.
    /// </summary>
    public class FormProductChild : Form
    {
        private DataGridView dgv;
        private BindingList<ProductItem> _items;

        public FormProductChild()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Quan ly san pham (Form con)";
            this.Size = new Size(500, 350);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Ten san pham",
                Width = 250
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Price",
                HeaderText = "Gia",
                Width = 150
            });

            _items = new BindingList<ProductItem>
            {
                new ProductItem { Name = "Ban phim", Price = 500000 },
                new ProductItem { Name = "Chuot", Price = 200000 }
            };
            dgv.DataSource = _items;

            this.Controls.Add(dgv);
        }
    }

    public class ProductItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
