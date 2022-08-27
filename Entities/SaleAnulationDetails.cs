namespace Store.Entities
{
    public class SaleAnulationDetails
    {
        public int Id { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public int CantidadAnulada { get; set; }
        public SaleDetail SaleDetailAfectado { get; set; }
    }
}
