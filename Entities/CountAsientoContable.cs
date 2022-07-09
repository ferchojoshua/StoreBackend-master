namespace Store.Entities
{
    public class CountAsientoContable
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; }
        public CountLibros LibroContable { get; set; }
        public ICollection<CountAsientoContableDetails> CountAsientoContableDetails { get; set; }
        public CountFuentesContables FuenteContable { get; set; }
        public Almacen Store { get; set; }
        public User User { get; set; }
    }
}
