namespace Store.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

#nullable enable
        public string? Email { get; set; }
#nullable disable
    }
}
