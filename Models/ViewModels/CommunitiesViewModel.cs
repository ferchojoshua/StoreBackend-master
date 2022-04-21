using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class AddCommunityViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int IdMunicipality { get; set; }
    }
}