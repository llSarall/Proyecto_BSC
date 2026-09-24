using BusinessLogic.Services;
using DataAccess;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Dapper: mapea columnas snake_case (id_producto) a propiedades PascalCase (IdProducto)
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("BSC")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BSC'.");

// Inyección de dependencias
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "BSC API"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();