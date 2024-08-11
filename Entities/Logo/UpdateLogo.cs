namespace Store.Entities.Logo
{
    public class UpdateLogo
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Direccion { get; set; }
        public string Ruc { get; set; }
        public byte[] Imagen { get; set; }
        public string Telefono { get; set; }
        public string TelefonoWhatsApp { get; set; }


    }

}

