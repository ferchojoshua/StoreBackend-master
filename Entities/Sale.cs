namespace Store.Entities
{
    public class Sales
    {
        public int Id { get; set; }
        public bool IsEventual { get; set; }
        public string NombreCliente { get; set; }
#nullable enable
        public Client? Client { get; set; }
#nullable disable
        public int ProductsCount { get; set; }

        //Representa el total de efectivo recibido
        public decimal MontoVenta { get; set; }
        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal DescuentoXMonto { get; set; }
        public decimal MontoVentaAntesDescuento { get; set; }
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
        public Almacen Store { get; set; }
        public TipoPago TipoPago { get; set; }
        public string CodigoDescuento { get; set; }
        public ICollection<Kardex> KardexMovments { get; set; }
        public string Reference { get; set; }
    }
}
