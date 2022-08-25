using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class AddFacturacionViewModel
    {
        public bool IsEventual { get; set; }
        public string NombreCliente { get; set; }
        public int ClientId { get; set; }
        public decimal MontoVenta { get; set; }
        public ICollection<FacturacionDetailsViewModel> FacturacionDetails { get; set; }

        [Required]
        public bool IsContado { get; set; }

        [Required]
        public int StoreId { get; set; }

        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal DescuentoXMonto { get; set; }
        public string CodigoDescuento { get; set; }

        [Required]
        public decimal MontoVentaAntesDescuento { get; set; }
    }

    public class FacturacionDetailsViewModel
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Cantidad { get; set; }
        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal Descuento { get; set; }
        public string CodigoDescuento { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal PVM { get; set; }
        public decimal PVD { get; set; }
        public decimal CostoTotalAntesDescuento { get; set; }
        public decimal CostoTotalDespuesDescuento { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal CostoCompra { get; set; }
        public bool IsAnulado { get; set; }
        public string UserId { get; set; }
        public DateTime FechaAnulacion { get; set; }
    }

    public class PayFactViewModel
    {
        public int FacturaId { get; set; }
        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal DescuentoXMonto { get; set; }
        public string CodigoDescuento { get; set; }
        public decimal MontoVenta { get; set; }
        public decimal MontoVentaAntesDescuento { get; set; }
    }
}
