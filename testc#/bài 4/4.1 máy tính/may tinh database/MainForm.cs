using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai4_1_Calculator
{
    /// <summary>
    /// BAI 4.1 (NANG CAP): Event Sharing + luu lich su tinh toan vao CSDL SQLite.
    /// - Moi phep tinh hoan tat (khi bam "=" hoac noi tiep phep toan) se duoc INSERT vao bang History.
    /// - Khi mo ung dung, lich su cu duoc doc len tu CSDL.
    /// - Nhap doi chuot vao 1 dong lich su: dua ket qua len man hinh.
    /// - Chon dong + nhan Delete: xoa dong do. Nut "Xoa lich su": xoa tat ca.
    /// </summary>
    public class MainForm : Form
    {
        private TextBox txtDisplay;
        private TableLayoutPanel tableButtons;

        // Thanh phan giao dien cho lich su
        private Panel pnlHistory;
        private Label lblHistory;
        private ListView lvHistory;
        private Button btnClearHistory;

        // Lop truy cap CSDL
        private readonly CalculatorDb _db;

        // Bien luu trang thai tinh toan
        private double _accumulator = 0;
        private string _pendingOperator = null;
        private bool _startNewEntry = true;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                _db = new CalculatorDb();
                LoadHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the mo CSDL: " + ex.Message,
                                "Loi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Calculator - Event Sharing + SQLite";
            this.Size = new Size(580, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            txtDisplay = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 20F),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0"
            };

            tableButtons = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5
            };
            for (int i = 0; i < 4; i++)
                tableButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            for (int i = 0; i < 5; i++)
                tableButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            string[,] layout = new string[5, 4]
            {
                { "7", "8", "9", "/" },
                { "4", "5", "6", "*" },
                { "1", "2", "3", "-" },
                { "0", ".", "=", "+" },
                { "C", "", "", "" }
            };

            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    string text = layout[r, c];
                    if (string.IsNullOrEmpty(text)) continue;

                    Button btn = new Button
                    {
                        Text = text,
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 14F),
                        Margin = new Padding(2)
                    };

                    if (char.IsDigit(text[0]) || text == ".")
                    {
                        // ===== EVENT SHARING =====
                        btn.Click += btnNum_Click;
                    }
                    else if (text == "=")
                    {
                        btn.Click += btnEquals_Click;
                    }
                    else if (text == "C")
                    {
                        btn.Click += btnClear_Click;
                    }
                    else
                    {
                        btn.Click += btnOperator_Click;
                    }

                    tableButtons.Controls.Add(btn, c, r);
                }
            }

            // ----- Panel lich su (ben phai) -----
            lvHistory = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false
            };
            lvHistory.Columns.Add("Phep tinh", 150);
            lvHistory.Columns.Add("Gio", 70);
            lvHistory.DoubleClick += lvHistory_DoubleClick;
            lvHistory.KeyDown += lvHistory_KeyDown;

            btnClearHistory = new Button
            {
                Text = "Xoa lich su",
                Dock = DockStyle.Bottom,
                Height = 34
            };
            btnClearHistory.Click += btnClearHistory_Click;

            lblHistory = new Label
            {
                Text = "Lich su (luu trong SQLite)",
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            pnlHistory = new Panel
            {
                Dock = DockStyle.Right,
                Width = 250
            };
            // Thu tu Add: Fill truoc, roi den Bottom/Top
            pnlHistory.Controls.Add(lvHistory);
            pnlHistory.Controls.Add(btnClearHistory);
            pnlHistory.Controls.Add(lblHistory);

            this.Controls.Add(tableButtons);
            this.Controls.Add(txtDisplay);
            this.Controls.Add(pnlHistory);
        }

        // ======================================================
        // CSDL: doc / ghi lich su
        // ======================================================
        private void LoadHistory()
        {
            lvHistory.Items.Clear();
            foreach (var item in _db.GetHistory())
                lvHistory.Items.Add(CreateListItem(item));
        }

        private ListViewItem CreateListItem(HistoryItem item)
        {
            var lvi = new ListViewItem(item.Expression + " = " + item.Result);
            lvi.SubItems.Add(item.CreatedAt.ToString("HH:mm:ss"));
            lvi.Tag = item; // giu lai Id de xoa / lay ket qua
            return lvi;
        }

        private void SaveToHistory(string expression, double result)
        {
            if (_db == null) return;
            try
            {
                long id = _db.AddHistory(expression, result);
                var item = new HistoryItem
                {
                    Id = id,
                    Expression = expression,
                    Result = result,
                    CreatedAt = DateTime.Now
                };
                // Chen len dau danh sach (moi nhat tren cung)
                lvHistory.Items.Insert(0, CreateListItem(item));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the luu lich su: " + ex.Message,
                                "Loi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lvHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lvHistory.SelectedItems.Count == 0) return;
            var item = (HistoryItem)lvHistory.SelectedItems[0].Tag;

            // Dua ket qua cu len man hinh de tinh tiep
            _accumulator = item.Result;
            _pendingOperator = null;
            _startNewEntry = true;
            txtDisplay.Text = item.Result.ToString();
        }

        private void lvHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || lvHistory.SelectedItems.Count == 0) return;

            try
            {
                var lvi = lvHistory.SelectedItems[0];
                _db.DeleteHistory(((HistoryItem)lvi.Tag).Id);
                lvHistory.Items.Remove(lvi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the xoa: " + ex.Message);
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            if (_db == null || lvHistory.Items.Count == 0) return;

            var confirm = MessageBox.Show("Xoa toan bo lich su?", "Xac nhan",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _db.ClearHistory();
                lvHistory.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Khong the xoa: " + ex.Message);
            }
        }

        // ======================================================
        // HAM DUY NHAT xu ly Click cho TAT CA cac nut so 0-9 va "."
        // ======================================================
        private void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string digit = btn.Text;

            if (_startNewEntry)
            {
                txtDisplay.Text = (digit == ".") ? "0." : digit;
                _startNewEntry = false;
            }
            else
            {
                if (digit == "." && txtDisplay.Text.Contains(".")) return;
                txtDisplay.Text += digit;
            }
        }

        private void btnOperator_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (!_startNewEntry)
            {
                CalculatePending();
            }

            _pendingOperator = btn.Text;
            _startNewEntry = true;
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            CalculatePending();
            _pendingOperator = null;
            _startNewEntry = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _accumulator = 0;
            _pendingOperator = null;
            _startNewEntry = true;
            txtDisplay.Text = "0";
        }

        private void CalculatePending()
        {
            if (!double.TryParse(txtDisplay.Text, out double current)) return;

            if (_pendingOperator == null)
            {
                _accumulator = current;
            }
            else
            {
                double left = _accumulator;
                string op = _pendingOperator;
                bool ok = true;

                switch (op)
                {
                    case "+": _accumulator += current; break;
                    case "-": _accumulator -= current; break;
                    case "*": _accumulator *= current; break;
                    case "/":
                        if (current == 0)
                        {
                            MessageBox.Show("Khong the chia cho 0!");
                            _accumulator = 0;
                            ok = false; // khong luu phep tinh loi
                        }
                        else _accumulator /= current;
                        break;
                }

                // ===== LUU VAO CSDL =====
                if (ok)
                    SaveToHistory($"{left} {op} {current}", _accumulator);
            }
            txtDisplay.Text = _accumulator.ToString();
        }
    }
}
