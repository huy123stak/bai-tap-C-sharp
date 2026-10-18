namespace BookManager;

public static class UiHelper
{
    public static Label Lbl(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Anchor = AnchorStyles.Left,
        Margin = new Padding(3, 6, 8, 6)
    };

    public static Button Btn(string text, EventHandler handler, int width = 100)
    {
        var b = new Button { Text = text, Width = width, Height = 28 };
        b.Click += handler;
        return b;
    }

    public static DataGridViewTextBoxColumn Col(string prop, string header, float weight = 10, string format = null)
    {
        var c = new DataGridViewTextBoxColumn { DataPropertyName = prop, HeaderText = header, FillWeight = weight };
        if (format != null) c.DefaultCellStyle.Format = format;
        return c;
    }

    public static DataGridView NewGrid() => new()
    {
        Dock = DockStyle.Fill,
        AutoGenerateColumns = false,
        AllowUserToAddRows = false,
        ReadOnly = true,
        MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    };
}
