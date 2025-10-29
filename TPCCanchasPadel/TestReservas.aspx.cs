using System;
using System.Linq;
using Negocio;

namespace TPCCanchasPadel
{
    public partial class TestReservas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarGrid();
        }

        private void CargarGrid()
        {
            try
            {
                var servicio = new ReservasNegocio();
                var reservas = servicio.ObtenerTodasLasReservas();

                var vista = reservas.Select(r => new
                {
                    r.IdReserva,
                    Usuario = (r.Usuario != null)
                        ? (r.Usuario.Nombre + " " + r.Usuario.Apellido)
                        : string.Empty,
                    Cancha = (r.Cancha != null) ? r.Cancha.Nombre : string.Empty,
                    Fecha = r.FechaReserva.ToString("yyyy-MM-dd"),
                    HoraInicio = r.HoraInicio.ToString(@"hh\:mm"),
                    HoraFin = r.HoraFin.ToString(@"hh\:mm"),
                    Promocion = r.Promocion != null ? r.Promocion.Descripcion : string.Empty
                }).ToList();

                gvReservas.DataSource = vista;
                gvReservas.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<pre style='color:red'>" + Server.HtmlEncode(ex.ToString()) + "</pre>");
            }
        }
    }
}
