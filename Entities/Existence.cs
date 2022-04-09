namespace Store.Entities
{
    public class Existence
    {
        public int Id { get; set; }
        public Almacen Almacen { get; set; }
        public Producto Producto { get; set; }
        public int Existencia { get; set; }
        public decimal PrecioVentaMayor { get; set; }
        public decimal PrecioVentaDetalle { get; set; }
    }
}
