using System;

namespace Store.Entities
{
    public class ProductMovments
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public int AlmacenProcedenciaId { get; set; }
        public int AlmacenDestinoId { get; set; }
        public int Cantidad { get; set; }
        public string Concepto { get; set; }
        public User User { get; set; }
        public DateTime Fecha { get; set; }
    }
}