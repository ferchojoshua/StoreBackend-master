using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels.Logo
{
    public class UpdateProductRecallViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public int Porcentaje { get; set; }  
    }
}
