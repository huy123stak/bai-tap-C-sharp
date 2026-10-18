using System.ComponentModel;
using static RevenueAnalytics.UiHelper;

namespace RevenueAnalytics;

public class frmRevenueAnalytics : Form
{
    static readonly string[] Branches = { "Hà Nội", "Đà Nẵng", "TP. Hồ Chí Minh" };

    DateTimePicker dtpFrom, dtpTo;
    ComboBox cboBranch;
    Button btnAnalyze, btnCancel;
    ProgressBar pgbLoading;
    Label lblProcessingStatus, lblTotal;
    TextBox txtNote;
    DataGridView dgvRevenueData;
    CancellationTokenSource cts;

    public frmRevenueAnalytics()
    {
        Text = "Bảng điều khiển thống kê doanh thu";
        MinimumSize = new Size(850, 560);
        Size = new Size(1000, 640);
        StartPosition = FormStartPosition.CenterScreen;
        BuildUi();
        FormClosing += (s, e) => cts?.Cancel();
    }

    void BuildUi()
    {
        dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(-30) };
        dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today };
        cboBranch = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cboBranch.Items.Add("Tất cả");
        cboBranch.Items.AddRange(Branches);
        cboBranch.SelectedIndex = 0;
        btnAnalyze = Btn("Thống Kê Doanh Thu", btnAnalyze_Click, 170);
        btnCancel = Btn("Hủy Thao Tác", btnCancel_Click, 130);
        btnCancel.Enabled = false;

        var criteria = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        criteria.Controls.Add(Lbl("Từ ngày:"));
        criteria.Controls.Add(dtpFrom);
        criteria.Controls.Add(Lbl("Đến ngày:"));
        criteria.Controls.Add(dtpTo);
        criteria.Controls.Add(Lbl("Chi nhánh:"));
        criteria.Controls.Add(cboBranch);
        criteria.Controls.Add(btnAnalyze);
        criteria.Controls.Add(btnCancel);
        var gbCriteria = new GroupBox { Text = "Tiêu chí thống kê", Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        gbCriteria.Controls.Add(criteria);

        pgbLoading = new ProgressBar { Dock = DockStyle.Fill, Minimum = 0, Maximum = 100, Margin = new Padding(3, 8, 3, 3) };
        lblProcessingStatus = new Label { AutoSize = true, Text = "Sẵn sàng", Anchor = AnchorStyles.Left };
        txtNote = new TextBox { Width = 260 };

        var statusRow = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, AutoSize = true };
        statusRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        statusRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        statusRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        statusRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        statusRow.Controls.Add(pgbLoading, 0, 0);
        statusRow.Controls.Add(lblProcessingStatus, 1, 0);
        statusRow.Controls.Add(Lbl("Ghi chú (thử nhập khi đang xử lý):"), 2, 0);
        statusRow.Controls.Add(txtNote, 3, 0);

        dgvRevenueData = NewGrid();
        dgvRevenueData.Columns.Add(Col(nameof(RevenueTransaction.TransactionId), "Mã giao dịch", 14));
        dgvRevenueData.Columns.Add(Col(nameof(RevenueTransaction.Date), "Ngày", 14, "dd/MM/yyyy"));
        dgvRevenueData.Columns.Add(Col(nameof(RevenueTransaction.Branch), "Chi nhánh", 20));
        dgvRevenueData.Columns.Add(Col(nameof(RevenueTransaction.Customer), "Khách hàng", 28));
        var colAmount = Col(nameof(RevenueTransaction.Amount), "Doanh thu", 18, "N0");
        colAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvRevenueData.Columns.Add(colAmount);

        lblTotal = new Label { AutoSize = true, Text = "Tổng doanh thu: -", Font = new Font(Font, FontStyle.Bold), Margin = new Padding(3, 8, 3, 3) };

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(8) };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(gbCriteria, 0, 0);
        root.Controls.Add(statusRow, 0, 1);
        root.Controls.Add(dgvRevenueData, 0, 2);
        root.Controls.Add(lblTotal, 0, 3);
        Controls.Add(root);
    }

    async void btnAnalyze_Click(object sender, EventArgs e)
    {
        if (dtpFrom.Value.Date > dtpTo.Value.Date)
        {
            MessageBox.Show("Từ ngày không được lớn hơn Đến ngày.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        cts = new CancellationTokenSource();
        var token = cts.Token;
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        string branch = cboBranch.Text;

        btnAnalyze.Enabled = false;
        btnCancel.Enabled = true;
        pgbLoading.Value = 0;
        lblProcessingStatus.Text = "Đang xử lý... 0%";
        lblTotal.Text = "Tổng doanh thu: -";
        dgvRevenueData.DataSource = null;

        var progress = new Progress<int>(p =>
        {
            if (token.IsCancellationRequested) return;
            pgbLoading.Value = p;
            lblProcessingStatus.Text = $"Đang xử lý... {p}%";
        });

        try
        {
            var data = await Task.Run(() => BuildReport(from, to, branch, progress, token), token);
            dgvRevenueData.DataSource = new BindingList<RevenueTransaction>(data);
            pgbLoading.Value = 100;
            lblProcessingStatus.Text = $"Hoàn tất - {data.Count:N0} giao dịch";
            lblTotal.Text = $"Tổng doanh thu: {data.Sum(t => t.Amount):N0} đ";
        }
        catch (OperationCanceledException)
        {
            pgbLoading.Value = 0;
            lblProcessingStatus.Text = "Đã hủy thao tác!";
            MessageBox.Show("Đã hủy thao tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            pgbLoading.Value = 0;
            lblProcessingStatus.Text = "Lỗi xử lý";
            MessageBox.Show("Có lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            cts.Dispose();
            cts = null;
            btnAnalyze.Enabled = true;
            btnCancel.Enabled = false;
        }
    }

    void btnCancel_Click(object sender, EventArgs e)
    {
        cts?.Cancel();
    }

    static List<RevenueTransaction> BuildReport(DateTime from, DateTime to, string branch, IProgress<int> progress, CancellationToken token)
    {
        const int steps = 50;
        int days = (to - from).Days + 1;
        int total = Math.Clamp(days * 8, 8, 4000);
        int perStep = Math.Max(1, total / steps);
        var rnd = new Random();
        var result = new List<RevenueTransaction>(total);
        string[] customers = { "Nguyễn Văn An", "Trần Thị Bình", "Lê Hoàng Cường", "Phạm Thu Dung", "Hoàng Minh Em", "Vũ Quốc Phong", "Đặng Thị Giang" };

        for (int i = 1; i <= steps; i++)
        {
            token.ThrowIfCancellationRequested();
            Thread.Sleep(100);
            for (int k = 0; k < perStep; k++)
            {
                result.Add(new RevenueTransaction
                {
                    TransactionId = $"GD{result.Count + 1:00000}",
                    Date = from.AddDays(rnd.Next(days)),
                    Branch = branch == "Tất cả" ? Branches[rnd.Next(Branches.Length)] : branch,
                    Customer = customers[rnd.Next(customers.Length)],
                    Amount = rnd.Next(50, 5000) * 1000
                });
            }
            progress.Report(i * 100 / steps);
        }

        return result.OrderBy(t => t.Date).ThenBy(t => t.TransactionId).ToList();
    }
}
