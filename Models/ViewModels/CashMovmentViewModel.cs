namespace Store.Models.ViewModels
{
    public class AddCashMovmentViewModel
    {
        public int AlmacenId { get; set; }
        public decimal Monto { get; set; }
        public string Description { get; set; }
        public bool IsEntrada { get; set; }
    }
}
