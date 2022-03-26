namespace Store.Entities
{
    public class TipoNegocio
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<Familia> Familias { get; set; }
    }
}
