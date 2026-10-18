using static HospitalMdi.UiHelper;

namespace HospitalMdi;

public abstract class ListFormBase : Form
{
    protected readonly DataGridView dgv = NewGrid();

    protected ListFormBase(string title)
    {
        Text = title;
        Size = new Size(720, 420);
        Controls.Add(dgv);
    }

    protected void AddColumn(string prop, string header, float weight = 10, string format = null)
    {
        dgv.Columns.Add(Col(prop, header, weight, format));
    }

    public virtual void RefreshData()
    {
    }
}
