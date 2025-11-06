using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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

                ddlSucursal.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione una sucursal --", "0"));
            }
        }

        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSucursal.SelectedValue != "0")
            {
                CargarCanchas(Convert.ToInt32(ddlSucursal.SelectedValue));
            }
        }

        private void CargarCanchas(int idSucursal)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT CanchaID, Nombre FROM Canchas WHERE SucursalID = @SucursalID", con);
                cmd.Parameters.AddWithValue("@SucursalID", idSucursal);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                gvCanchas.DataSource = dr;
                gvCanchas.DataBind();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvCanchas.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                if (chk.Checked)
                {
                    int id = int.Parse(hdnId.Value);
                    EliminarCancha(id);
                }
            }

            if (ddlSucursal.SelectedValue != "0")
                CargarCanchas(Convert.ToInt32(ddlSucursal.SelectedValue));
        }

        private void EliminarCancha(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Canchas WHERE CanchaID = @CanchaID", con);
                cmd.Parameters.AddWithValue("@CanchaID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ddlSucursal.SelectedValue == "0")
                return;

            int sucursalId = Convert.ToInt32(ddlSucursal.SelectedValue);
            AgregarCancha(sucursalId);
            CargarCanchas(sucursalId); 
        }

        private void AgregarCancha(int sucursalId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM Canchas WHERE SucursalID = @SucursalID", con);
                countCmd.Parameters.AddWithValue("@SucursalID", sucursalId);

                con.Open();
                int cantidadActual = (int)countCmd.ExecuteScalar();
                con.Close();

                string nuevoNombre = "Cancha" + (cantidadActual + 1);

                SqlCommand insertCmd = new SqlCommand("INSERT INTO Canchas (Nombre, SucursalID) VALUES (@Nombre, @SucursalID)", con);
                insertCmd.Parameters.AddWithValue("@Nombre", nuevoNombre);
                insertCmd.Parameters.AddWithValue("@SucursalID", sucursalId);

                con.Open();
                insertCmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}