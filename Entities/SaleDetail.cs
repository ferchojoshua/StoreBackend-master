namespace Store.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public Almacen Store { get; set; }
        public Producto Product { get; set; }
        public int Cantidad { get; set; }
        public int Descuento { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal PVM { get; set; }
        public decimal PVD { get; set; }
        public decimal CostoTotal { get; set; }
        public bool IsAnulado { get; set; }
    }
}
