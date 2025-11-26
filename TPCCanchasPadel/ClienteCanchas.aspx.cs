using ConexionBD;
using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class ClienteCanchas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad.NoCache(this);
            Seguridad.RequerirSesion(this);

            if (!Seguridad.SesionActiva(Session))
                return;

            if (!IsPostBack)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-AR");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-AR");

                gvCanchas.Visible = false;
                lblMensaje.Text = "";
                btnNuevaBusqueda.Visible = false;
                CargarSucursales();

                var loged = Session["Usuario"] as Usuario;

                if (loged != null && loged.RolID == 1)
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

            var usuario = Session["Usuario"] as Usuario;
            if (usuario != null && usuario.RolID == 1)
                btnVolver.Visible = true;
            else
                btnVolver.Visible = false;

            lblPromo.Visible = false;
            lblPromo.Text = string.Empty;
            hidPromoId.Value = string.Empty;
            ViewState.Remove("PromoPct");
        }

        private void MostrarInfoSucursal(Sucursal suc)
        {
            if (suc == null)
            {
                lblDescCancha.Text = string.Empty;
                lblUbicacion.Text = string.Empty;
                lblDescCancha.Visible = false;
                lblUbicacion.Visible = false;
                return;
            }

            string desc;
            string ubicacion;

            switch (suc.SucursalID)
            {
                case 1:
                    desc = "Complejo con canchas de césped sintético profesional, todas techadas.";
                    ubicacion = "Ubicación: Av. Padel Club 1234, Barrio Centro, Buenos Aires.";
                    break;

                case 2:
                    desc = "Canchas al aire libre, iluminación LED y estacionamiento propio.";
                    ubicacion = "Ubicación: Calle Padel Sur 456, Barrio Sur, La Plata.";
                    break;

                default:
                    desc = $"Canchas disponibles en {suc.Nombre}. Consultanos por más detalles.";
                    ubicacion = string.Empty;
                    break;
            }

            lblDescCancha.Text = desc;
            lblUbicacion.Text = ubicacion;

            lblDescCancha.Visible = !string.IsNullOrEmpty(desc);
            lblUbicacion.Visible = !string.IsNullOrEmpty(ubicacion);
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
            Sucursal suc = negocio.ListarSucursales() .Find(x => x.SucursalID == id);

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
            MostrarInfoSucursal(suc);

            var fechaSeleccionada = DateTime.Today;
            if (DateTime.TryParse(txtFecha.Text, out var f)) fechaSeleccionada = f;

            if (ddlSucursal.SelectedIndex > 0)
            {
                int sucursalId = int.Parse(ddlSucursal.SelectedValue);
                CargarPromoVigente(sucursalId, fechaSeleccionada);
            }
            else
            {
                lblPromo.Visible = false;
                lblPromo.Text = string.Empty;
                hidPromoId.Value = string.Empty;
                ViewState.Remove("PromoPct");
            }
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

                CargarPromoVigente(sucursalId, fecha);

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

                    int promoPct = 0;
                    if (ViewState["PromoPct"] != null)
                        int.TryParse(ViewState["PromoPct"].ToString(), out promoPct);

                    if (promoPct > 0)
                    {
                        foreach (var c in disponibles)
                        {
                            c.TotalEstimado = Math.Round(
                                c.TotalEstimado * (100 - promoPct) / 100m,
                                2
                            );
                        }
                    }

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

                    int? promoId = null;
                    if (int.TryParse(hidPromoId.Value, out int parsedPromo) && parsedPromo > 0)
                    {
                        promoId = parsedPromo;
                    }

                    int promoPct = 0;
                    if (ViewState["PromoPct"] != null)
                        int.TryParse(ViewState["PromoPct"].ToString(), out promoPct);
            
                    int nuevaId = negocio.ReservarCancha(
                        usuarioId,
                        canchaId,
                        fecha,
                        horaInicio,
                        horaFin,
                        promoId
                    );

                    if (nuevaId > 0)
                    {
                        string extraPromo = "";
                        if (promoId.HasValue && promoPct > 0)
                        {
                            extraPromo = $"<br/>🎉 Se aplicará la promoción vigente de <b>{promoPct}% OFF</b> sobre el total.";
                        }

                        MostrarMensaje(
                            $"🟡 <b>Reserva registrada como PENDIENTE DE PAGO.</b><br/>" +
                            $"📅 Fecha: {fecha:dd/MM/yyyy}<br/>" +
                            $"🕒 Horario: {horaInicio:hh\\:mm} - {horaFin:hh\\:mm}{extraPromo}<br/>" +
                            $"✅ Realizá el pago a nuestro Alias: <b>canchaspadel.mp</b><br/>" +
                            $"📌 Enviá el comprobante al teléfono: 1163097274<br/>" +
                            $"❌ Para cancelar tu reserva debés avisarnos por chat 24 hs previas al turno.",
                            "warning");

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
                List<Reserva> reservas = negocio.ListarPorUsuario(usuario.UsuarioID);

                if (reservas != null && reservas.Count > 0)
                {
                    gvMisReservas.Visible = true;
                    gvMisReservas.DataSource = reservas;
                    gvMisReservas.DataBind();

                    lblCantidadReservas.Text = $"Total de reservas: {reservas.Count}";
                }
                else
                {
                    gvMisReservas.Visible = false;
                    lblMisReservasMsg.Text = "Todavía no hiciste ninguna reserva.";
                    lblMisReservasMsg.CssClass = "text-danger fw-bold";
                    lblCantidadReservas.Text = "";
                    return;
                }

                
                var reservaConfirmada = reservas.Find(r =>
                    r.Estado.Nombre.Equals("Confirmada", StringComparison.OrdinalIgnoreCase));

                if (reservaConfirmada != null)
                {
                   
                    lblMisReservasMsg.Text =
                    $@"<div class='alert alert-success shadow-sm p-3 mb-3' role='alert' style='font-size:1.05rem;'>
                    <strong>🎉 ¡Reserva confirmada!</strong><br/>
                    Tu reserva de la cancha <b>{reservaConfirmada.Cancha.Nombre}</b> fue confirmada con éxito.<br/>
                    ¡Gracias por elegirnos!
                    </div>";

                    lblMisReservasMsg.Visible = true;
                }
                else
                {
                    lblMisReservasMsg.Text = "";
                    lblMisReservasMsg.Visible = false;
                }
            }
            catch (Exception ex)
            {
                gvMisReservas.Visible = false;
                lblMisReservasMsg.Text =
                    "⚠️ Error al listar tus reservas: " + ex.Message +
                    (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                lblMisReservasMsg.CssClass = "alert alert-danger fw-bold";
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
        protected void gvMisReservas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Reserva r = (Reserva)e.Row.DataItem;

                string estado = r.Estado?.Nombre ?? "Pendiente";
                
                if (estado.Equals("Confirmada", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.CssClass += " table-success";  
                }
                else if (estado.Equals("Cancelada", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.CssClass += " table-danger";
                }
                else
                {
                    e.Row.CssClass += " table-warning";  
                }
            }
        }
        private void CargarPromoVigente(int sucursalId, DateTime fecha)
        {
            lblPromo.Visible = false;
            lblPromo.Text = string.Empty;
            hidPromoId.Value = string.Empty;

            var datos = new AccesoDatos();
            try
            {        
                string f = fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                datos.setearConsulta($@"
            SELECT TOP 1 PromocionID, Descripcion, Descuento
            FROM Promociones
            WHERE SucursalID = {sucursalId}
            AND EstadoID = 1
            AND '{f}' BETWEEN FechaInicio AND FechaFin
            ORDER BY PromocionID DESC");

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int promoId = Convert.ToInt32(datos.Lector["PromocionID"]);
                    string desc = Convert.ToString(datos.Lector["Descripcion"]);
                    int descuentoPct = Convert.ToInt32(datos.Lector["Descuento"]);

                    hidPromoId.Value = promoId.ToString();
                    lblPromo.Text = $"🎉 <b>Promo vigente:</b> {desc} – <b>{descuentoPct}% OFF</b>";
                    lblPromo.Visible = true;

                    ViewState["PromoPct"] = descuentoPct;
                }
                else
                {
                    ViewState.Remove("PromoPct");
                }
            }
            catch (Exception ex)
            {
   
                ViewState.Remove("PromoPct");
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}