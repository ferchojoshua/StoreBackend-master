namespace Store.Entities
{
    public class HistoricalReceivablesDocuments
    {
        public int Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int DiasAtraso { get; set; }
        public int FacturaId { get; set; }
        public string Almacen { get; set; }
        public string Cliente { get; set; }
        public decimal MontoVenta { get; set; }
        public decimal TotalAbonado { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Periodo { get; set; }
        public bool IsContado { get; set; }
        public bool IsAnulado { get; set; }
    }
}
