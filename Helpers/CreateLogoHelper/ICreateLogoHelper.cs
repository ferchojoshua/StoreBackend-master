using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;
using System.Threading.Tasks;

namespace Store.Helpers.CreateLogoHelper
{
    public interface ICreateLogoHelper
    {
        Task<string> CreateLogoAsync(int storeId, string direccion, string ruc, byte[] imagenBase64, string telefono, string telefonoWhatsApp);
    }
}
