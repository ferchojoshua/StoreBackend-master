using Store.Models.ViewModels;
using Store.Entities;
using Store.Models.Responses;
using Store.Entities.Ajustes;
using Store.Entities.CreateupdateConfig;

namespace Store.Helpers.CreateLogoHelper
{
    public interface ICreateLogoHelper
    {
        Task<string> CreateLogoAsync(int storeId, string direccion, string ruc, byte[] imagenBase64, string telefono, string telefonoWhatsApp);
        Task<string> UpdateLogoAsync(int storeId, string direccion, string ruc, byte[] imagenBase64, string telefono, string telefonoWhatsApp);
        Task<GetLogoViewModel> GetLogoByStoreIdAsync(int storeId);
        Task<IEnumerable<Ajustes>> AjustesAsync(int operacion, string valor, string catalogo, string descripcion, string usuario);

        Task<IEnumerable<Ajustes>> AjustesUpdateAsync(int? id, int operacion, string valor, string catalogo, string descripcion, bool? estado, string email);
        
        Task<IEnumerable<AjustesgetList>> ObtenerCatalogosAsync(int operacion);
       
    }
}
