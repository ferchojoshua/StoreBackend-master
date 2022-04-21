namespace Store.Entities
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Municipality Municipality { get; set; }
    }
}
