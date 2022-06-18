using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.ProductExistenceService
{
    public class ProdExistService : IProdExistService
    {
        private readonly DataContext _context;

        public ProdExistService(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ExistenciaResponse>> GetProductExistencesAsync()
        {
            List<ExistenciaResponse> eRList = new();
            var productList = await _context.Productos
                .Include(p => p.Familia)
                .Include(p => p.TipoNegocio)
                .ToListAsync();
            foreach (var item in productList)
            {
                ExistenciaResponse e = new();
                var exist = await _context.Existences.Where(e => e.Producto == item).ToListAsync();

                List<ExistenceDetail> eDList = new();
                foreach (var itemDetail in exist)
                {
                    ExistenceDetail eD =
                        new()
                        {
                            IdExistence = itemDetail.Id,
                            Almacen = itemDetail.Almacen.Name,
                            Exisistencia = itemDetail.Existencia,
                            PVD = itemDetail.PrecioVentaDetalle,
                            PVM = itemDetail.PrecioVentaMayor
                        };
                    eDList.Add(eD);
                }
                e = new()
                {
                    IdProducto = item.Id,
                    BarCode = item.BarCode,
                    Description = item.Description,
                    Familia = item.Familia == null ? "" : item.Familia.Description,
                    Marca = item.Marca,
                    Modelo = item.Modelo,
                    TipoNegocio = item.TipoNegocio == null ? "" : item.TipoNegocio.Description,
                    UM = item.UM,
                    Existence = eDList
                };
                eRList.Add(e);
            }
            return eRList;
        }
    }
}
