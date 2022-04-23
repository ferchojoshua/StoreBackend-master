using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class AddClientViewModel
    {
        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string Cedula { get; set; }

        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public int IdCommunity { get; set; }

        [Required]
        public string Direccion { get; set; }
    }

    public class UpdateClientViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string Cedula { get; set; }

        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public int IdCommunity { get; set; }

        [Required]
        public string Direccion { get; set; }
    }
}
