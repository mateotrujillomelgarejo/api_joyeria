using api_joyeria.Data;
using api_joyeria.Data.IRepository;
using api_joyeria.Data.IService;
using api_joyeria.Data.Middleware;
using api_joyeria.Data.Repository;
using api_joyeria.Data.Service;
using api_joyeria.Data.Validators;
using api_joyeria.Helpers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 🔧 CONFIGURACIÓN DE SERVICIOS PRINCIPALES
// ========================================

// Conexion a la base de datos MySQL
builder.Services.AddDbContext<JoyeriaDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Inyección de dependencias de Repositorios y Servicios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();


builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IDetallePedidoService, DetallePedidoService>();

builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<PasswordHasher>();

// FluentValidation (registra todos los Validators del ensamblado)
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<ClienteRequestValidator>()
    );

// AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// ========================================
// 🔐 CONFIGURACIÓN DE JWT
// ========================================
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ========================================
// 🌐 CORS
// ========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin());
});

// ========================================
// 📘 SWAGGER
// ========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Joyeria", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT como: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ========================================
// 🚀 PIPELINE DE MIDDLEWARES
// ========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRequestResponseLogging();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<JoyeriaDbContext>();
    context.Database.EnsureCreated(); // crea la base si no existe

    if (!context.Clientes.Any())
    {
        var adminPassword = PasswordHasher.HashPassword("admin123");

        context.Clientes.Add(new api_joyeria.Models.Cliente
        {
            Id = 1,
            Nombre = "Administrador",
            Email = "admin@joyeria.com",
            Password = adminPassword, // admin123
            Direccion = "Oficina Principal",
            EsAdmin = true,
            FechaRegistro = new DateTime(2025, 1, 1)
        });

        context.Productos.AddRange(
            new api_joyeria.Models.Producto
            {
                Id = 1,
                Nombre = "Anillo de Oro 18k",
                Descripcion = "Elegante anillo de oro de 18 quilates con acabado brillante",
                Precio = 850.00m,
                Stock = 15,
                ImagenUrl = "https://cdn-media.glamira.com/media/product/newgeneration/view/1/sku/GWD210000/alloycolour/yellow/width/w4/profile/prA/surface/polished.jpg",
                Disponible = true,
                FechaCreacion = new DateTime(2025, 1, 1)
            },
            new api_joyeria.Models.Producto
            {
                Id = 2,
                Nombre = "Collar de Plata",
                Descripcion = "Hermoso collar de plata 925 con colgante de corazón",
                Precio = 320.00m,
                Stock = 25,
                ImagenUrl = "https://baliq.com/wp-content/uploads/2023/12/DIS-2982-y-CAS-0864.jpg",
                Disponible = true,
                FechaCreacion = new DateTime(2025, 1, 1)
            },
            new api_joyeria.Models.Producto
            {
                Id = 3,
                Nombre = "Aretes de Diamante",
                Descripcion = "Exclusivos aretes con diamantes naturales",
                Precio = 1200.00m,
                Stock = 8,
                ImagenUrl = "https://cdn-media.glamira.com/media/product/newgeneration/view/1/sku/G100735/diamond/lab-grown-diamond_AAA/alloycolour/white.jpg",
                Disponible = true,
                FechaCreacion = new DateTime(2025, 1, 1)
            }
        );

        context.SaveChanges();
    }
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
