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
    }

    public class AddAbonoViewModel
    {
        [Required]
        public int IdClient { get; set; }

        [Required]
        public int IdStore { get; set; }

        [Required]
        public decimal Monto { get; set; }
    }

    public class AddAbonoEspecificoViewModel
    {
        [Required]
        public int IdSale { get; set; }

        [Required]
        public decimal Monto { get; set; }
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
}
