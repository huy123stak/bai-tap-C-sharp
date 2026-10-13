namespace Bai4_7
{
    public abstract class ChildFormBase : Form
    {
        public event Action<string> StatusChanged;
        protected void ReportStatus(string message) => StatusChanged?.Invoke(message);

        public string CurrentUser { get; set; }

        public virtual void RefreshData() { }
    }
}
