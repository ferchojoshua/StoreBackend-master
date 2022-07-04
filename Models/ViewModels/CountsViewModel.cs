namespace StoreBackend.Models.ViewModels
{
    public class AddCountViewModel
    {
        public string CountNumber { get; set; }
        public string Description { get; set; }
        public int IdCountGroup { get; set; }
    }

    public class UpdateCountViewModel
    {
        public int Id { get; set; }
        public string CountNumber { get; set; }
        public string Description { get; set; }
        public int IdCountGroup { get; set; }
    }
}
