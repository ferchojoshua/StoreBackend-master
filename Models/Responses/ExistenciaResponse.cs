namespace Store.Models.Responses
{
    public class ExistenciaResponse
    {
        public int IdProducto { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public string Familia { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string TipoNegocio { get; set; }
        public string UM { get; set; }

        public ICollection<ExistenceDetail> Existence { get; set; }
    }

    public class ExistenceDetail
    {
        public int IdExistence { get; set; }
        public string Almacen { get; set; }
        public int Exisistencia { get; set; }
        public decimal PVD { get; set; }
        public decimal PVM { get; set; }
        public decimal PrecioCompra { get; set; }
    }
}
