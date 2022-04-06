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
    }
}
