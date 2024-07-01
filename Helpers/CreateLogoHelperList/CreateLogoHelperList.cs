using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Store.Helpers.CreateLogoHelperList
{
    public class CreateLogoHelperList : ICreateLogoHelperList
    {
        private readonly DataContext _context;

        public CreateLogoHelperList(DataContext context)
        {
            _context = context;
        }

        public async Task<GetLogoViewModel> GetLogoByStoreIdAsync(int storeId)
        {
            try
            {
                // Busca el registro en la base de datos
                var existingAdmin = await _context.C_Administrables
                    .FirstOrDefaultAsync(a => a.StoreId == storeId);

                // Si no se encuentra ningún registro, retorna null
                if (existingAdmin == null)
                {
                    return null;
                }

                // Mapea los datos del modelo de entidad al ViewModel
                var viewModel = new GetLogoViewModel
                {
                    StoreId = existingAdmin.StoreId,
                    Direccion = existingAdmin.Direccion,
                    Ruc = existingAdmin.Ruc,
                    Imagen = existingAdmin.Imagen != null ? Convert.ToBase64String(existingAdmin.Imagen) : null,
                    Telefono = existingAdmin.Telefono,
                    TelefonoWhatsApp = existingAdmin.TelefonoWhatsApp
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                // Lanza una excepción con un mensaje detallado
                throw new Exception($"Error al obtener el logo para el storeId {storeId}: {ex.Message}");
            }
        }
    }
}
