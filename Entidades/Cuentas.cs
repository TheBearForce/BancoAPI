using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoAPI.Entidades
{
    public class Cuentas
    {
        public string Numero { get; set; }
        public string Tipo { get; set; }
        public decimal Saldo { get; set; }
        public string CedulaCliente { get; set; }
    }
}
