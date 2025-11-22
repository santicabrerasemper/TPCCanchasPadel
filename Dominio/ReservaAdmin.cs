using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ReservaAdmin
    {
        public int ReservaID { get; set; }
        public string Sucursal { get; set; }
        public string Cancha { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Estado { get; set; }
    }
}