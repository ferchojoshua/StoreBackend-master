using System.ComponentModel.DataAnnotations;
using Store.Entities;

namespace StoreBackend.Models.ViewModels
{
    public class AddAsientoContableViewModel
    {
        [Required]
        public string Referencia { get; set; }

        [Required]
        public int IdLibroContable { get; set; }

        [Required]
        public ICollection<AsientoContableDetailsViewModel> AsientoContableDetails { get; set; }

        [Required]
        public int IdFuenteContable { get; set; }

        [Required]
        public Almacen Store { get; set; }
    }

    public class AsientoContableDetailsViewModel
    {
        [Required]
        public Count Count { get; set; }

        [Required]
        public decimal Debito { get; set; }

        [Required]
        public decimal Credito { get; set; }

        [Required]
        public decimal Saldo { get; set; }
    }

    public class AddAtoContViewModel
    {
        [Required]
        public string Referencia { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int IdLibroContable { get; set; }

        [Required]
        public ICollection<AtoContDetailsViewModel> AsientoContableDetails { get; set; }

        [Required]
        public int IdFuenteContable { get; set; }

        [Required]
        public int StoreId { get; set; }
    }

    public class AtoContDetailsViewModel
    {
        [Required]
        public int CountId { get; set; }

        [Required]
        public decimal Debito { get; set; }

        [Required]
        public decimal Credito { get; set; }
    }
}
