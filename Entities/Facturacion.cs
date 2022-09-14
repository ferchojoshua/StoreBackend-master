namespace Store.Entities
{
    public class Facturacion
    {
        public int Id { get; set; }
        public bool IsEventual { get; set; }
        public string NombreCliente { get; set; }
#nullable enable
        public Client? Client { get; set; }
#nullable disable
        public int ProductsCount { get; set; }

        //Representa el total de efectivo arecibir
        public decimal MontoVenta { get; set; }
        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal DescuentoXMonto { get; set; }
        public decimal MontoVentaAntesDescuento { get; set; }
        public DateTime FechaVenta { get; set; }
        public User FacturedBy { get; set; }
        public User PaidBy { get; set; }
        public ICollection<FacturaDetails> FacturaDetails { get; set; }
        public bool IsContado { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsAnulado { get; set; }
        public User AnulatedBy { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public Almacen Store { get; set; }
        public string CodigoDescuento { get; set; }

#nullable enable
        public Sales? Sale { get; set; }
#nullable disable
    }
}
