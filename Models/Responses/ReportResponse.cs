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

    public class TrasladoResponse
    {
        public int Id { get; set; }
        public string Almacen { get; set; }
        public string Usuario { get; set; }
        public string Concepto { get; set; }
        public int ProductCount { get; set; }
        public decimal SumCostoCompra { get; set; }
        public decimal SumVentaMayor { get; set; }
        public decimal SumVentaDetalle { get; set; }
        public DateTime Fecha { get; set; }
    }
}
