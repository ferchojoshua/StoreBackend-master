using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Helpers.CreateLogoHelper
{
    public class CreateLogoHelper : ICreateLogoHelper
    {
        private readonly DataContext _context; 

        public CreateLogoHelper(DataContext context)
            {
              _context = context;
             }

        public async Task<string> CreateLogoAsync(int storeId, string direccion, string ruc, byte[] imagen, string telefono, string telefonoWhatsApp)
        {
            try
            {
                // Crear un nuevo objeto Admin y asignarle valores
                var admin = new CreateLogo
                {
                    //Nombre = nombre,
                    StoreId = storeId,
                    Direccion = direccion,
                    Ruc = ruc,
                    Imagen = imagen,
                    Telefono = telefono,
                    TelefonoWhatsApp = telefonoWhatsApp
                  
                };

                _context.C_Administrables.Add(admin);
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC [dbo].[Create_Imagen] {admin.Id}, {storeId}, {direccion}, {ruc},{imagen},{telefono}, {telefonoWhatsApp}");

                return "Logo creado correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error al crear el logo: {ex.Message}";
            }
        }
    }
}