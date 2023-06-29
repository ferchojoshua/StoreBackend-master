using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models.Responses;

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
            return await _context.Productos
                .Include(p => p.Familia)
                .Include(p => p.TipoNegocio)
                .Include(p => p.Existences)
                .ThenInclude(e => e.Almacen)
                .Select(
                    x =>
                        new ExistenciaResponse()
                        {
                            IdProducto = x.Id,
                            BarCode = x.BarCode,
                            Description = x.Description,
                            Familia = x.Familia.Description,
                            Marca = x.Marca,
                            Modelo = x.Modelo,
                            TipoNegocio = x.TipoNegocio.Description,
                            UM = x.UM,
                            Existence = x.Existences
                                .Select(
                                    e =>
                                        new ExistenceDetail()
                                        {
                                            IdExistence = e.Id,
                                            Almacen = e.Almacen.Name,
                                            Exisistencia = e.Existencia,
                                            PVD = e.PrecioVentaDetalle ,
                                            PVM = e.PrecioVentaMayor,
                                            PrecioCompra = e.PrecioCompra
                                        }
                                )
                                .ToList()
                        }
                )
                .ToListAsync();
        }
    }
}
