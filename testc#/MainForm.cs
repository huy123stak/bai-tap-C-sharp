using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai4_1_Calculator
{
    /// <summary>
    /// BAI 4.1: Minh hoa ky thuat EVENT SHARING.
    /// Tat ca cac nut so 0-9 (va dau cham) deu duoc gan CHUNG mot ham
    /// xu ly su kien duy nhat: btnNum_Click.
    /// Ben trong ham do, ta dung "sender" de biet chinh xac nut nao vua bam.
    /// </summary>
    public class MainForm : Form
    {
        private TextBox txtDisplay;
        private TableLayoutPanel tableButtons;

        // Bien luu trang thai tinh toan
        private double _accumulator = 0;
        private string _pendingOperator = null;
        private bool _startNewEntry = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Calculator - Event Sharing Demo";
            this.Size = new Size(320, 420);
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

            // Bo tri cac nut: hang cuoi chi co nut "C" (Clear)
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
                        // TAT CA cac nut so (0-9) va dau "." dung CHUNG 1 event handler
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
                        // Cac phep toan + - * / cung dung chung 1 handler khac
                        btn.Click += btnOperator_Click;
                    }

                    tableButtons.Controls.Add(btn, c, r);
                }
            }

            this.Controls.Add(tableButtons);
            this.Controls.Add(txtDisplay);
        }

        // ======================================================
        // HAM DUY NHAT xu ly Click cho TAT CA cac nut so 0-9 va "."
        // ======================================================
        private void btnNum_Click(object sender, EventArgs e)
        {
            // Ep kieu sender ve Button de biet nut nao vua duoc bam
            Button btn = sender as Button;
            if (btn == null) return;

            // Lay thuoc tinh Text cua nut de noi vao chuoi hien thi
            string digit = btn.Text;

            if (_startNewEntry)
            {
                txtDisplay.Text = (digit == ".") ? "0." : digit;
                _startNewEntry = false;
            }
            else
            {
                if (digit == "." && txtDisplay.Text.Contains(".")) return; // tranh 2 dau cham
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
                switch (_pendingOperator)
                {
                    case "+": _accumulator += current; break;
                    case "-": _accumulator -= current; break;
                    case "*": _accumulator *= current; break;
                    case "/":
                        if (current == 0)
                        {
                            MessageBox.Show("Khong the chia cho 0!");
                            _accumulator = 0;
                        }
                        else _accumulator /= current;
                        break;
                }
            }
            txtDisplay.Text = _accumulator.ToString();
        }
    }
}
