using System.ComponentModel.DataAnnotations;


namespace BancoAPI.Entidades
{
    public class Transferencia
    {
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La cuenta origen es obligatoria.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "La cuenta origen no debe contener caracteres especiales.")]
        public string CuentaOrigen { get; set; }

        [Required(ErrorMessage = "La cuenta destino es obligatoria.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "La cuenta destino no debe contener caracteres especiales.")]
        public string CuentaDestino { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayor que cero.")]
        public decimal Valor { get; set; }
    }
}
