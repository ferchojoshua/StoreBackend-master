namespace Store.Entities
{
    public class Rack
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Almacen Almacen { get; set; }
    }
}
