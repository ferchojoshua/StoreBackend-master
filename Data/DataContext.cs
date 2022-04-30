using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Entities;

namespace Store.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Almacen> Almacen { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Existence> Existences { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Kardex> Kardex { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ProductIn> ProductIns { get; set; }
        public DbSet<ProductInDetails> ProductInDetails { get; set; }
        public DbSet<ProductMovments> ProductMovments { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<TipoNegocio> TipoNegocios { get; set; }
        public DbSet<UserSession> UserSession { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rol>().HasIndex(t => t.RoleName).IsUnique();

            // modelBuilder.Entity<Familia>().HasIndex(t => t.Description).IsUnique();

            modelBuilder.Entity<TipoNegocio>().HasIndex(t => t.Description).IsUnique();

            // modelBuilder.Entity<Producto>().HasIndex(t => t.Description).IsUnique();

        }
    }
}
