namespace Store.Entities
{
    public class ProductMovments
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Concepto { get; set; }
        public DateTime Fecha { get; set; }
        public ICollection<ProductMovmentDetails> MovmentDetails { get; set; }
    }
}
