using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models.ViewModels.Ajustes
{
    public class AjustesgetListViewModel
    {
        public int Operacion { get; set; }
        public int Valor { get; set; }

        [NotMapped]
        public string Mensaje { get; set; }

        //public AjustesgetListViewModel(int operacion)
        //{{
        //{{
        //    Operacion = operacion;
        //}
    }
}
