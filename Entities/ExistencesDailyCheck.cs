namespace Store.Entities
{
    public class ExistencesDailyCheck
    {
        public int Id { get; set; }
        public Almacen Almacen { get; set; }
        public Producto Producto { get; set; }
        public int Existencia { get; set; }
        public DateTime Fecha { get; set; }
    }
}
