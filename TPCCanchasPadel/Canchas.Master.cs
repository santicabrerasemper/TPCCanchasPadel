using System;
using System.Web;
using System.Web.UI;
using Negocio;

namespace TPCCanchasPadel
{
    public partial class Canchas : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var file = VirtualPathUtility.GetFileName(Request.AppRelativeCurrentExecutionFilePath) ?? string.Empty;

            bool esLogin =
                this.Page is TPCCanchasPadel.Login ||
                file.Equals("Login", StringComparison.OrdinalIgnoreCase) ||
                file.Equals("Login.aspx", StringComparison.OrdinalIgnoreCase);

            bool esRegistro =
                this.Page is TPCCanchasPadel.Registro ||
                file.Equals("Registro", StringComparison.OrdinalIgnoreCase) ||
                file.Equals("Registro.aspx", StringComparison.OrdinalIgnoreCase);

            bool esAuth = esLogin || esRegistro;

            var haySesion = Seguridad.SesionActiva(Session);

            phAnonimo.Visible = !haySesion && !esAuth;
            phAutenticado.Visible = haySesion;
            liInicio.Visible = !haySesion;

            lnkMarca.NavigateUrl = ResolveUrl(haySesion ? "~/ClienteCanchas.aspx" : "~/Home.aspx");
        }
    }
}
