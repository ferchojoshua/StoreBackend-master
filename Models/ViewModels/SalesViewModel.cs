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
    }

    public class AddAbonoViewModel
    {
        [Required]
        public int IdSale { get; set; }

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
