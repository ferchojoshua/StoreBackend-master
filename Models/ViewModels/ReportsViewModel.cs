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

    public class IngresosViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int StoreId { get; set; }
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

        [Required]
        public bool IncludeUncanceledSales { get; set; }
    }

    public class ArtNoVendidosViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int? StoreId { get; set; }

        [Required]
        public int TipoNegocioId { get; set; }

        [Required]
        public int FamiliaId { get; set; }
    }

    public class CierreDiarioViewModel
    {
        [Required]
        public string Desde { get; set; }

        [Required]
        public string Hasta { get; set; }

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

    public class ComprasViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public bool ContadoCompras { get; set; }

        [Required]
        public bool CreditCompras { get; set; }
    }

    public class TrasladoInventarioViewModel
    {
        [Required]
        public DateTime Desde { get; set; }

        [Required]
        public DateTime Hasta { get; set; }

        [Required]
        public int StoreId { get; set; }
    }
}
