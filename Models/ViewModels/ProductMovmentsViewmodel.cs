using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class AddProductMovementViewModel
    {
        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int AlmacenProcedenciaId { get; set; }

        [Required]
        public int AlmacenDestinoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public string Concepto { get; set; }
    }
}
