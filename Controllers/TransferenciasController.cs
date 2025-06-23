using Microsoft.AspNetCore.Mvc;
using BancoAPI.Entidades;
using BancoAPI.Logica;
using System.Data.SqlClient;

namespace BancoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : ControllerBase
    {
        [HttpPost("realizar")]
        public IActionResult RealizarTransferencia([FromBody] Transferencia t)
        {
            using var con = new SqlConnection(ConexionBD.Cadena);
            con.Open();
            using var tran = con.BeginTransaction();

            try
            {
                // 1. Verifica que haya suficiente saldo
                var cmdSaldo = new SqlCommand("SELECT SAL_CUE FROM CUENTAS WHERE NUM_CUE=@ori", con, tran);
                cmdSaldo.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                decimal saldoOrigen = Convert.ToDecimal(cmdSaldo.ExecuteScalar());

                if (saldoOrigen < t.Valor)
                    return BadRequest(new { mensaje = "❌ Saldo insuficiente." });

                // 2. Descontar de origen
                var cmdDescontar = new SqlCommand("UPDATE CUENTAS SET SAL_CUE = SAL_CUE - @val WHERE NUM_CUE = @ori", con, tran);
                cmdDescontar.Parameters.AddWithValue("@val", t.Valor);
                cmdDescontar.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                cmdDescontar.ExecuteNonQuery();

                // 3. Sumar a destino
                var cmdSumar = new SqlCommand("UPDATE CUENTAS SET SAL_CUE = SAL_CUE + @val WHERE NUM_CUE = @des", con, tran);
                cmdSumar.Parameters.AddWithValue("@val", t.Valor);
                cmdSumar.Parameters.AddWithValue("@des", t.CuentaDestino);
                cmdSumar.ExecuteNonQuery();

                // 4. Registrar la transferencia
                var cmdInsert = new SqlCommand(@"
                    INSERT INTO TRANSFERENCIAS (FEC_TRA, VALOR_TRA, NUM_CUE_ORI, NUM_CUE_DES)
                    VALUES (@fec, @val, @ori, @des)", con, tran);
                cmdInsert.Parameters.AddWithValue("@fec", t.Fecha);
                cmdInsert.Parameters.AddWithValue("@val", t.Valor);
                cmdInsert.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                cmdInsert.Parameters.AddWithValue("@des", t.CuentaDestino);
                cmdInsert.ExecuteNonQuery();

                tran.Commit();
                return Ok(new { mensaje = "✅ Transferencia realizada con éxito." });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return StatusCode(500, new { mensaje = "❌ Error durante la transferencia", error = ex.Message });
            }
        }
    }
}

