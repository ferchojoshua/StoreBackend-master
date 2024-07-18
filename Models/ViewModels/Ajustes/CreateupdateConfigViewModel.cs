using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels.Ajustes
{
    public class AjustesViewModel
    {
        
        public int Operacion { get; set; }
        public int? Id { get; set; }
        //[Required]
        public string Valor { get; set; }
        //[Required]
        public string Catalogo { get; set; }
        //[Required]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}





