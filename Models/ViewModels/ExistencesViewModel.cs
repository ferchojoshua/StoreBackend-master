namespace Store.Models.ViewModels
{
    public class GetExistencesViewModel
    {
        public int IdProduct { get; set; }

        public int IdAlmacen { get; set; }
    }

    public class GetExistencesByStoreViewModel
    {
        public int IdAlmacen { get; set; }
    }

    public class UpdateExistencesByStoreViewModel
    {
        public int Id { get; set; }
        public int NewExistencias { get; set; }
        public decimal NewPVD { get; set; }
        public decimal NewPVM { get; set; }
    }
}
