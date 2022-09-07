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

    public class UpdateCommunityViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class UpdateMunicipalityViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Abreviatura { get; set; }
    }
}
