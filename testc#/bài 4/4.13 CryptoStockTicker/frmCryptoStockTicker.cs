using System.ComponentModel;
using static CryptoStockTicker.UiHelper;

namespace CryptoStockTicker;

public class frmCryptoStockTicker : Form
{
    const int MaxHistory = 500;

    readonly BindingList<PriceTick> history = new();
    readonly Random random = new();
    readonly object sync = new();
    decimal lastPrice = 65000m;
    int syncCount;
    CancellationTokenSource cts;

    Button btnStart, btnPause;
    Label lblBTCPrice;
    DataGridView dgvHistoryLog;
    ToolStripStatusLabel lblLastUpdate, lblSyncCount, lblState;

    public frmCryptoStockTicker()
    {
        Text = "Giám sát tỷ giá BTC/USD thời gian thực";
        MinimumSize = new Size(700, 520);
        Size = new Size(860, 620);
        StartPosition = FormStartPosition.CenterScreen;
        BuildUi();
        dgvHistoryLog.DataSource = history;
        FormClosing += (s, e) => cts?.Cancel();
    }

    void BuildUi()
    {
        btnStart = Btn("Bắt đầu Giám sát", btnStart_Click, 150);
        btnPause = Btn("Tạm dừng", btnPause_Click, 110);
        btnPause.Enabled = false;

        var controls = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        controls.Controls.Add(btnStart);
        controls.Controls.Add(btnPause);

        lblBTCPrice = new Label
        {
            Dock = DockStyle.Fill,
            AutoSize = false,
            Height = 80,
            Text = "BTC/USD: --",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 28F, FontStyle.Bold),
            ForeColor = SystemColors.ControlText
        };

        dgvHistoryLog = NewGrid();
        dgvHistoryLog.Columns.Add(Col(nameof(PriceTick.Time), "Thời gian", 18, "HH:mm:ss"));
        dgvHistoryLog.Columns.Add(Col(nameof(PriceTick.Price), "Giá (USD)", 22, "N2"));
        dgvHistoryLog.Columns.Add(Col(nameof(PriceTick.Change), "Biến động", 20, "+#,##0.00;-#,##0.00;0.00"));
        dgvHistoryLog.Columns.Add(Col(nameof(PriceTick.Percent), "%", 16, "+0.000%;-0.000%;0.000%"));
        dgvHistoryLog.Columns.Add(Col(nameof(PriceTick.Direction), "Xu hướng", 16));
        dgvHistoryLog.CellFormatting += (s, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= history.Count) return;
            var t = history[e.RowIndex];
            if (t.Direction == "Tăng") e.CellStyle.ForeColor = Color.ForestGreen;
            else if (t.Direction == "Giảm") e.CellStyle.ForeColor = Color.Firebrick;
        };

        var status = new StatusStrip();
        lblLastUpdate = new ToolStripStatusLabel("Cập nhật lần cuối: --:--:--");
        lblSyncCount = new ToolStripStatusLabel("Số lượt đồng bộ: 0");
        lblState = new ToolStripStatusLabel("Đã dừng") { Spring = true, TextAlign = ContentAlignment.MiddleRight };
        status.Items.AddRange(new ToolStripItem[] { lblLastUpdate, new ToolStripStatusLabel("|"), lblSyncCount, lblState });

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(8) };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(controls, 0, 0);
        root.Controls.Add(lblBTCPrice, 0, 1);
        root.Controls.Add(dgvHistoryLog, 0, 2);

        Controls.Add(root);
        Controls.Add(status);
    }

    void btnStart_Click(object sender, EventArgs e)
    {
        if (cts != null) return;
        cts = new CancellationTokenSource();
        var token = cts.Token;
        btnStart.Enabled = false;
        btnPause.Enabled = true;
        lblState.Text = "Đang giám sát...";
        Task.Run(() => MonitorLoop(token));
    }

    void btnPause_Click(object sender, EventArgs e)
    {
        cts?.Cancel();
        cts = null;
        btnStart.Enabled = true;
        btnPause.Enabled = false;
        lblState.Text = "Đã tạm dừng";
    }

    void MonitorLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            PriceTick tick;
            lock (sync)
            {
                decimal change = Math.Round((decimal)(random.NextDouble() * 400 - 200), 2);
                decimal newPrice = Math.Max(1000m, lastPrice + change);
                change = newPrice - lastPrice;
                tick = new PriceTick
                {
                    Time = DateTime.Now,
                    Price = newPrice,
                    Change = change,
                    Percent = lastPrice == 0 ? 0 : change / lastPrice,
                    Direction = change > 0 ? "Tăng" : change < 0 ? "Giảm" : "Đứng giá"
                };
                lastPrice = newPrice;
            }
            PushTick(tick);
            if (token.WaitHandle.WaitOne(2000)) break;
        }
    }

    void PushTick(PriceTick tick)
    {
        if (IsDisposed || !IsHandleCreated) return;

        if (InvokeRequired)
        {
            try
            {
                Invoke(new Action(() => PushTick(tick)));
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is InvalidOperationException)
            {
            }
            return;
        }

        lblBTCPrice.Text = $"BTC/USD: ${tick.Price:N2}";
        if (tick.Change > 0) lblBTCPrice.ForeColor = Color.ForestGreen;
        else if (tick.Change < 0) lblBTCPrice.ForeColor = Color.Firebrick;

        history.Insert(0, tick);
        while (history.Count > MaxHistory) history.RemoveAt(history.Count - 1);

        syncCount++;
        lblLastUpdate.Text = "Cập nhật lần cuối: " + tick.Time.ToString("HH:mm:ss");
        lblSyncCount.Text = "Số lượt đồng bộ: " + syncCount;
    }
}
