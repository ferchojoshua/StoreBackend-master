namespace Store.Models.ViewModels
{
    public class ProductDetailViewmodel
    {
        public int ProductId { get; set; }
        public int Cantidad { get; set; }

        public decimal CostoCompra { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal Descuento { get; set; }

        public decimal Impuesto { get; set; }

        public decimal PrecioVentaMayor { get; set; }

        public decimal PrecioVentaDetalle { get; set; }
    }
}
