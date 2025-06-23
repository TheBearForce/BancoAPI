using Microsoft.Extensions.Configuration;

namespace BancoAPI.Logica
{
    public static class ConexionBD
    {
        public static string Cadena { get; private set; }

        public static void Inicializar(IConfiguration configuration)
        {
            Cadena = configuration.GetConnectionString("BancoDB");
        }
    }
}