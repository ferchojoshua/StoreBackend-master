using System.ComponentModel.DataAnnotations;
using Store.Entities;

namespace Store.Models.ViewModels
{
    public class AddClientViewModel
    {
        [Required]
        public string NombreCliente { get; set; }

        public string NombreComercial { get; set; }

        [Required]
        public string Cedula { get; set; }

        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public int IdCommunity { get; set; }

        [Required]
        public int IdStore { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public decimal CreditLimit { get; set; }   

        [Required]
        public int Valor { get; set; }
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

        [Required]
        public decimal CreditLimit { get; set; }

        [Required]
        public int Valor { get; set; }
    }

    public class GetRouteClientViewModel
    {
        [Required]
        public ICollection<Municipality> MunicipalityList { get; set; }
    }
}
