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
            Seguridad.RequerirSesion(this);
            Seguridad.NoCache(this);
            if (!IsPostBack)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-AR");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-AR");

                gvCanchas.Visible = false;
                lblMensaje.Text = "";
                btnNuevaBusqueda.Visible = false;
                CargarSucursales();

                Usuario loged = (Usuario)Session["Usuario"];

                if(loged.RolID == 01)
                {
                    txtFecha.Attributes.Remove("required");
                    txtHoraInicio.Attributes.Remove("required");
                    txtHoraFin.Attributes.Remove("required");
                    ddlSucursal.Attributes.Remove("required");
                }


                if (Session["MensajeInfo"] != null)
                {
                    MostrarMensaje(Session["MensajeInfo"].ToString(), "info");
                    Session["MensajeInfo"] = null;
                }
            }

            Usuario usuario = (Usuario)Session["Usuario"];
            if (usuario.RolID == 01)
            {
                btnVolver.Visible = true;
            }
            else
            {
                btnVolver.Visible = false;
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


        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (ddlSucursal.SelectedIndex <= 0)
            {
                imgSucursal.Visible = false;
                return;
            }

            int id = int.Parse(ddlSucursal.SelectedValue);

            
            SucursalNegocio negocio = new SucursalNegocio();
            Sucursal suc = negocio.ListarSucursales()
                                  .Find(x => x.SucursalID == id);

            
            string imagenDefault = "https://via.placeholder.com/350x200.png?text=Sin+Imagen";

            if (suc != null && !string.IsNullOrEmpty(suc.FotoUrl))
            {
                imgSucursal.ImageUrl = suc.FotoUrl;
            }
            else
            {
                imgSucursal.ImageUrl = imagenDefault;
            }

            imgSucursal.Visible = true;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReservasAdmin.aspx");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFecha.Text) ||
                    string.IsNullOrEmpty(txtHoraInicio.Text) ||
                    string.IsNullOrEmpty(txtHoraFin.Text) ||
                    ddlSucursal.SelectedValue == "0")
                {
                    MostrarMensaje("⚠️ Completá todos los campos antes de buscar (sucursal, fecha y horarios).", "warning");
                    gvCanchas.Visible = false;
                    return;
                }

                DateTime fecha = DateTime.Parse(txtFecha.Text);
                TimeSpan horaInicio = TimeSpan.Parse(txtHoraInicio.Text);
                TimeSpan horaFin = TimeSpan.Parse(txtHoraFin.Text);
                int sucursalId = int.Parse(ddlSucursal.SelectedValue);

                ClienteCanchaNegocio negocio = new ClienteCanchaNegocio();

                
                string errorValidacion = negocio.ValidarBusquedaGeneral(fecha, horaInicio, horaFin);
                if (errorValidacion != null)
                {
                    MostrarMensaje(errorValidacion, "warning");
                    gvCanchas.Visible = false;
                    return;
                }

                
                List<Cancha> disponibles = negocio.ListarCanchasDisponibles(fecha, horaInicio, horaFin, sucursalId);

                if (disponibles.Count == 0)
                {
                    MostrarMensaje("⚠️ En ese horario ya hay reservas activas para esta sucursal. Probá con otro rango horario.", "warning");
                    gvCanchas.Visible = false;
                }
                else
                {
                    disponibles = disponibles.Where(c => c.Activa).ToList();
                    gvCanchas.DataSource = disponibles;
                    gvCanchas.DataBind();
                    gvCanchas.Visible = true;
                    lblMensaje.Text = "";
                    btnNuevaBusqueda.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("❌ Error al buscar las canchas: " + ex.Message, "danger");
                gvCanchas.Visible = false;
            }
        }

        protected void grillaCanchas_ComandoReserva(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reservar")
            {
                try
                {
                    int canchaId = Convert.ToInt32(e.CommandArgument);

                    DateTime fecha = DateTime.Parse(txtFecha.Text);
                    TimeSpan horaInicio = TimeSpan.Parse(txtHoraInicio.Text);
                    TimeSpan horaFin = TimeSpan.Parse(txtHoraFin.Text);
                    if (Session["Usuario"] == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    Usuario usuario = (Usuario)Session["Usuario"];
                    int usuarioId = usuario.UsuarioID;

                    ClienteCanchaNegocio negocio = new ClienteCanchaNegocio();

                    
                    string error = negocio.ValidarReservaCompleta(canchaId, fecha, horaInicio, horaFin);
                    if (error != null)
                    {
                        MostrarMensaje(error, "warning");
                        return;
                    }

                   
                    int nuevaId = negocio.ReservarCancha(usuarioId, canchaId, fecha, horaInicio, horaFin);

                    if (nuevaId > 0)
                    {
                        MostrarMensaje(
                            $"✅ ¡Reserva confirmada!<br/>📅 Fecha: {fecha:dd/MM/yyyy}<br/>🕒 Horario: {horaInicio:hh\\:mm} - {horaFin:hh\\:mm}<br/>💵 Total: ${CalcularPrecio(horaInicio, horaFin)}",
                            "success");

                        gvCanchas.Visible = false;
                        btnNuevaBusqueda.Visible = true;
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

        protected void btnMisReservas_Click(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Usuario usuario = (Usuario)Session["Usuario"];
            ReservasNegocio negocio = new ReservasNegocio();

            try
            {
                lblMisReservasMsg.Text = "Usuario ID: " + usuario.UsuarioID;

                List<Reserva> reservas = negocio.ListarPorUsuario(usuario.UsuarioID);

                if (reservas != null && reservas.Count > 0)
                {
                    gvMisReservas.Visible = true;
                    gvMisReservas.DataSource = reservas;
                    gvMisReservas.DataBind();

                    lblMisReservasMsg.Text = "";
                    lblCantidadReservas.Text = $"Total de reservas: {reservas.Count}";
                }
                else
                {
                    gvMisReservas.Visible = false;
                    lblMisReservasMsg.Text = "Todavía no hiciste ninguna reserva.";
                    lblCantidadReservas.Text = "";
                }
            }
            catch (Exception ex)
            {
                gvMisReservas.Visible = false;
                lblMisReservasMsg.Text =
                    "⚠️ Error al listar tus reservas: " + ex.Message +
                    (ex.InnerException != null ? " | " + ex.InnerException.Message : "") +
                    "<br><br><strong>Detalle completo:</strong><br>" + ex.ToString();
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
            string color, icono, titulo;

            switch (tipo)
            {
                case "success":
                    color = "border-success text-success bg-light";
                    icono = "✅";
                    titulo = "¡Reserva confirmada!";
                    break;
                case "warning":
                    color = "border-warning text-warning bg-light";
                    icono = "⚠️";
                    titulo = "Atención";
                    break;
                case "danger":
                    color = "border-danger text-danger bg-light";
                    icono = "❌";
                    titulo = "Error";
                    break;
                default:
                    color = "border-info text-info bg-light";
                    icono = "ℹ️";
                    titulo = "Información";
                    break;
            }

            lblMensaje.Text = $@"
            <div class='alert {color} p-4 rounded-3 shadow-sm mx-auto text-center' 
                 style='max-width:500px; animation: fadeIn 0.6s ease-in-out;'>
                <h5 class='fw-bold mb-2'>{icono} {titulo}</h5>
                <p class='mb-0'>{texto}</p>
            </div>

            <style>
                @keyframes fadeIn {{
                    from {{ opacity: 0; transform: translateY(-10px); }}
                    to {{ opacity: 1; transform: translateY(0); }}
                }}
            </style>";
        }

       

        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {
            Session["MensajeInfo"] = "Listo, podés realizar una nueva búsqueda.";
            Response.Redirect(Request.RawUrl);
        }

    }
}