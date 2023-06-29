using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public int TipoNegocioId { get; set; }

        [Required]
        public int FamiliaId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BarCode { get; set; }

        [Required]
        public string Marca { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public string UM { get; set; }
    }

    public class GetKardexViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        public int StoreId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
