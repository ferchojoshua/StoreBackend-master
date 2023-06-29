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

    public class GetClientsLocationsResponse
    {
        public Municipality Municipality { get; set; }
        public int Count { get; set; }
    }
}
