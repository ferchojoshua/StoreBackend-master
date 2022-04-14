using Store.Entities;

namespace Store.Models.Responses
{
    public class ProductMovementsResponse
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public Almacen AlmacenProcedencia { get; set; }
        public Almacen AlmacenDestino { get; set; }
        public int Cantidad { get; set; }
        public string Concepto { get; set; }
        public User User { get; set; }
        public DateTime Fecha { get; set; }
    }
}
 