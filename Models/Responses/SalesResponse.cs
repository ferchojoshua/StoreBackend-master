using Store.Entities;

namespace Store.Models.Responses
{
    public class GetSalesAndQuotesResponse
    {
        public Sales Sale { get; set; }
        public ICollection<Abono> Abonos { get; set; }
    }
}
