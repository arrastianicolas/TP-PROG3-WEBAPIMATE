
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data; // Asegúrate de ajustar el espacio de nombres según la ubicación real de tu DbContext

using Microsoft.Data.Sqlite;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<CartService>(); // Agrega el servicio del carrito
#region Repositories
builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();
builder.Services.AddScoped<IUserRepository, UserRepositoryEf>();
#endregion
#region Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
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
