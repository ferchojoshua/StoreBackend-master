using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Helpers.User;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Helpers.EntradaProductos;
using Store.Helpers.ProductHelper;
using Store.Helpers.ProdMovements;
using Store.Helpers.ClientService;
using Store.Helpers.Locations;
using System.Text.Json.Serialization;
using Store.Helpers.SalesHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .AddControllers()
    .AddJsonOptions(s => s.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<DataContext>(
    opt =>
        opt.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnetion"),
            x => x.UseNetTopologySuite()
        )
);

builder.Services
    .AddIdentity<Store.Entities.User, IdentityRole>(
        cfg =>
        {
            cfg.User.RequireUniqueEmail = true;
            cfg.Password.RequireDigit = false;
            cfg.Password.RequiredUniqueChars = 0;
            cfg.Password.RequireLowercase = false;
            cfg.Password.RequireNonAlphanumeric = false;
            cfg.Password.RequireUppercase = false;
        }
    )
    .AddEntityFrameworkStores<DataContext>();

builder.Services
    .AddAuthentication()
    .AddCookie()
    .AddJwtBearer(
        cfg =>
        {
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Tokens:Issuer"],
                ValidAudience = builder.Configuration["Tokens:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"])
                )
            };
        }
    );

builder.Services.Configure<SecurityStampValidatorOptions>(
    options =>
    {
        options.ValidationInterval = TimeSpan.Zero;
    }
);

builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IProductsInHelper, ProductsInHelper>();
builder.Services.AddScoped<IProductHelper, ProductHelper>();
builder.Services.AddScoped<IProductMovementsHelper, ProductMovementsHelper>();
builder.Services.AddScoped<IClientsHelper, ClientsHelper>();
builder.Services.AddScoped<ILocationsHelper, LocationsHelper>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ICashMovmentService, CashMovmentService>();

var MyAllowSpecificOrigins = "Origins";
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            name: MyAllowSpecificOrigins,
            builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000", "https://auto-moto.netlify.app/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
