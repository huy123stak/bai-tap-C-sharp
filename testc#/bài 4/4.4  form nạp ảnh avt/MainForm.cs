using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Bai4_4_AvatarCsv
{
    /// <summary>
    /// BAI 4.4: Tuong tac voi he thong tep tin thong qua OpenFileDialog va SaveFileDialog.
    /// </summary>
    public class MainForm : Form
    {
        private PictureBox pictureBoxAvatar;
        private Button btnChooseImage;
        private Button btnExportCsv;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Nap anh Avatar & Xuat file CSV";
            this.Size = new Size(420, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            pictureBoxAvatar = new PictureBox
            {
                Location = new Point(110, 20),
                Size = new Size(180, 180),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom // Anh khong bi bien dang
            };

            btnChooseImage = new Button
            {
                Text = "Chon anh Avatar...",
                Location = new Point(110, 210),
                Width = 180
            };
            btnChooseImage.Click += btnChooseImage_Click;

            btnExportCsv = new Button
            {
                Text = "Xuat du lieu ra CSV...",
                Location = new Point(110, 250),
                Width = 180
            };
            btnExportCsv.Click += btnExportCsv_Click;

            // ----- OpenFileDialog: chi hien thi cac dinh dang anh pho bien -----
            openFileDialog1 = new OpenFileDialog
            {
                Title = "Chon anh dai dien",
                Filter = "Anh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Tat ca file (*.*)|*.*"
            };

            // ----- SaveFileDialog: luu file dang .csv -----
            saveFileDialog1 = new SaveFileDialog
            {
                Title = "Luu file CSV",
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = "csv",
                FileName = "danh_sach.csv"
            };

            this.Controls.Add(pictureBoxAvatar);
            this.Controls.Add(btnChooseImage);
            this.Controls.Add(btnExportCsv);
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Truyen duong dan file da chon vao thuoc tinh Image cua PictureBox
                pictureBoxAvatar.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Du lieu mau dang bang, san sang de ghi ra CSV
                string[] header = { "Id", "HoTen", "Tuoi" };
                string[][] rows =
                {
                    new[] { "1", "Nguyen Van A", "20" },
                    new[] { "2", "Tran Thi B", "22" },
                    new[] { "3", "Le Van C", "21" }
                };

                using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(string.Join(",", header));
                    foreach (var row in rows)
                    {
                        writer.WriteLine(string.Join(",", row));
                    }
                }

                MessageBox.Show("Xuat file CSV thanh cong!", "Thong bao",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
