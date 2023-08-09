namespace Store.Entities
{
    public class ProductsRecal
    {
        public int Id { get; set; }
        public string Negocio  { get; set; }
        public string Description { get; set; }
        public string  TipoNegocio { get; set; }     
        public string CodigoBarra { get; set; }
        public string Marca { get; set; }
        public string Almacen { get; set; }
        //public string Modelo { get; set; }
        public string UM { get; set; }
        public int Existencia { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PVD { get; set; }
        public decimal PVM { get; set; }
    }
}
