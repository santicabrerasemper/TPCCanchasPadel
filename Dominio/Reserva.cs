using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public Usuario Usuario { get; set; }
        public Cancha Cancha { get; set; }
        public DateTime FechaReserva { get; set; }     
        public TimeSpan HoraInicio { get; set; }        
        public TimeSpan HoraFin { get; set; }           
        public Promocion Promocion { get; set; }
    }
}
