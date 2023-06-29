namespace Store.Entities
{
    public class StockAdjustment
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public User RealizadoPor { get; set; }
        public ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public ICollection<Kardex> KardexMovments { get; set; }
        public decimal MontoPrecioCompra { get; set; }
        public decimal MontoPrecioVenta { get; set; }
        public Almacen Store { get; set; }
    }
}
