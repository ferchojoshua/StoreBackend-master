namespace Store.Entities
{
    public class CountMovment
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Count Count { get; set; }
        public Almacen Store { get; set; }
        public string Concepto { get; set; }
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
        public decimal Saldo { get; set; }
        public User User { get; set; }
    }
}
