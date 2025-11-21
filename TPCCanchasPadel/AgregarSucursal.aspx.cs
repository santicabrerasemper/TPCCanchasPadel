using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class AgregarSucursal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnConfirmar_Click(object Sender, EventArgs e)
        {
            var nuevo = new Sucursal
            {
                //NombreSucursal = txtNombre.Text,
                //NombreLocalidad = txtLocalidad.Text,
                //FotoLocalidad = txtFoto.Text,
                //DireccionLocalidad = txtDireccion.Text,
                //DescripcionLocalidad = txtDescripcion.Text
            };
        }

        protected void btnCancelar_Click(object Sender, EventArgs e)
        {
            Response.Redirect("ReservasAdmin.aspx");
        }

    }
}