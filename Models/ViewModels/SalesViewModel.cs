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
    }
}
