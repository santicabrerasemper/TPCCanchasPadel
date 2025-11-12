using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class ReservasAdmin : System.Web.UI.Page
    {
        private readonly ReservasNegocio reservasNegocio = new ReservasNegocio();
        private readonly SucursalNegocio sucursalNegocio = new SucursalNegocio();
        private readonly CanchaNegocio canchaNegocio = new CanchaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarSucursales();
                CargarReservas();
            }
        }

        // 🔹 Cargar todas las reservas sin filtros (inicio)
        private void CargarReservas()
        {
            var lista = reservasNegocio.ListarReservas();
            gvCanchas.DataSource = lista;
            gvCanchas.DataBind();
        }

        // 🔹 Cargar sucursales en el primer dropdown
        private void CargarSucursales()
        {
            var sucursales = sucursalNegocio.ListarSucursales();
            ddlSucursal.DataSource = sucursales;
            ddlSucursal.DataTextField = "Nombre";
            ddlSucursal.DataValueField = "SucursalID";
            ddlSucursal.DataBind();

            ddlSucursal.Items.Insert(0, new ListItem("-- Seleccione una sucursal --", ""));
        }

        // 🔹 Al cambiar la sucursal, se cargan las canchas
        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCancha.Items.Clear();
            ddlFecha.Items.Clear();

            if (!string.IsNullOrEmpty(ddlSucursal.SelectedValue))
            {
                int idSucursal = int.Parse(ddlSucursal.SelectedValue);
                var canchas = canchaNegocio.ListarPorSucursal(idSucursal);

                ddlCancha.DataSource = canchas;
                ddlCancha.DataTextField = "Nombre";
                ddlCancha.DataValueField = "CanchaID";
                ddlCancha.DataBind();

                ddlCancha.Items.Insert(0, new ListItem("-- Seleccione una cancha --", ""));
            }
        }

        // 🔹 Al cambiar la cancha, se cargan las fechas disponibles
        protected void ddlCancha_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFecha.Items.Clear();

            if (!string.IsNullOrEmpty(ddlCancha.SelectedValue))
            {
                int idCancha = int.Parse(ddlCancha.SelectedValue);
                var fechas = reservasNegocio.ListarFechasDisponiblesPorCancha(idCancha);

                ddlFecha.DataSource = fechas;
                ddlFecha.DataTextField = "Fecha";
                ddlFecha.DataValueField = "Fecha";
                ddlFecha.DataBind();

                ddlFecha.Items.Insert(0, new ListItem("-- Seleccione una fecha --", ""));
            }
        }

        // 🔹 Al cambiar la fecha, se filtran las reservas
        protected void ddlFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCancha.SelectedValue) && !string.IsNullOrEmpty(ddlFecha.SelectedValue))
            {
                int idCancha = int.Parse(ddlCancha.SelectedValue);
                DateTime fecha = DateTime.Parse(ddlFecha.SelectedValue);

                var reservas = reservasNegocio.ListarPorCanchaYFecha(idCancha, fecha);
                gvCanchas.DataSource = reservas;
                gvCanchas.DataBind();
            }
        }

        // 🔹 Botón para eliminar reservas seleccionadas
        protected void btnEliminarReserva_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvCanchas.Rows)
            {
                var chk = row.FindControl("chkSelect") as CheckBox;
                var hdn = row.FindControl("hdnId") as HiddenField;

                if (chk != null && chk.Checked && hdn != null)
                {
                    int id = int.Parse(hdn.Value);
                    reservasNegocio.EliminarReserva(id);
                }
            }

            CargarReservas(); // refrescar la grilla
        }

        // 🔹 Redirigir a la página de edición
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Editar.aspx");
        }
    }
}

