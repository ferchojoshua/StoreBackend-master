using Store.Entities;

namespace Store.Models.Responses
{
    public class GetSalesByDateResponse
    {
        public DateTime Fecha { get; set; }
        public decimal Contado { get; set; }
        public decimal Credito { get; set; }
        public decimal Recuperacion { get; set; }
    }
}
