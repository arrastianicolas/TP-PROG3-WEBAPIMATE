
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data; // Aseg�rate de ajustar el espacio de nombres seg�n la ubicaci�n real de tu DbContext

using Microsoft.Data.Sqlite;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Services;
using Azure;
using Domain.Entities;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddScoped<CartService>(); // Agrega el servicio del carrito
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("ConsultaAlumnosApiBearerAuth", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Ac� pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ConsultaAlumnosApiBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definici�n
                }, new List<string>() }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setupAction.IncludeXmlComments(xmlPath);

});



#region Repositories
builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();
builder.Services.AddScoped<IUserRepository, UserRepositoryEf>();
builder.Services.AddScoped<IRepositoryBase<User>, EfRepository<User>>();
builder.Services.AddScoped<IRepositoryBase<Product>, EfRepository<Product>>();  
//builder.Services.AddScoped<IRepositoryBase<>, EfRepository<>>();
//builder.Services.AddScoped<IRepositoryBase<>, EfRepository<>>();

#endregion
#region Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService , UserService>();   
builder.Services.AddScoped<ICustomAuthenticationService, AutenticacionService>();

#endregion
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"]!;
var connection = new SqliteConnection(connectionString);
connection.Open();

using (var command = connection.CreateCommand()) 
{
    command.CommandText = "PRAGMA journal_mode = DELETE;";
    command.ExecuteNonQuery();
}
builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));
builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticaci�n que tenemos que elegir despu�s en PostMan para pasarle el token
    .AddJwtBearer(options => //Ac� definimos la configuraci�n de la autenticaci�n. le decimos qu� cosas queremos comprobar. La fecha de expiraci�n se valida por defecto.
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AutenticacionService:Issuer"],
            ValidAudience = builder.Configuration["AutenticacionService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AutenticacionService:SecretForKey"]))
        };
    }
);
// Configuraci�n de pol�ticas de autorizaci�n
builder.Services.AddAuthorization(options => //Agregamos pol�ticas para la autorizaci�n de los respectivos ENDPOINTS.
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("usertype", "Admin"));
    options.AddPolicy("Client", policy => policy.RequireClaim("usertype", "Client"));
    options.AddPolicy("Admin&Seller", policy => policy.RequireClaim("usertype", "Admin", "Seller"));
});

var app = builder.Build();

// Configurar el middleware de la solicitud HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
