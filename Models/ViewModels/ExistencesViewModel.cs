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
}
