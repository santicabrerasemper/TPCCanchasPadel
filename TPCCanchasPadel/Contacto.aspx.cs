using System;
using Negocio;

namespace TPCCanchasPadel
{
    public partial class Contacto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            var nombre = (txtNombre.Text ?? "").Trim();
            var email = (txtEmail.Text ?? "").Trim();
            var telefono = (txtTelefono.Text ?? "").Trim();
            var prefijo = ddlPais.SelectedValue ?? "+54";
            var mensaje = (txtMensaje.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(mensaje))
            {
                lblResultado.CssClass = "text-danger d-block mt-3";
                lblResultado.Text = "Completá todos los campos obligatorios (*).";
                return;
            }

            try
            {
                var svc = new ServicioEmail();
                svc.EnviarContacto(nombre, email, $"{prefijo} {telefono}", mensaje);

                Response.Redirect("Home.aspx?contacto=ok", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch
            {
                lblResultado.CssClass = "text-danger d-block mt-3";
                lblResultado.Text = "No pudimos enviar tu mensaje. Intentá nuevamente en unos minutos.";
            }
        }
    }
}
