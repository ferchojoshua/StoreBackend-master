using System;
using System.ComponentModel.DataAnnotations;
using Store.Entities;

namespace Store.Models.ViewModels
{
    public class AddEntradaProductoViewModel
    {
        [Required]
        public string NoFactura { get; set; }

        [Required]
        public string TipoPago { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public decimal MontoFactura { get; set; }

        [Required]
        public List<ProductInDetails> ProductInDetails { get; set; }
    }

    public class UpdateEntradaProductoViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string NoFactura { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        [Required]
        public string TipoEntrada { get; set; }

        [Required]
        public string TipoPago { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public decimal MontoFactura { get; set; }

        [Required]
        public List<ProductInDetails> ProductInDetails { get; set; }
    }
}
