namespace Store.Entities
{
    public class Sales
    {
        public int Id { get; set; }
        public bool IsEventual { get; set; }
        public string NombreCliente { get; set; }
        public Client? Client { get; set; }
        public int ProductsCount { get; set; }
        public decimal MontoVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public User FacturedBy { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; }
        public bool IsContado { get; set; }
        public bool IsCanceled { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool IsAnulado { get; set; }
        public User AnulatedBy { get; set; }
        public DateTime FechaAnulacion { get; set; }
    }
}
