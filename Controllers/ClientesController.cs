using BancoAPI.Entidades;
using BancoAPI.Logica;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var lista = new List<Cliente>();
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("SELECT * FROM CLIENTES", con);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(new Cliente
            {
                Cedula = dr["CED_CLI"].ToString(),
                Nombre = dr["NOM_CLI"].ToString(),
                Apellido = dr["APE_CLI"].ToString()
            });
        }
        return Ok(lista);
    }

    [HttpGet("{cedula}")]
    public IActionResult GetById(string cedula)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("SELECT * FROM CLIENTES WHERE CED_CLI=@ced", con);
        cmd.Parameters.AddWithValue("@ced", cedula);
        using var dr = cmd.ExecuteReader();
        if (dr.Read())
            return Ok(new Cliente
            {
                Cedula = dr["CED_CLI"].ToString(),
                Nombre = dr["NOM_CLI"].ToString(),
                Apellido = dr["APE_CLI"].ToString()
            });
        return NotFound();
    }

    [HttpPost]
    public IActionResult Post([FromBody] Cliente cliente)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("INSERT INTO CLIENTES (CED_CLI,NOM_CLI,APE_CLI) VALUES (@ced,@nom,@ape)", con);
        cmd.Parameters.AddWithValue("@ced", cliente.Cedula);
        cmd.Parameters.AddWithValue("@nom", cliente.Nombre);
        cmd.Parameters.AddWithValue("@ape", cliente.Apellido);
        cmd.ExecuteNonQuery();
        return CreatedAtAction(nameof(GetById), new { cedula = cliente.Cedula }, cliente);
    }

    [HttpPut("{cedula}")]
    public IActionResult Put(string cedula, [FromBody] Cliente cliente)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("UPDATE CLIENTES SET NOM_CLI=@nom,APE_CLI=@ape WHERE CED_CLI=@ced", con);
        cmd.Parameters.AddWithValue("@ced", cedula);
        cmd.Parameters.AddWithValue("@nom", cliente.Nombre);
        cmd.Parameters.AddWithValue("@ape", cliente.Apellido);
        int filas = cmd.ExecuteNonQuery();
        return filas > 0 ? NoContent() : NotFound();
    }

    [HttpDelete("{cedula}")]
    public IActionResult Delete(string cedula)
    {
        using var con = new SqlConnection(ConexionBD.Cadena);
        con.Open();
        using var cmd = new SqlCommand("DELETE CLIENTES WHERE CED_CLI=@ced", con);
        cmd.Parameters.AddWithValue("@ced", cedula);
        int filas = cmd.ExecuteNonQuery();
        return filas > 0 ? NoContent() : NotFound();
    }
}
