namespace Store.Entities.Ajustes
{
    public class Ajustes
    { 
        public int? Id { get; set; }
        public int Valor { get; set; } 
        public string Catalogo { get; set; }
        public string Descripcion { get; set; }
        public bool? Estado { get; set; }
        public string Mensaje { get; set; }
    }
}
