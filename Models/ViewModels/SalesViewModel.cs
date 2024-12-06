using System.ComponentModel.DataAnnotations;
using Store.Entities;

namespace Store.Models.ViewModels
{
    public class AddSaleViewModel
    {
        [Required]
        public bool IsEventual { get; set; }

        public string NombreCliente { get; set; }

        public int IdClient { get; set; }

        [Required]
        public decimal MontoVenta { get; set; }

        [Required]
        public ICollection<SaleDetail> SaleDetails { get; set; }    

        //[Required]
        //public ICollection<ProformasDetail> ProformasDetail { get; set; }

        [Required]
        public bool IsContado { get; set; }

        [Required]
        public int Storeid { get; set; }

        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal DescuentoXMonto { get; set; }

        public string CodigoDescuento { get; set; }

        [Required]
        public decimal MontoVentaAntesDescuento { get; set; }

        public int TipoPagoId { get; set; }
        public string Reference { get; set; }
    }






    public class AddAbonoViewModel
    {
        [Required]
        public int IdClient { get; set; }

        [Required]
        public int IdStore { get; set; }

        [Required]
        public int IdTipoPago { get; set; }

        [Required]
        public decimal Monto { get; set; }
        public string Reference { get; set; }
    }

    public class AddAbonoEspecificoViewModel
    {
        [Required]
        public int IdSale { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public int IdTipoPago { get; set; }
        public string Reference { get; set; }
    }

    public class EditSaleViewModel
    {
        [Required]
        public int IdSale { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public decimal Saldo { get; set; }

        [Required]
        public ICollection<SaleDetail> SaleDetails { get; set; }
    }

    public class UpdateSaleDetailsViewModel
    {
        public int Id { get; set; }
        public List<SaleDetailViewModel> ProformasDetails { get; set; }
    }

    public class SaleDetailViewModel
    {
        public int ProductId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CostoTotal { get; set; }
        public bool IsDescuento { get; set; }
        public decimal DescuentoXPercent { get; set; }
        public decimal Descuento { get; set; }
        public string CodigoDescuento { get; set; }
        public decimal PVM { get; set; }
        public decimal PVD { get; set; }
        public decimal CostoTotalAntesDescuento { get; set; }
        public decimal CostoTotalDespuesDescuento { get; set; }
        public decimal CostoCompra { get; set; }
    }
    public class FinishSalesViewModel
    {
        public int Id { get; set; }
        public int TipoPagoId { get; set; }
        public string Reference { get; set; }
    }
}
