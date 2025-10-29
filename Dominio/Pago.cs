using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pago
    {
        public int PagoID { get; set; }

        public int ReservaID { get; set; }
        public Reserva Reserva { get; set; }   

        public decimal Importe { get; set; }

        /// Valores esperados: "FAKE", "CREDITO", "DEBITO", "EFECTIVO"
        public string MedioPago { get; set; }

        /// Valores esperados: "APROBADO", "RECHAZADO"
        public string Estado { get; set; }

        public DateTime FechaPago { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string Notas { get; set; }
    }
}

