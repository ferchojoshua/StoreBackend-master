namespace Store.Entities
{
    public class Almacen
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public ICollection<Rack> Racks { get; set; }

        //public int RacksNumber => Racks == null ? 0 : Racks.Count;
    }
}
