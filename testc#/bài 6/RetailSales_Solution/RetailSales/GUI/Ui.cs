using System;
using System.Windows.Forms;

namespace RetailSales.GUI
{
    public static class Ui
    {
        public static Label Lbl(Control p, string text, int x, int y)
        {
            var l = new Label { Text = text, Left = x, Top = y, AutoSize = true };
            p.Controls.Add(l);
            return l;
        }

        public static TextBox Txt(Control p, int x, int y, int w = 180)
        {
            var t = new TextBox { Left = x, Top = y, Width = w };
            p.Controls.Add(t);
            return t;
        }

        public static Button Btn(Control p, string text, int x, int y, int w, EventHandler h)
        {
            var b = new Button { Text = text, Left = x, Top = y, Width = w, Height = 32 };
            b.Click += h;
            p.Controls.Add(b);
            return b;
        }

        public static DataGridView Grid(Control p, int x, int y, int w, int h)
        {
            var g = new DataGridView
            {
                Left = x,
                Top = y,
                Width = w,
                Height = h,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            p.Controls.Add(g);
            return g;
        }

        public static void Run(Action a)
        {
            try
            {
                a();
            }
            catch (Exception ex)
            {
                Err(ex.Message);
            }
        }

        public static void Err(string msg)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Info(string msg)
        {
            MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool Confirm(string msg)
        {
            return MessageBox.Show(msg, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
