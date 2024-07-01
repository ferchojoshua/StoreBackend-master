using Store.Models.ViewModels;

namespace Store.Helpers.CreateLogoHelperList
{
    public interface ICreateLogoHelperList
    {
        Task<GetLogoViewModel> GetLogoByStoreIdAsync(int storeId);
    }
}
