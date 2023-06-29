using Store.Entities;

namespace Store.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Fecha { get; set; }
        public string Modulo { get; set; }
        public User RealizadoPor { get; set; }
    }
}
