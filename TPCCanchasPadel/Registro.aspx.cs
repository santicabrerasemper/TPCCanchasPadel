using System;
using Negocio;
using Dominio;

namespace TPCCanchasPadel
{
    public partial class Registro : System.Web.UI.Page
    {
        private readonly RegistroNegocio _registroNegocio = new RegistroNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            var nuevo = new Usuario
            {
                NombreUsuario = txtUsuario.Text,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Password = txtPassword.Text
            };
            if (_registroNegocio.IntentoRegistrar(nuevo, out string msg))
            {
                Response.Redirect("Login.aspx?registradoexitosamente=1", false);
                return;
            }

            else
            {
                lblMensaje.CssClass = "text-danger d-block mt-3";
                lblMensaje.Text = msg; 
            }
        }
    }
}
