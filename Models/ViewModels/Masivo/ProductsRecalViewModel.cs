using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels.Masivo
{
    public class ProductRecalViewModel
    {
        [Required]
        public int Id { get; set; }

        public int? StoreId { get; set; }

        [Required]
        public int Porcentaje { get; set; }
    }
}
