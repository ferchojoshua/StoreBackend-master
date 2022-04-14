﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.Data;

#nullable disable

namespace Store.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Store.Entities.Almacen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Almacen");
                });

            modelBuilder.Entity("Store.Entities.Existence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AlmacenId")
                        .HasColumnType("int");

                    b.Property<int>("Existencia")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecioVentaDetalle")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrecioVentaMayor")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProductoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlmacenId");

                    b.HasIndex("ProductoId");

                    b.ToTable("Existences");
                });

            modelBuilder.Entity("Store.Entities.Familia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TipoNegocioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TipoNegocioId");

                    b.ToTable("Familias");
                });

            modelBuilder.Entity("Store.Entities.Kardex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AlmacenId")
                        .HasColumnType("int");

                    b.Property<string>("Concepto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Entradas")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Saldo")
                        .HasColumnType("int");

                    b.Property<int>("Salidas")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AlmacenId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Kardex");
                });

            modelBuilder.Entity("Store.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEnable")
                        .HasColumnType("bit");

                    b.Property<int?>("RolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Store.Entities.ProductIn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AlmacenId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EditBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EditDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaVencimiento")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MontoFactura")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("NoFactura")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProviderId")
                        .HasColumnType("int");

                    b.Property<string>("TipoEntrada")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoPago")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AlmacenId");

                    b.HasIndex("ProviderId");

                    b.ToTable("ProductIns");
                });

            modelBuilder.Entity("Store.Entities.ProductInDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<decimal>("CostoCompra")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CostoUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Descuento")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Impuesto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrecioVentaDetalle")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PrecioVentaMayor")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("ProductInId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductInId");

                    b.ToTable("ProductInDetails");
                });

            modelBuilder.Entity("Store.Entities.ProductMovments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AlmacenDestinoId")
                        .HasColumnType("int");

                    b.Property<int>("AlmacenProcedenciaId")
                        .HasColumnType("int");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("Concepto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProductoId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProductoId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductMovments");
                });

            modelBuilder.Entity("Store.Entities.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BarCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FamiliaId")
                        .HasColumnType("int");

                    b.Property<string>("Marca")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modelo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TipoNegocioId")
                        .HasColumnType("int");

                    b.Property<string>("UM")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FamiliaId");

                    b.HasIndex("TipoNegocioId");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Store.Entities.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("Store.Entities.Rack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AlmacenId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AlmacenId");

                    b.ToTable("Racks");
                });

            modelBuilder.Entity("Store.Entities.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleName")
                        .IsUnique()
                        .HasFilter("[RoleName] IS NOT NULL");

                    b.ToTable("Rols");
                });

            modelBuilder.Entity("Store.Entities.TipoNegocio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasFilter("[Description] IS NOT NULL");

                    b.ToTable("TipoNegocios");
                });

            modelBuilder.Entity("Store.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefaultPass")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("RolId")
                        .HasColumnType("int");

                    b.Property<string>("SecondLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("UserSessionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("RolId");

                    b.HasIndex("UserSessionId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Store.Entities.UserSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ExpirationDateToken")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserBrowser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserSO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserSession");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Store.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Store.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Store.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Store.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Store.Entities.Existence", b =>
                {
                    b.HasOne("Store.Entities.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("AlmacenId");

                    b.HasOne("Store.Entities.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId");

                    b.Navigation("Almacen");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Store.Entities.Familia", b =>
                {
                    b.HasOne("Store.Entities.TipoNegocio", null)
                        .WithMany("Familias")
                        .HasForeignKey("TipoNegocioId");
                });

            modelBuilder.Entity("Store.Entities.Kardex", b =>
                {
                    b.HasOne("Store.Entities.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("AlmacenId");

                    b.HasOne("Store.Entities.Producto", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("Store.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Almacen");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Store.Entities.Permission", b =>
                {
                    b.HasOne("Store.Entities.Rol", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RolId");
                });

            modelBuilder.Entity("Store.Entities.ProductIn", b =>
                {
                    b.HasOne("Store.Entities.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("AlmacenId");

                    b.HasOne("Store.Entities.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId");

                    b.Navigation("Almacen");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("Store.Entities.ProductInDetails", b =>
                {
                    b.HasOne("Store.Entities.Producto", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("Store.Entities.ProductIn", null)
                        .WithMany("ProductInDetails")
                        .HasForeignKey("ProductInId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Store.Entities.ProductMovments", b =>
                {
                    b.HasOne("Store.Entities.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId");

                    b.HasOne("Store.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Producto");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Store.Entities.Producto", b =>
                {
                    b.HasOne("Store.Entities.Familia", "Familia")
                        .WithMany()
                        .HasForeignKey("FamiliaId");

                    b.HasOne("Store.Entities.TipoNegocio", "TipoNegocio")
                        .WithMany()
                        .HasForeignKey("TipoNegocioId");

                    b.Navigation("Familia");

                    b.Navigation("TipoNegocio");
                });

            modelBuilder.Entity("Store.Entities.Rack", b =>
                {
                    b.HasOne("Store.Entities.Almacen", "Almacen")
                        .WithMany()
                        .HasForeignKey("AlmacenId");

                    b.Navigation("Almacen");
                });

            modelBuilder.Entity("Store.Entities.User", b =>
                {
                    b.HasOne("Store.Entities.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("RolId");

                    b.HasOne("Store.Entities.UserSession", "UserSession")
                        .WithMany()
                        .HasForeignKey("UserSessionId");

                    b.Navigation("Rol");

                    b.Navigation("UserSession");
                });

            modelBuilder.Entity("Store.Entities.ProductIn", b =>
                {
                    b.Navigation("ProductInDetails");
                });

            modelBuilder.Entity("Store.Entities.Rol", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Store.Entities.TipoNegocio", b =>
                {
                    b.Navigation("Familias");
                });
#pragma warning restore 612, 618
        }
    }
}
