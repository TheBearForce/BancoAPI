using BancoAPI.Entidades;
using BancoAPI.Logica;
using System.Data.SqlClient;
using System.Data;

namespace BancoAPI.Service
{
    public class BancoService
    {
        public bool CuentaExiste(string numeroCuenta)
        {
            using SqlConnection con = new(ConexionBD.Cadena);
            using SqlCommand cmd = new("SELECT COUNT(*) FROM CUENTAS WHERE NUM_CUE = @num", con);
            cmd.Parameters.AddWithValue("@num", numeroCuenta);
            con.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public decimal ObtenerSaldo(string numeroCuenta)
        {
            using SqlConnection con = new(ConexionBD.Cadena);
            using SqlCommand cmd = new("SELECT SAL_CUE FROM CUENTAS WHERE NUM_CUE = @num", con);
            cmd.Parameters.AddWithValue("@num", numeroCuenta);
            con.Open();
            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public bool Transferir(Transferencia t)
        {
            using SqlConnection con = new(ConexionBD.Cadena);
            con.Open();
            using SqlTransaction tran = con.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                using SqlCommand saldoCmd = new("SELECT SAL_CUE FROM CUENTAS WHERE NUM_CUE = @ori", con, tran);
                saldoCmd.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                object result = saldoCmd.ExecuteScalar();

                if (result == null) throw new Exception("La cuenta origen no existe.");

                decimal saldo = Convert.ToDecimal(result);
                if (saldo < t.Valor) throw new Exception($"Saldo insuficiente. Disponible: {saldo}");

                using SqlCommand debito = new("UPDATE CUENTAS SET SAL_CUE = SAL_CUE - @val WHERE NUM_CUE = @ori", con, tran);
                debito.Parameters.AddWithValue("@val", t.Valor);
                debito.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                debito.ExecuteNonQuery();

                using SqlCommand cuentaDestinoCmd = new("SELECT COUNT(*) FROM CUENTAS WHERE NUM_CUE = @des", con, tran);
                cuentaDestinoCmd.Parameters.AddWithValue("@des", t.CuentaDestino);
                int destinoExiste = (int)cuentaDestinoCmd.ExecuteScalar();
                if (destinoExiste == 0) throw new Exception("La cuenta destino no existe.");

                using SqlCommand credito = new("UPDATE CUENTAS SET SAL_CUE = SAL_CUE + @val WHERE NUM_CUE = @des", con, tran);
                credito.Parameters.AddWithValue("@val", t.Valor);
                credito.Parameters.AddWithValue("@des", t.CuentaDestino);
                credito.ExecuteNonQuery();

                using SqlCommand insert = new("INSERT INTO TRANSFERENCIAS (FEC_TRA, VALOR_TRA, NUM_CUE_ORI, NUM_CUE_DES) VALUES (@fec, @val, @ori, @des)", con, tran);
                insert.Parameters.AddWithValue("@fec", t.Fecha);
                insert.Parameters.AddWithValue("@val", t.Valor);
                insert.Parameters.AddWithValue("@ori", t.CuentaOrigen);
                insert.Parameters.AddWithValue("@des", t.CuentaDestino);
                insert.ExecuteNonQuery();

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}