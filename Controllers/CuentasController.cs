using BancoAPI.Entidades;
using BancoAPI.Logica;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var lista = new List<Cuentas>();
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("SELECT * FROM CUENTAS", con);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(new Cuentas
            {
                Numero = dr["NUM_CUE"].ToString(),
                Tipo = dr["TIPO_CUE"].ToString(),
                Saldo = Convert.ToDecimal(dr["SAL_CUE"]),
                CedulaCliente = dr["CED_CLI"] != DBNull.Value ? dr["CED_CLI"].ToString() : ""
            });
        }
        return Ok(lista);
    }

    [HttpGet("{numero}")]
    public IActionResult GetByNumero(string numero)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("SELECT * FROM CUENTAS WHERE NUM_CUE = @num", con);
        cmd.Parameters.AddWithValue("@num", numero);
        using var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            var cuenta = new Cuentas
            {
                Numero = dr["NUM_CUE"].ToString(),
                Tipo = dr["TIPO_CUE"].ToString(),
                Saldo = Convert.ToDecimal(dr["SAL_CUE"]),
                CedulaCliente = dr["CED_CLI"] != DBNull.Value ? dr["CED_CLI"].ToString() : ""
            };
            return Ok(cuenta);
        }
        return NotFound();
    }

    [HttpPost]
    public IActionResult Post([FromBody] Cuentas cuenta)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("INSERT INTO CUENTAS (NUM_CUE, TIPO_CUE, SAL_CUE, CED_CLI) VALUES (@num, @tip, @sal, @ced)", con);
        cmd.Parameters.AddWithValue("@num", cuenta.Numero);
        cmd.Parameters.AddWithValue("@tip", cuenta.Tipo);
        cmd.Parameters.AddWithValue("@sal", cuenta.Saldo);
        cmd.Parameters.AddWithValue("@ced", cuenta.CedulaCliente);
        cmd.ExecuteNonQuery();

        return CreatedAtAction(nameof(GetByNumero), new { numero = cuenta.Numero }, cuenta);
    }

    [HttpPut("{numero}")]
    public IActionResult Put(string numero, [FromBody] Cuentas cuenta)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("UPDATE CUENTAS SET TIPO_CUE = @tip, SAL_CUE = @sal, CED_CLI = @ced WHERE NUM_CUE = @num", con);
        cmd.Parameters.AddWithValue("@num", numero);
        cmd.Parameters.AddWithValue("@tip", cuenta.Tipo);
        cmd.Parameters.AddWithValue("@sal", cuenta.Saldo);
        cmd.Parameters.AddWithValue("@ced", cuenta.CedulaCliente);
        int filas = cmd.ExecuteNonQuery();

        return filas > 0 ? NoContent() : NotFound();
    }

    [HttpDelete("{numero}")]
    public IActionResult Delete(string numero)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("DELETE FROM CUENTAS WHERE NUM_CUE = @num", con);
        cmd.Parameters.AddWithValue("@num", numero);
        int filas = cmd.ExecuteNonQuery();

        return filas > 0 ? NoContent() : NotFound();
    }
}
