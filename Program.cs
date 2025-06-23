using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BancoAPI.Logica;

var builder = WebApplication.CreateBuilder(args);

// Cadena de conexión
ConexionBD.Inicializar(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BancoAPI",
        Version = "v1",
        Description = "API REST para gestión de cuentas, clientes y transferencias."
    });
});

var app = builder.Build();

app.UseCors("PermitirTodo");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BancoAPI v1");
    c.RoutePrefix = string.Empty;
});
app.UseAuthorization();
app.MapControllers();
app.Run();
