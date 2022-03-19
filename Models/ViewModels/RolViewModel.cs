using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class AddRolViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public ICollection<AddPermissonsViewModel> Permissions { get; set; }
    }

    public class AddPermissonsViewModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsEnable { get; set; }
    }
}
