namespace Store.Entities
{
    public class ProductIn
    {
        public int Id { get; set; }

        public string TipoEntrada { get; set; }

        public string TipoPago { get; set; }

        public string NoFactura { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public Provider Provider { get; set; }

        public Almacen Almacen { get; set; }

        public decimal MontoFactura { get; set; }

        public List<ProductInDetails> ProductInDetails { get; set; }
    }
}
