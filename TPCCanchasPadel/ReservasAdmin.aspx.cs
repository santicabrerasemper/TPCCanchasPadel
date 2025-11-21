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

        private void CargarReservas()
        {
            var lista = reservasNegocio.ListarReservas();
            gvCanchas.DataSource = lista;
            gvCanchas.DataBind();
        }

        private void CargarSucursales()
        {
            var sucursales = sucursalNegocio.ListarSucursales();
            ddlSucursal.DataSource = sucursales;
            ddlSucursal.DataTextField = "Nombre";
            ddlSucursal.DataValueField = "SucursalID";
            ddlSucursal.DataBind();

            ddlSucursal.Items.Insert(0, new ListItem("-- Seleccione una sucursal --", ""));
        }

        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCancha.Items.Clear();
            ddlFecha.Items.Clear();

            if (!string.IsNullOrEmpty(ddlSucursal.SelectedValue))
            {
                int SucursalID = int.Parse(ddlSucursal.SelectedValue);
                var canchas = canchaNegocio.ListarPorSucursal(SucursalID);

                ddlCancha.DataSource = canchas;
                ddlCancha.DataTextField = "Nombre";
                ddlCancha.DataValueField = "CanchaID";
                ddlCancha.DataBind();

                ddlCancha.Items.Insert(0, new ListItem("-- Seleccione una cancha --", ""));
            }
            CargarReservasFiltradas();
        }

        protected void ddlCancha_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFecha.Items.Clear();

            if (!string.IsNullOrEmpty(ddlCancha.SelectedValue))
            {
                int canchaID = int.Parse(ddlCancha.SelectedValue);
                var fechas = reservasNegocio.ListarFechasDisponiblesPorCancha(canchaID);

                ddlFecha.DataSource = fechas.Select(f => new
                {
                    FechaTexto = f.ToString("dd/MM/yyyy"),
                    FechaValor = f.ToString("o") 
                }).ToList();

                ddlFecha.DataTextField = "FechaTexto";
                ddlFecha.DataValueField = "FechaValor";
                ddlFecha.DataBind();

                ddlFecha.Items.Insert(0, new ListItem("-- Seleccione una fecha --", ""));
            }
            CargarReservasFiltradas();
        }

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

            CargarReservas(); 
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Editar.aspx");
        }
        
        protected void btnEditarSucursales_Click(object sender, EventArgs e)
        {
            Response.Redirect("Editar.aspx");
        }

        protected void btnAgregarReserva_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClienteCanchas.aspx");
        }

        private void CargarReservasFiltradas()
        {
            int tempInt;
            int? sucursalId = int.TryParse(ddlSucursal.SelectedValue, out tempInt) ? (int?)tempInt : null;
            int? canchaId = int.TryParse(ddlCancha.SelectedValue, out tempInt) ? (int?)tempInt : null;

            DateTime tempDate;
            DateTime? fecha = DateTime.TryParse(ddlFecha.SelectedValue, out tempDate) ? (DateTime?)tempDate : null;

            var reservas = reservasNegocio.ListarFiltradas(sucursalId, canchaId, fecha);
            gvCanchas.DataSource = reservas;
            gvCanchas.DataBind();
        }

    }
}

