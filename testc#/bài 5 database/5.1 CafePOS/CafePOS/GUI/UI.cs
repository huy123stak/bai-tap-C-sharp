namespace CafePOS.GUI;

internal static class UI
{
    public static Label Lbl(string text, int x, int y)
    {
        return new Label { Text = text, Left = x, Top = y + 4, AutoSize = true };
    }

    public static TextBox Txt(int x, int y, int w)
    {
        return new TextBox { Left = x, Top = y, Width = w };
    }

    public static Button Btn(string text, int x, int y, int w, EventHandler handler)
    {
        var b = new Button { Text = text, Left = x, Top = y, Width = w, Height = 30 };
        b.Click += handler;
        return b;
    }

    public static ComboBox Cmb(int x, int y, int w)
    {
        return new ComboBox { Left = x, Top = y, Width = w, DropDownStyle = ComboBoxStyle.DropDownList };
    }

    public static DataGridView Grid()
    {
        return new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            BackgroundColor = Color.White
        };
    }

    public static void Header(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col].HeaderText = text;
    }
}
