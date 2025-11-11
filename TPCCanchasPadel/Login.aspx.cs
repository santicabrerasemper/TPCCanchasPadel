using System;
using Negocio;
using Dominio;

namespace TPCCanchasPadel
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly LoginNegocio _loginNegocio = new LoginNegocio();

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack && Request.QueryString["registradoexitosamente"] == "1")
            {
                lblMensaje.CssClass = "text-success d-block mt-3";
                lblMensaje.Text = "¡Usuario registrado exitosamente! Iniciá sesión.";
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            var usuarioInput = (txtUsuario.Text ?? string.Empty);
            var passwordInput = txtPassword.Text ?? string.Empty;

            var user = _loginNegocio.Autenticar(usuarioInput, passwordInput, out string msg);

            if (user == null)
            {
                lblMensaje.Text = msg; 
                return;
            }

            Session["Usuario"] = user;
            Session["RolID"] = user.RolID;

            if (user.RolID == 1)
                Response.Redirect("Editar.aspx", false);
            else
                Response.Redirect("ClienteCanchas.aspx", false);
        }
    }
}
