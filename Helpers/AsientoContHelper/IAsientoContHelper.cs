using Store.Entities;
using StoreBackend.Models.ViewModels;

namespace Store.Helpers.AsientoContHelper
{
    public interface IAsientoContHelper
    {
        Task<CountAsientoContable> AddAsientoContable(
            AddAsientoContableViewModel model,
            Entities.User user
        );

        Task<ICollection<CountAsientoContable>> GetAsientoContableListAsync();
    }
}
