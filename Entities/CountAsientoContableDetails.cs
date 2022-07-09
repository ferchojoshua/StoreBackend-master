namespace Store.Entities
{
    public class CountAsientoContableDetails
    {
        public int Id { get; set; }
        public Count Cuenta { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public decimal Saldo { get; set; }
    }
}
