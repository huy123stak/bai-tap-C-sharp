namespace Bai4_8
{
    public abstract class ChildFormBase : Form
    {
        public event Action<string> StatusChanged;     
        public event Action<int> ProgressChanged;      

        protected void ReportStatus(string m) => StatusChanged?.Invoke(m);
        protected void ReportProgress(int p) => ProgressChanged?.Invoke(p);
    }
}
