using System.ComponentModel.DataAnnotations;

namespace Store.Models.ViewModels
{
    public class MasterVentasViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public bool ContadoSales { get; set; }

        [Required]
        public bool CreditSales { get; set; }
    }

    public class CuentasXCobrarViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public int ClientId { get; set; }
    }

    public class ArtVendidosViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public int TipoNegocioId { get; set; }

        [Required]
        public int FamiliaId { get; set; }

        [Required]
        public int ClientId { get; set; }
    }

    public class CierreDiarioViewModel
    {
        [Required]
        public string FechaDesde { get; set; }

        [Required]
        public string FechaHasta { get; set; }

        [Required]
        public string HoraDesde { get; set; }

        [Required]
        public string HoraHasta { get; set; }

        [Required]
        public int StoreId { get; set; }
    }

    public class CajaChicaViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int StoreId { get; set; }
    }
}
