namespace Store.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public TipoNegocio TipoNegocio { get; set; }
        public Familia Familia { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string UM { get; set; }
    }
}
