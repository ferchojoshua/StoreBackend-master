using Store.Models.ViewModels;
using System.Threading.Tasks;

namespace Store.Helpers.CreateLogoHelperList
{
    public interface ICreateLogoHelperList
    {
        Task<GetLogoViewModel> GetLogoByStoreIdAsync(int storeId);
    }
}
