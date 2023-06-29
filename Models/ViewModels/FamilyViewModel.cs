using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class FamilyViewModel
    {
        [Required]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
    }
}
