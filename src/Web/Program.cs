
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data; // Aseg�rate de ajustar el espacio de nombres seg�n la ubicaci�n real de tu DbContext

using Microsoft.Data.Sqlite;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<CartService>(); // Agrega el servicio del carrito


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
