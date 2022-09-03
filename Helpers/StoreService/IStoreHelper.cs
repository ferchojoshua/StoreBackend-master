using Store.Entities;

namespace Store.Helpers.StoreService
{
    public interface IStoreHelper
    {
        Task<Almacen> UpdateStoreAsync(Almacen model);
    }
}
