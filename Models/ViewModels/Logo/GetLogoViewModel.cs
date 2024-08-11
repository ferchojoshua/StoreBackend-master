namespace Store.Models.ViewModels
{
    public class GetLogoViewModel
    {
        public int StoreId { get; set; }
        public string Direccion { get; set; }
        public string Ruc { get; set; }
        public byte[] Imagen { get; set; } // Mantener el campo de la imagen en byte[]
        public string ImagenBase64 { get; set; } // Nuevo campo para la imagen en base64
        public string Telefono { get; set; }
        public string TelefonoWhatsApp { get; set; }

        }
}
