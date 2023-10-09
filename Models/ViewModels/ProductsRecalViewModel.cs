using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class ProductRecalViewModel
    {
        [Required]
        public int TipoNegocioId { get; set; }

        [Required]
        public int FamiliaId { get; set; }

        public int StoreId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public bool Actualizado { get; set; }
    }
}
