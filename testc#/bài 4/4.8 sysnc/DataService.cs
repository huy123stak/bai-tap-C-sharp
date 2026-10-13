namespace Bai4_8
{
    public static class DataService
    {
        private const int Steps = 100;

        public static async Task<List<Student>> LoadStudentsAsync(IProgress<int> progress = null)
        {
            return await Task.Run(() =>
            {
                var rnd = new Random();
                var list = new List<Student>();
                for (int i = 1; i <= Steps; i++)
                {
                    Thread.Sleep(40);   
                    for (int k = 0; k < 5; k++)
                    {
                        int n = list.Count + 1;
                        list.Add(new Student
                        {
                            StudentId = $"SV{n:0000}",
                            FullName = "Sinh viên " + n,
                            Age = rnd.Next(18, 26),
                            Gender = n % 2 == 0 ? "Nữ" : "Nam"
                        });
                    }
                    progress?.Report(i * 100 / Steps);
                }
                return list;
            });
        }

        public static async Task<List<Product>> LoadProductsAsync(IProgress<int> progress = null)
        {
            var rnd = new Random();
            var list = new List<Product>();
            for (int i = 1; i <= Steps; i++)
            {
                await Task.Delay(30);   
                for (int k = 0; k < 3; k++)
                {
                    int n = list.Count + 1;
                    list.Add(new Product { ProductId = $"SP{n:0000}", ProductName = "Sản phẩm " + n, Price = rnd.Next(1, 500) * 1000 });
                }
                progress?.Report(i * 100 / Steps);
            }
            return list;
        }
    }
}
