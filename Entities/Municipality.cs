namespace Store.Entities
{
    public class Municipality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Department Department { get; set; }
    }
}
