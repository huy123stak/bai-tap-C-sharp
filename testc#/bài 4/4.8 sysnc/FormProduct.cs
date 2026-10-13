namespace Bai4_8
{
    public class FormProduct : AsyncListForm<Product>
    {
        public FormProduct() { Text = "Quản lý Sản phẩm (Async)"; }
        protected override string Title => "Sản phẩm";
        protected override Task<List<Product>> LoadAsync(IProgress<int> progress)
            => DataService.LoadProductsAsync(progress);
    }
}
