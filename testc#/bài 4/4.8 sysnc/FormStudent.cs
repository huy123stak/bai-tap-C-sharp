namespace Bai4_8
{
    public class FormStudent : AsyncListForm<Student>
    {
        public FormStudent() { Text = "Quản lý Sinh viên (Async)"; }
        protected override string Title => "Sinh viên";
        protected override Task<List<Student>> LoadAsync(IProgress<int> progress)
            => DataService.LoadStudentsAsync(progress);
    }
}
