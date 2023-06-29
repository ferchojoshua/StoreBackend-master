namespace Store.Entities
{
    public class CajaMovment
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Description { get; set; }
        public CajaTipo CajaTipo { get; set; }
        public decimal Entradas { get; set; }
        public decimal Salidas { get; set; }
        public decimal Saldo { get; set; }
        public User RealizadoPor { get; set; }
        public Almacen Store { get; set; }

    }
}
