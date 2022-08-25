using Store.Entities;

namespace Store.Models.Responses
{
    public class DailyCloseResponse
    {
        public ICollection<Sales> SaleList { get; set; }
        public ICollection<SaleDetail> AnulatedSaleList { get; set; }
        public ICollection<Abono> AbonoList { get; set; }
    }
}
