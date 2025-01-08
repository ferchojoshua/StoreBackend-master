using Microsoft.EntityFrameworkCore;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public interface ISalesService
    {
        Task<ICollection<Sales>> GetContadoSalesByStoreAsync(int idStore);
        Task<ICollection<Proformas>> GetContadoSalesByProfAsync();
        Task<ICollection<Proformas>> GetContadoSalesByProfAsync(int idStore);
        Task<ICollection<Sales>> GetCreditoSalesByStoreAsync(int idStore);
        Task<ICollection<Sales>> GetAnulatedSalesByStoreAsync(int idStore);
      
        Task<ICollection<Sales>> GetdevolutionSalesByStoreAsync(int idStore);
        Task<Sales> AnularSaleforIdAsync(int id, Entities.User user);

        Task<Proformas> FinishSalesAsync(int Id, int tipoPagoId, Entities.User user);

        Task<ICollection<Abono>> GetQuoteListAsync(int id);
        //Task<ICollection<Abono>> GetProformaByIdAsync(int id, char action);
        Task<ICollection<GetSalesAndQuotesResponse>> GetSalesUncanceledByClientAsync(int idClient);

        Task<Sales> AnularSaleAsync(int id, Entities.User user);
        Task<Sales> AnularSaleParcialAsync(EditSaleViewModel model, Entities.User user);
        Task<Sales> AddSaleAsync(AddSaleViewModel model, Entities.User user);
        Task<bool> UpdateSaleProductsAsync(UpdateSaleDetailsViewModel model, Entities.User user);
        Task<Proformas> AddProformasAsync(AddProformasViewModel model, Entities.User user);

        Task<Proformas> DeleteProformAsync(int Id, Entities.User user);



        //Task<IEnumerable<Proformas>> ProformaListAsync(char action);

        Task<Proformas> ProformaUpdateAsync(
              int id,
              int action,
              int storeId,
              string nombreCliente,
              string detalle,
              //int cantidad,
              //decimal precioUnitario,
              decimal total,
              decimal montoTotal,
              DateTime fechaEmision,
              DateTime fechaVencimiento,
              bool proformaRealizada,
              bool proformaVencida
          );



        //Task<ProformaViewModel> GetProformaByIdAsync(id, action, NombreCliente, FechaEmision);

        Task<ICollection<Abono>> AddAbonoAsync(AddAbonoViewModel model, Entities.User user);
        Task<Abono> AddAbonoEspecificoAsync(AddAbonoEspecificoViewModel model, Entities.User user);
    }
}
