using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Cancha
    {
        public int CanchaID { get; set; }          
        public int SucursalID { get; set; }        
        public int EstadoID { get; set; }          
        public string Nombre { get; set; }
        public bool Activa { get; set; }
        public decimal PrecioHora { get; set; }
        public string NombreSucursal { get; set; }
        public decimal TotalEstimado { get; set; }
        public string NombreLocalidad { get; set; }
        public Sucursal Sucursal { get; set; }

    }
}
