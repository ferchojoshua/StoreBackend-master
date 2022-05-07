namespace Store.Entities
{
    public class AnuladaSales
    {
        public int Id { get; set; }
        public Sales Sale { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public User AnuledBy { get; set; }
    }
}
