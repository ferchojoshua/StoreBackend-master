namespace Store.Entities.ProductoRecal
{
    public class GetProductslistEntity
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public string Familia { get; set; }
        public int Fid { get; set; } // New field for Familia Id
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string TipoNegocio { get; set; }
        public int TNId { get; set; } // New field for TipoNegocio Id
        public string UM { get; set; }
        public int? IdExistence { get; set; }
        public string Almacen { get; set; }
        public int AID { get; set; } // New field for Almacen Id
        public int Exisistencia { get; set; }
        public decimal PVD { get; set; }
        public decimal PVM { get; set; }
        public decimal PrecioCompra { get; set; }
    }
}
