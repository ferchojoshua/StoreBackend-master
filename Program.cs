using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Helpers.User;
using Store.Helpers.CreateLogoHelper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Helpers.EntradaProductos;
using Store.Helpers.ProductHelper;
using Store.Helpers.ProdMovements;
using Store.Helpers.ClientService;
using Store.Helpers.Locations;
using System.Text.Json.Serialization;
using Store.Helpers.SalesHelper;
using Store.Helpers.ProductExistenceService;
using StoreBackend.Helpers.ContabilidadService;
using Store.Helpers.AsientoContHelper;
using Store.Hubs;
using Store.Helpers.ReportHelper;
using Store.Helpers.FacturacionHelper;
using Store.Helpers.StoreService;
using Store;
using Store.Helpers.Worker;
using Store.Helpers.ServerHelper;
using Store.Helpers.CreateLogoHelperList;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services
    .AddControllers()
    .AddJsonOptions(s => s.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var MyAllowSpecificOrigins = "Clients";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        builder =>
        {
            builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://automoto.eastus.cloudapp.azure.com", "http://localhost:3000");
        });
});

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    builder.Services.AddDbContext<DataContext>(
        opt =>
            opt.UseSqlServer(
                builder.Configuration.GetConnectionString("DevConnetion"),
                x => x.UseNetTopologySuite()
            )
    );
}
else
{
    builder.Services.AddDbContext<DataContext>(
        opt =>
            opt.UseSqlServer(
                builder.Configuration.GetConnectionString("ProdConnetion"),
                x => x.UseNetTopologySuite()
            )
    );
}

builder.Services.AddSignalR();

builder.Services
    .AddIdentity<Store.Entities.User, IdentityRole>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;
        cfg.Password.RequireDigit = false;
        cfg.Password.RequiredUniqueChars = 0;
        cfg.Password.RequireLowercase = false;
        cfg.Password.RequireNonAlphanumeric = false;
        cfg.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<DataContext>();

builder.Services
    .AddAuthentication()
    .AddCookie()
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Tokens:Issuer"],
            ValidAudience = builder.Configuration["Tokens:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"])
            )
        };
    });

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IProductsInHelper, ProductsInHelper>();
builder.Services.AddScoped<IProductHelper, ProductHelper>();
builder.Services.AddScoped<IProductMovementsHelper, ProductMovementsHelper>();
builder.Services.AddScoped<IClientsHelper, ClientsHelper>();
builder.Services.AddScoped<ICreateLogoHelper, CreateLogoHelper>();
builder.Services.AddScoped<ICreateLogoHelperList, CreateLogoHelperList>();
builder.Services.AddScoped<ILocationsHelper, LocationsHelper>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ICashMovmentService, CashMovmentService>();
builder.Services.AddScoped<IProdExistService, ProdExistService>();
builder.Services.AddScoped<IContService, ContService>();
builder.Services.AddScoped<IAsientoContHelper, AsientoContHelper>();
builder.Services.AddScoped<IReportsHelper, ReportsHelper>();
builder.Services.AddScoped<IFacturationHelper, FacturationHelper>();
builder.Services.AddScoped<IStoreHelper, StoreHelper>();
builder.Services.AddScoped<IServerService, ServerService>();

builder.Services.AddHostedService<Worker>().AddSingleton<IWorkerService, WorkerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("notificationHub");
app.MapHub<NewSalehub>("newSaleHub");
app.MapHub<NewFacturaHub>("newFactHub");
app.MapHub<UpdateHub>("updateClientHub");
app.MapHub<ServerHub>("serverHub");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
