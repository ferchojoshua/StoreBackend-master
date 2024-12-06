using System.ComponentModel.DataAnnotations;
using Store.Entities;

namespace Store.Models.ViewModels
{
    public class AddProformasViewModel
    {
        [Required]
        public bool IsEventual { get; set; }

        public string NombreCliente { get; set; }

        public int IdClient { get; set; }

        [Required]
        public decimal MontoVenta { get; set; }


        [Required]
        public ICollection<ProformasDetails> ProformasDetails { get; set; }

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
}
