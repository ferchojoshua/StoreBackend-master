namespace Store.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public Community Community { get; set; }
        public string Direccion { get; set; }
        public User CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime FechaEdicion { get; set; }
        public Almacen Store { get; set; }
    }
}
