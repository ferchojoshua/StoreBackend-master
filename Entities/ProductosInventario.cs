namespace Store.Entities
{
    public class ProductosInventario
    {
        public int Id { get; set; }
        public string nombre_producto { get; set; }
        public string nombre_almacen { get; set; }
        public int existencia { get; set; }
        public decimal precio_detalle { get; set; }
        public decimal total_detalle { get; set; }
        public decimal precio_xmayor { get; set; }
        public decimal total_mayor { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal costo_total { get; set; }
    }
}
