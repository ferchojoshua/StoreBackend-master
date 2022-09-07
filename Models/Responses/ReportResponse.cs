using Store.Entities;

namespace Store.Models.Responses
{
    public class ReportResponse
    {
        public string BarCode { get; set; }
        public string Producto { get; set; }
        public int CantidadVendida { get; set; }
        public decimal CostoCompra { get; set; }
        public decimal MontoVenta { get; set; }
        public decimal Utilidad { get; set; }
    }
    public class ProdNoVendidosResponse
    {
        public string BarCode { get; set; }
        public string Producto { get; set; }
        public int CantidadVendida { get; set; }
        public decimal CostoCompra { get; set; }
    }
}
