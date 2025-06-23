using Microsoft.AspNetCore.Mvc;
using BancoAPI.Entidades;
using BancoAPI.Service;
using System.Linq;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BancoController : ControllerBase
    {
        private readonly BancoService _banco = new();

        [HttpGet("cuenta-existe/{numero}")]
        public IActionResult CuentaExiste(string numero)
        {
            if (!EsValido(numero))
                return BadRequest(new { mensaje = "El número de cuenta contiene caracteres no permitidos." });

            bool existe = _banco.CuentaExiste(numero);
            return Ok(existe);
        }

        [HttpGet("saldo/{numero}")]
        public IActionResult ObtenerSaldo(string numero)
        {
            if (!EsValido(numero))
                return BadRequest(new { mensaje = "El número de cuenta contiene caracteres no permitidos." });

            try
            {
                decimal saldo = _banco.ObtenerSaldo(numero);
                return Ok(saldo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("transferir")]
        public IActionResult Transferir([FromBody] Transferencia t)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!EsValido(t.CuentaOrigen) || !EsValido(t.CuentaDestino))
                    throw new Exception("Las cuentas no deben contener caracteres especiales.");
                if (t.CuentaOrigen == t.CuentaDestino)
                    throw new Exception("La cuenta origen y destino no pueden ser iguales.");
                if (!_banco.CuentaExiste(t.CuentaOrigen))
                    throw new Exception("La cuenta origen no existe.");
                if (!_banco.CuentaExiste(t.CuentaDestino))
                    throw new Exception("La cuenta destino no existe.");
                if (_banco.ObtenerSaldo(t.CuentaOrigen) < t.Valor)
                    throw new Exception("Saldo insuficiente en la cuenta origen.");

                bool exito = _banco.Transferir(t);
                return Ok(new { mensaje = "Transferencia realizada con éxito", exito });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        private bool EsValido(string cuenta)
            => cuenta.All(c => char.IsLetterOrDigit(c));
    }
}

