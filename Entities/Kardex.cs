namespace Store.Entities
{
    public class Kardex
    {
        public int Id { get; set; }
        public Producto Product { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public Almacen Almacen { get; set; }
        public int Entradas { get; set; }
        public int Salidas { get; set; }
        public int Saldo { get; set; }
        public User User { get; set; }
#nullable enable
        public ProductIn? EntradaProduct { get; set; }
#nullable disable
#nullable enable
        public Sales? Sale { get; set; }
#nullable disable
#nullable enable
        public ProductMovments? TrasladoInventario { get; set; }
#nullable disable
#nullable enable
        public SaleAnulation? SaleAnulation { get; set; }
#nullable disable
#nullable enable
        public StockAdjustment? AjusteInventario { get; set; }
#nullable disable
    }
}
