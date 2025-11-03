using ConexionBD;
using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class ClienteCanchas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvCanchas.Visible = false;
                lblMensaje.Text = "";
                CargarSucursales();
            }
        }

        private void CargarSucursales()
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT SucursalID, Nombre FROM Sucursales ORDER BY Nombre");
                datos.ejecutarLectura();

                ddlSucursal.DataSource = datos.Lector;
                ddlSucursal.DataTextField = "Nombre";
                ddlSucursal.DataValueField = "SucursalID";
                ddlSucursal.DataBind();

                ddlSucursal.Items.Insert(0, new ListItem("-- Seleccione sucursal --", "0"));
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "⚠️ Error al cargar las sucursales: " + ex.Message;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }




        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFecha.Text) ||
                    string.IsNullOrEmpty(txtHoraInicio.Text) ||
                    string.IsNullOrEmpty(txtHoraFin.Text))
                {
                    lblMensaje.Text = "Por favor, completá todos los campos antes de buscar.";
                    gvCanchas.Visible = false;
                    return;
                }

                DateTime fecha = DateTime.Parse(txtFecha.Text);
                TimeSpan horaInicio = TimeSpan.Parse(txtHoraInicio.Text);
                TimeSpan horaFin = TimeSpan.Parse(txtHoraFin.Text);

                if (horaInicio >= horaFin)
                {
                    lblMensaje.Text = "La hora de inicio debe ser menor que la hora de fin.";
                    gvCanchas.Visible = false;
                    return;
                }

                ReservasNegocio negocio = new ReservasNegocio();
                List<Cancha> disponibles = negocio.ListarCanchasDisponibles(fecha, horaInicio, horaFin);
                int sucursalId = int.Parse(ddlSucursal.SelectedValue);
                if (sucursalId != 0)
                    disponibles = disponibles.Where(c => c.SucursalID == sucursalId).ToList();

                if (disponibles.Count == 0)
                {
                    lblMensaje.Text = "No hay canchas disponibles en ese horario.";
                    gvCanchas.Visible = false;
                }
                else
                {
                    gvCanchas.DataSource = disponibles;
                    gvCanchas.DataBind();
                    gvCanchas.Visible = true;
                    lblMensaje.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Ocurrió un error al buscar las canchas: " + ex.Message;
                gvCanchas.Visible = false;
            }
        }

        protected void gvCanchas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reservar")
            {
                try
                {
                    int canchaId = Convert.ToInt32(e.CommandArgument);

                    if (string.IsNullOrEmpty(txtFecha.Text) || string.IsNullOrEmpty(txtHoraInicio.Text) || string.IsNullOrEmpty(txtHoraFin.Text))
                    {
                        MostrarMensaje("⚠️ Por favor, seleccioná una fecha y horario antes de reservar.", "warning");
                        return;
                    }

                    DateTime fecha = DateTime.Parse(txtFecha.Text);
                    TimeSpan horaInicio = TimeSpan.Parse(txtHoraInicio.Text);
                    TimeSpan horaFin = TimeSpan.Parse(txtHoraFin.Text);

                    int usuarioId = 1;

                    Reserva nueva = new Reserva
                    {
                        Usuario = new Usuario { UsuarioID = usuarioId },
                        Cancha = new Cancha { CanchaID = canchaId },
                        FechaReserva = fecha,
                        HoraInicio = horaInicio,
                        HoraFin = horaFin,
                        Promocion = null
                    };

                    ReservasNegocio negocio = new ReservasNegocio();
                    int nuevaId = negocio.AgregarReserva(nueva);

                    if (nuevaId > 0)
                    {
                        MostrarMensaje($@"
<div class='alert alert-success border border-success shadow-sm p-3 rounded text-center' 
     style='max-width:500px; margin:20px auto; font-size:1.1em;'>
    ✅ <strong>¡Reserva confirmada!</strong><br/>
    📅 Fecha: {fecha:dd/MM/yyyy}<br/>
    🕒 Horario: {horaInicio.ToString(@"hh\:mm")} - {horaFin.ToString(@"hh\:mm")}<br/>
    💵 Total: ${CalcularPrecio(horaInicio, horaFin)}
</div>", "success");

                        gvCanchas.Visible = false; 
                    }
                    else
                    {
                        MostrarMensaje("⚠️ No se pudo registrar la reserva.", "warning");
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("❌ Error al registrar la reserva: " + ex.Message, "danger");
                }
            }
        }
        private decimal CalcularPrecio(TimeSpan horaInicio, TimeSpan horaFin)
        {
            decimal precioHora = 6000m; 
            double duracion = (horaFin - horaInicio).TotalHours;
            return precioHora * (decimal)duracion;
        }
        private void MostrarMensaje(string texto, string tipo)
        {
            string color = "";
            switch (tipo)
            {
                case "success":
                    color = "border-success text-success bg-light";
                    break;
                case "warning":
                    color = "border-warning text-warning bg-light";
                    break;
                case "danger":
                    color = "border-danger text-danger bg-light";
                    break;
                default:
                    color = "border-info text-info bg-light";
                    break;
            }

            lblMensaje.Text = $@"
        <div class='alert {color} p-4 rounded-3 shadow-sm mx-auto text-center' 
             style='max-width:500px; animation: fadeIn 0.6s ease-in-out;'>
            <h5 class='fw-bold mb-2'>✅ ¡Reserva confirmada!</h5>
            <p class='mb-0'>{texto}</p>
        </div>

        <style>
            @keyframes fadeIn {{
                from {{ opacity: 0; transform: translateY(-10px); }}
                to {{ opacity: 1; transform: translateY(0); }}
            }}
        </style>
    ";
        }
    }
}