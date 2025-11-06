using Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class Editar : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CanchasBD"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RolID"] == null || Convert.ToInt32(Session["RolID"]) != 1)
            {
                Response.Redirect("ClienteCanchas.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarSucursales();
            }
        }

        private void CargarSucursales()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT SucursalID, Nombre FROM Sucursales", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlSucursal.DataSource = dr;
                ddlSucursal.DataTextField = "Nombre";
                ddlSucursal.DataValueField = "SucursalID";
                ddlSucursal.DataBind();

                ddlSucursal.Items.Insert(0, new ListItem("-- Seleccione una sucursal --", "0"));
            }
        }

        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCanchas();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCanchas();
        }

        private void CargarCanchas()
        {
            if (ddlSucursal.SelectedValue == "0")
            {
                gvCanchas.DataSource = null;
                gvCanchas.DataBind();
                return;
            }

            string query = "SELECT CanchaID, Nombre, EstadoID FROM Canchas WHERE SucursalID = @SucursalID";

            if (ddlEstado.SelectedValue == "1") // Activos
                query += " AND EstadoID = (SELECT EstadoID FROM Estados WHERE Nombre = 'Activo')";
            else if (ddlEstado.SelectedValue == "2") // Inactivos
                query += " AND EstadoID = (SELECT EstadoID FROM Estados WHERE Nombre = 'Inactivo')";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SucursalID", ddlSucursal.SelectedValue);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                gvCanchas.DataSource = dr;
                gvCanchas.DataBind();
            }
        }

        protected void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvCanchas.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk != null && chk.Checked)
                {
                    int canchaId = Convert.ToInt32(gvCanchas.DataKeys[row.RowIndex].Value);
                    CambiarEstadoCancha(canchaId);
                }
            }

            CargarCanchas();
        }

        private void CambiarEstadoCancha(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Alternar entre activo/inactivo
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Canchas
                    SET EstadoID = CASE 
                        WHEN EstadoID = (SELECT EstadoID FROM Estados WHERE Nombre = 'Activo') 
                            THEN (SELECT EstadoID FROM Estados WHERE Nombre = 'Inactivo')
                        ELSE (SELECT EstadoID FROM Estados WHERE Nombre = 'Activo')
                    END
                    WHERE CanchaID = @CanchaID", con);

                cmd.Parameters.AddWithValue("@CanchaID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ddlSucursal.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Seleccione una sucursal antes de agregar una cancha.');", true);
                return;
            }

            string nuevaCanchaNombre = ObtenerNuevoNombre(Convert.ToInt32(ddlSucursal.SelectedValue));

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Canchas (Nombre, SucursalID, EstadoID)
                    VALUES (@Nombre, @SucursalID, (SELECT EstadoID FROM Estados WHERE Nombre = 'Activo'))", con);

                cmd.Parameters.AddWithValue("@Nombre", nuevaCanchaNombre);
                cmd.Parameters.AddWithValue("@SucursalID", ddlSucursal.SelectedValue);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            CargarCanchas();
        }

        private string ObtenerNuevoNombre(int sucursalId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Canchas WHERE SucursalID = @SucursalID", con);
                cmd.Parameters.AddWithValue("@SucursalID", sucursalId);
                con.Open();
                int cantidad = (int)cmd.ExecuteScalar();
                return "Cancha" + (cantidad + 1);
            }
        }

    }
}
