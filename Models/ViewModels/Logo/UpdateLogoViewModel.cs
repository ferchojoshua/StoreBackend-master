using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels.Logo
{
    public class UpdateLogoViewModel
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Ruc { get; set; }

        [Required]
        public IFormFile Imagen { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string TelefonoWhatsApp { get; set; }




    }
}
