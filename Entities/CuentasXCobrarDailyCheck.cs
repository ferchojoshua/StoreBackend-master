namespace Store.Entities
{
    public class CuentasXCobrarDailyCheck
    {
        public int Id { get; set; }
        public Almacen Almacen { get; set; }
        public Client Cliente { get; set; }
        public decimal MontoVenta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
