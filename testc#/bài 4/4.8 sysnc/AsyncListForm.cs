using System.ComponentModel;

namespace Bai4_8
{
    // Form con dùng chung: nút Nạp dữ liệu + ProgressBar + Label % + DataGridView
    public abstract class AsyncListForm<T> : ChildFormBase
    {
        protected readonly DataGridView dgv = new() { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
            AutoGenerateColumns = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly Button btnLoad = new() { Text = "Nạp dữ liệu", Left = 10, Top = 12, Width = 110, Height = 30 };
        private readonly ProgressBar progressBar = new() { Left = 135, Top = 14, Width = 330, Height = 26, Minimum = 0, Maximum = 100 };
        private readonly Label lblPercent = new() { Left = 475, Top = 19, AutoSize = true, Text = "0%" };

        protected abstract Task<List<T>> LoadAsync(IProgress<int> progress);
        protected abstract string Title { get; }

        protected AsyncListForm()
        {
            Size = new Size(720, 450);
            var top = new Panel { Dock = DockStyle.Top, Height = 54 };
            top.Controls.AddRange(new Control[] { btnLoad, progressBar, lblPercent });
            Controls.Add(dgv);
            Controls.Add(top);
            btnLoad.Click += BtnLoad_Click;
        }

        private async void BtnLoad_Click(object sender, EventArgs e)
        {
            btnLoad.Enabled = false;                       // khóa nút khi đang chạy
            progressBar.Value = 0;
            lblPercent.Text = "0%";

            // Progress<int> được tạo trên UI thread -> callback tự chạy lại trên UI thread
            var progress = new Progress<int>(p =>
            {
                progressBar.Value = p;
                lblPercent.Text = p + "%";
                ReportProgress(p);                         // cập nhật % lên StatusStrip của Form cha
            });

            try
            {
                ReportStatus($"Đang nạp dữ liệu {Title}...");
                List<T> data = await LoadAsync(progress);
                dgv.DataSource = new BindingList<T>(data);
                ReportStatus($"Nạp xong {data.Count} bản ghi {Title}");
                MessageBox.Show($"Hoàn tất! Đã nạp {data.Count} bản ghi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ReportStatus("Lỗi khi nạp dữ liệu");
                MessageBox.Show("Có lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoad.Enabled = true;                    // mở lại nút dù thành công hay lỗi
                ReportProgress(-1);
            }
        }
    }
}
