using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class UpdateProductViewModel
    {

        [Required]
        public int Id { get; set; }

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
}
