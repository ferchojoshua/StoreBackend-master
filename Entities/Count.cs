namespace Store.Entities
{
    public class Count
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public CountGroup CountGroup { get; set; }
        public string CountNumber { get; set; }
        public bool IsActive { get; set; }
        public CountCodeStructure Clasificacion { get; set; }
    }
}
