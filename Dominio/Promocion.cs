using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Promocion
    {
        public int PromocionID { get; set; }
        public int SucursalID { get; set; }
        public string Descripcion { get; set; }
        public byte Descuento { get; set; }      
        public DateTime FechaInicio { get; set; } 
        public DateTime FechaFin { get; set; } 
        public int EstadoID { get; set; }

        public bool EstaVigenteEn(DateTime fecha) =>
            fecha.Date >= FechaInicio.Date && fecha.Date <= FechaFin.Date;

        public decimal DescuentoComoFactor => Descuento / 100m;
    }
}
