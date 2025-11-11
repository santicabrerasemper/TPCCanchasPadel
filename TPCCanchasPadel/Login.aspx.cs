using System;
using Negocio;
using Dominio;

namespace TPCCanchasPadel
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly LoginNegocio _loginNegocio = new LoginNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Seguridad.CerrarSesion(this);

                if (Request.QueryString["registradoexitosamente"] == "1")
                {
                    lblMensaje.CssClass = "text-success d-block mt-3";
                    lblMensaje.Text = "¡Usuario registrado exitosamente! Iniciá sesión.";
                }

                if (Request.QueryString["exp"] == "1")
                {
                    lblMensaje.CssClass = "text-warning d-block mt-3";
                    lblMensaje.Text = "Tu sesión expiró. Volvé a iniciar sesión.";
                }
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            var usuarioInput = (txtUsuario.Text ?? string.Empty).Trim();
            var passwordInput = txtPassword.Text ?? string.Empty;

            var user = _loginNegocio.Autenticar(usuarioInput, passwordInput, out string msg);

            if (user == null)
            {
                lblMensaje.CssClass = "text-danger d-block mt-3";
                lblMensaje.Text = msg; 
                return;
            }

            Session["UsuarioID"] = user.UsuarioID;
            Session["Usuario"] = user.NombreUsuario;
            Session["RolID"] = user.RolID;

            var destino = user.RolID == Seguridad.RolAdmin ? "~/Editar.aspx" : "~/ClienteCanchas.aspx";
            Response.Redirect(ResolveUrl(destino), false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
