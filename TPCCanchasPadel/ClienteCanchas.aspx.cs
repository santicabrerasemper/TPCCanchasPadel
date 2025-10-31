using System;
using System.Collections.Generic;
using System.Web.UI;
using Dominio;
using Negocio;

namespace TPCCanchasPadel
{
    public partial class ClienteCanchas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvCanchas.Visible = false;
                lblMensaje.Text = "";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que todos los campos estén completos
                if (string.IsNullOrEmpty(txtFecha.Text) ||
                    string.IsNullOrEmpty(txtHoraInicio.Text) ||
                    string.IsNullOrEmpty(txtHoraFin.Text))
                {
                    lblMensaje.Text = "Por favor, completá todos los campos antes de buscar.";
                    gvCanchas.Visible = false;
                    return;
                }

                // Convertir los valores del formulario
                DateTime fecha = DateTime.Parse(txtFecha.Text);
                TimeSpan horaInicio = TimeSpan.Parse(txtHoraInicio.Text);
                TimeSpan horaFin = TimeSpan.Parse(txtHoraFin.Text);

                // Validar lógica horaria
                if (horaInicio >= horaFin)
                {
                    lblMensaje.Text = "La hora de inicio debe ser menor que la hora de fin.";
                    gvCanchas.Visible = false;
                    return;
                }

                // Llamar a la capa de negocio
                ReservasNegocio negocio = new ReservasNegocio();
                List<Cancha> disponibles = negocio.ListarCanchasDisponibles(fecha, horaInicio, horaFin);

                // Mostrar resultados
                if (disponibles.Count == 0)
                {
                    lblMensaje.Text = "No hay canchas disponibles en ese horario.";
                    gvCanchas.Visible = false;
                }
                else
                {
                    gvCanchas.DataSource = disponibles;
                    gvCanchas.DataBind();
                    gvCanchas.Visible = true;
                    lblMensaje.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Ocurrió un error al buscar las canchas: " + ex.Message;
                gvCanchas.Visible = false;
            }
        }
    }
}