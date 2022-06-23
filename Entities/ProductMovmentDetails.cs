using System;

namespace Store.Entities
{
    public class ProductMovmentDetails
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public int AlmacenProcedenciaId { get; set; }
        public int AlmacenDestinoId { get; set; }
        public int Cantidad { get; set; }
       
    }
}
