namespace Store.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public Almacen Store { get; set; }
        public Producto Product { get; set; }
        public Sales Sales { get; set; }
        public int Cantidad { get; set; }
        public bool IsDescuento { get; set; }
        public decimal CostoCompra { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal Descuento { get; set; }
        public string CodigoDescuento { get; set; }
        public decimal Ganancia { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal PVM { get; set; }
        public decimal PVD { get; set; }
        public decimal CostoTotalAntesDescuento { get; set; }
        public decimal CostoTotalDespuesDescuento { get; set; }
        public decimal CostoTotal { get; set; }
        public bool IsAnulado { get; set; }
        public bool IsPartialAnulation { get; set; }
        public int CantidadAnulada { get; set; }
        public User AnulatedBy { get; set; }
        public DateTime FechaAnulacion { get; set; }
    }
}
