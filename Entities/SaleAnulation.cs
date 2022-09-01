namespace Store.Entities
{
    public class SaleAnulation
    {
        public int Id { get; set; }
        public Sales VentaAfectada { get; set; }
        public decimal MontoAnulado { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public User AnulatedBy { get; set; }
        public ICollection<SaleAnulationDetails> SaleAnulationDetails { get; set; }
        public Almacen Store { get; set; }
    }
}
