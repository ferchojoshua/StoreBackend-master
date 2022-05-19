namespace Store.Entities
{
    public class Abono
    {
        public int Id { get; set; }
        public Sales Sale { get; set; }
        public decimal Monto { get; set; }
        public User RealizedBy { get; set; }
        public DateTime FechaAbono { get; set; }
        public bool IsAnulado { get; set; }
        public Almacen Store { get; set; }
    }
}
