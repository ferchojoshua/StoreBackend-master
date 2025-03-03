using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Entities;
using Store.Entities.Ajustes;
using Store.Entities.CreateupdateConfig;
using Store.Entities.Logo;
using Store.Entities.ProductoRecal;


namespace Store.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Abono> Abonos { get; set; }
        public DbSet<Almacen> Almacen { get; set; }
        public DbSet<CajaMovment> CajaMovments { get; set; }
        public DbSet<CajaTipo> CajaTipos { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Count> Counts { get; set; }
        public DbSet<CountAsientoContable> CountAsientosContables { get; set; }
        public DbSet<CountAsientoContableDetails> CountAsientoContableDetails { get; set; }
        public DbSet<CountCodeStructure> CountCodeStructures { get; set; }
        public DbSet<CountFuentesContables> CountFuentesContables { get; set; }
        public DbSet<CountGroup> CountGroups { get; set; }
        public DbSet<CountLibros> CountLibros { get; set; }
        public DbSet<CuentasXCobrarDailyCheck> CuentasXCobrarDailyChecks { get; set; }
        public DbSet<HistoricalReceivablesDocuments> HistoricalReceivablesDocuments { get; set; }
        public DbSet<Department> Departments { get; set; }

        // public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Existence> Existences { get; set; }
        public DbSet<ProductsRecal> ProductsRecal { get; set; }
        public DbSet<ExistencesDailyCheck> ExistencesDailyChecks { get; set; }
        public DbSet<Facturacion> Facturacions { get; set; }
 
        public DbSet<Proformas> GetProformasAsync { get; set; }
        public DbSet<FacturaDetails> FacturaDetails { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Kardex> Kardex { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ProductIn> ProductIns { get; set; }
        public DbSet<ProductInDetails> ProductInDetails { get; set; }
        public DbSet<ProductMovmentDetails> ProductMovmentDetails { get; set; }
        public DbSet<ProductMovments> ProductMovments { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Proformas> Proformas { get; set; }
        //public DbSet<ProformasDetail> ProformasDetails { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public DbSet<SaleAnulation> SaleAnulations { get; set; }
        public DbSet<SaleAnulationDetails> SaleAnulationDetails { get; set; }
        public DbSet<TipoNegocio> TipoNegocios { get; set; }
        public DbSet<TipoPago> TipoPagos { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<CreateLogo> C_Administrables { get; set; }
        public DbSet<Ajustes> Catalogs { get; set; }
        public DbSet<AjustesgetList> AjustesgetList { get; set; }
        public DbSet<GetProductslistEntity> GetProductslistEntity { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rol>().HasIndex(t => t.RoleName).IsUnique();

            // modelBuilder.Entity<Familia>().HasIndex(t => t.Description).IsUnique();

            modelBuilder.Entity<TipoNegocio>().HasIndex(t => t.Description).IsUnique();
            modelBuilder.Entity<ProductosInventario>().HasNoKey();
              // modelBuilder.Entity<Producto>().HasIndex(t => t.Description).IsUnique();
        }


    }
}
