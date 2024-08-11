using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels.Logo
{
    public class UpdateProductRecallViewModel
    {
        [Required]
        public int Id { get; set; }
        public int StoreId { get; set; }
        [Required]
        public int Porcentaje { get; set; }
        public bool ActualizarVentaDetalle { get; set; }
        public bool ActualizarVentaMayor { get; set; }
    }
}
