namespace Store.Entities
{
    public class ProductImportResult
    {
        public int SuccessCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<Producto> ProcessedProducts { get; set; } = new List<Producto>();
    }
}
