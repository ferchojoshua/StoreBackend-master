using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class AddProductMovementViewModel
    {
        [Required]
        public string Concepto { get; set; }

        [Required]
        public ICollection<ProductMovmentDetailsViewModel> MovmentDetails { get; set; }
    }

    public class ProductMovmentDetailsViewModel
    {
        public int IdProducto { get; set; }
        public int AlmacenProcedenciaId { get; set; }
        public int AlmacenDestinoId { get; set; }
        public int Cantidad { get; set; }
    }
}
