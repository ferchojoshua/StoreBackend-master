using System;

namespace Store.Entities
{
    public class ProductMovments
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public User User { get; set; }
        public DateTime Fecha { get; set; }
    }
}
