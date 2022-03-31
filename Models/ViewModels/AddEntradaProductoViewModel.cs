using Store.Entities;

namespace Store.Models.ViewModels
{
    public class AddEntradaProductoViewModel
    {
        public string NoFactura { get; set; }

        public string TipoEntrada { get; set; }

        public string TipoPago { get; set; }

        public int ProviderId { get; set; }

        public decimal MontoFactura { get; set; }

        public List<ProductInDetails> ProductInDetails { get; set; }
    }
}
