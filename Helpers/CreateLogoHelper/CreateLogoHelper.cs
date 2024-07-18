using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities.Ajustes;
using Store.Entities.CreateupdateConfig;
using Store.Entities.Logo;
using Store.Models.ViewModels;
using System.Data;

namespace Store.Helpers.CreateLogoHelper
{
    public class CreateLogoHelper : ICreateLogoHelper
    {
        private readonly DataContext _context; 

        public CreateLogoHelper(DataContext context)
            {
              _context = context;
             }

        public async Task<string> CreateLogoAsync(int storeId, string direccion, string ruc, byte[] imagen, string telefono, string telefonoWhatsApp)
        {
            try
            {
                // Crear un nuevo objeto Admin y asignarle valores
                var admin = new CreateLogo
                {
                    //Nombre = nombre,
                    StoreId = storeId,
                    Direccion = direccion,
                    Ruc = ruc,
                    Imagen = imagen,
                    Telefono = telefono,
                    TelefonoWhatsApp = telefonoWhatsApp
                  
                };

                _context.C_Administrables.Add(admin);
                await _context.SaveChangesAsync();
                var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1);
                mensajeParam.Direction = ParameterDirection.Output;
                await _context.Database.ExecuteSqlInterpolatedAsync($@" SET @Mensaje = ''; 
                EXEC [dbo].[Create_Imagen] {admin.Id}, {storeId}, {direccion}, {ruc}, {imagen}, {telefono}, {telefonoWhatsApp}, @Mensaje OUTPUT; 
                SELECT @Mensaje;");

                string mensaje = mensajeParam.Value.ToString();
                return mensaje;
            }
            catch (Exception ex)
            {
                return $"Error al crear el logo: {ex.Message}";
            }
        }

        public async Task<IEnumerable<Ajustes>> AjustesAsync( int operacion, string valor, string catalogo, string descripcion, string usuario)
        {
            try
            {
                var mensaje = await uspCreateCatalogosList( operacion, valor, catalogo, descripcion, usuario);
                List<Ajustes> ajustesList = new List<Ajustes>();
                ajustesList.Add(new Ajustes { Mensaje = mensaje });

                return ajustesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<string> uspCreateCatalogosList(int operacion, string valor, string catalogo, string descripcion, string usuario)
        {
           try
            {
                // Define el comando SQL con el orden correcto de los parámetros
                var sqlCommand = "EXEC [dbo].[usp_CreateCatalogos] @Operacion, @Valor, @Catalogo, @Descripcion, @Usuario, @ID OUTPUT, @Estado OUTPUT, @Mensaje OUTPUT";

                var estadoParam = new SqlParameter("@Estado", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };

                var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };

                // Ejecuta el comando SQL de forma asíncrona
                await _context.Database.ExecuteSqlRawAsync(sqlCommand,
                    new SqlParameter("@Operacion", operacion), // Parámetro de entrada @Operacion
                    new SqlParameter("@Valor", (object)valor ?? DBNull.Value),
                    new SqlParameter("@Catalogo", (object)catalogo ?? DBNull.Value),
                    new SqlParameter("@Descripcion", (object)descripcion ?? DBNull.Value),
                    new SqlParameter("@Usuario", (object)usuario ?? DBNull.Value),
                    estadoParam,
                    mensajeParam
                );

                // Recupera y devuelve el valor del parámetro de salida @Mensaje
                string result = mensajeParam.Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la operación: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<Ajustes>> AjustesUpdateAsync(int? id, int operacion, string valor, string catalogo, string descripcion, bool? estado, string email)
        {
            try
            {
                var mensaje = await uspCreateCatalogosList(id, operacion, valor, catalogo, descripcion, estado, email);
                List<Ajustes> ajustesList = new List<Ajustes>
        {
            new Ajustes { Mensaje = mensaje }
        };

                return ajustesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> uspCreateCatalogosList(int? id, int operacion, string valor, string catalogo, string descripcion, bool? estado, string email)
        {
            if (operacion == 3 || operacion == 4)
            {
                if (!id.HasValue)
                {
                    throw new ArgumentException("El ID es necesario para las operaciones de actualización o eliminación.");
                }
            }

            try
            {
                var sqlCommand = "EXEC [dbo].[usp_CreateCatalogos] @Operacion, @Valor, @Catalogo, @Descripcion, @Usuario, @ID OUTPUT, @Estado OUTPUT, @Mensaje OUTPUT";

                var idParam = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Direction = operacion == 1 ? ParameterDirection.Output : ParameterDirection.InputOutput,
                    Value = id.HasValue ? (object)id.Value : DBNull.Value
                };

                var estadoParam = new SqlParameter("@Estado", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = estado.HasValue ? (object)estado.Value : DBNull.Value
                };

                var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(sqlCommand,
                    new SqlParameter("@Operacion", operacion),
                    new SqlParameter("@Valor", (object)valor ?? DBNull.Value),
                    new SqlParameter("@Catalogo", (object)catalogo ?? DBNull.Value),
                    new SqlParameter("@Descripcion", (object)descripcion ?? DBNull.Value),
                    new SqlParameter("@Usuario", (object)email ?? DBNull.Value),
                    idParam,
                    estadoParam,
                    mensajeParam
                );

                string result = mensajeParam.Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la operación: {ex.Message}");
                throw;
            }
        }


        public async Task<GetLogoViewModel> GetLogoByStoreIdAsync(int storeId)
        {
            try
            { 
           
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


        public async Task<IEnumerable<AjustesgetList>> ObtenerCatalogosAsync(int operacion)
        {
            try
            {
                if (operacion == 2)
                {
                    // Solo ejecuta la consulta para la operación 2 sin parámetros de salida
                    var result = await _context.Set<AjustesgetList>()
                        .FromSqlRaw("EXEC [dbo].[usp_CreateCatalogos] @Operacion", new SqlParameter("@Operacion", operacion))
                        .ToListAsync();

                    return result;
                }
                //else
                //{
                //    // Para las demás operaciones (1, 3, 4), incluye los parámetros de salida
                //    var idParam = new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                //    var estadoParam = new SqlParameter("@Estado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                //    var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };

                //    await _context.Database.ExecuteSqlRawAsync(
                //        "EXEC [dbo].[usp_CreateCatalogos] @Operacion, @Valor, @Catalogo, @Descripcion, @Usuario, @ID OUTPUT, @Estado OUTPUT, @Mensaje OUTPUT",
                //        new SqlParameter("@Operacion", operacion),
                //        new SqlParameter("@Valor", DBNull.Value), // O cualquier valor necesario
                //        new SqlParameter("@Catalogo", DBNull.Value), // O cualquier valor necesario
                //        new SqlParameter("@Descripcion", DBNull.Value), // O cualquier valor necesario
                //        new SqlParameter("@Usuario", DBNull.Value), // O cualquier valor necesario
                //        idParam,
                //        estadoParam,
                //        mensajeParam);

                //    // Leer los valores de salida
                //    var id = idParam.Value != DBNull.Value ? idParam.Value.ToString() : string.Empty;
                //    var estado = estadoParam.Value != DBNull.Value ? (bool)estadoParam.Value : false;
                //    var mensaje = mensajeParam.Value != DBNull.Value ? mensajeParam.Value.ToString() : string.Empty;

                //    Console.WriteLine($"ID: {id}, Estado: {estado}, Mensaje: {mensaje}");

                //    // Puedes retornar una lista vacía o cualquier otro valor según sea necesario
                //    return new List<AjustesgetList>();
                //}
            }
            
            catch (Exception ex)
            {
                // Manejo de excepciones si es necesario
                Console.WriteLine($"Error en ObtenerCatalogosAsync: {ex.Message}");
                throw;

            }
            return new List<AjustesgetList>();
        }

      



        //public async Task<IEnumerable<AjustesgetList>> ObtenerCatalogosAsync(int operacion)
        //{
        //    try
        //    {


        //        var operacionParam = new SqlParameter("@Operacion", operacion);
        //        //var idParam = new SqlParameter("@ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //        //var estadoParam = new SqlParameter("@Estado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
        //        //var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };

        //        var result = await _context.AjustesgetList
        //            .FromSqlRaw("EXEC [dbo].[usp_CreateCatalogos] @Operacion,  operacionParam")

        //            .ToListAsync();

        //        //var mensaje = mensajeParam.Value.ToString();

        //         return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}