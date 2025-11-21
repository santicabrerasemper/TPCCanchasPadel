using ConexionBD;
using Dominio;
using Negocio;
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

            if (string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtLocalidad.Text) ||
                string.IsNullOrEmpty(txtFoto.Text)
                )
            {
                MostrarMensaje("⚠️ Completá todos los campos.", "warning");
                return;
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                // 1️⃣ Obtener próximo LocalidadID
                datos.setearConsulta("SELECT ISNULL(MAX(LocalidadID), 0) + 1 AS NextId FROM Localidades");
                datos.ejecutarLectura();
                datos.Lector.Read();
                int nuevoLocalidadId = Convert.ToInt32(datos.Lector["NextId"]);
                datos.cerrarConexion();

                // 2️⃣ Insertar nueva Localidad
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Localidades (LocalidadID, Nombre) VALUES (@id, @nombre)");
                datos.setearParametro("@id", nuevoLocalidadId);
                datos.setearParametro("@nombre", txtLocalidad.Text);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // 3️⃣ Obtener próximo SucursalID
                datos = new AccesoDatos();
                datos.setearConsulta("SELECT ISNULL(MAX(SucursalID), 0) + 1 AS NextId FROM Sucursales");
                datos.ejecutarLectura();
                datos.Lector.Read();
                int nuevaSucursalId = Convert.ToInt32(datos.Lector["NextId"]);
                datos.cerrarConexion();

                // 4️⃣ Insertar nueva Sucursal
                datos = new AccesoDatos();
                datos.setearConsulta(@"
            INSERT INTO Sucursales (SucursalID, LocalidadID, Nombre, FotoUrl)
            VALUES (@id, @loc, @nom, @foto)
        ");

                datos.setearParametro("@id", nuevaSucursalId);
                datos.setearParametro("@loc", nuevoLocalidadId);
                datos.setearParametro("@nom", txtNombre.Text);
                datos.setearParametro("@foto", txtFoto.Text);

                datos.ejecutarAccion();
                datos.cerrarConexion();

                MostrarMensaje("Sucursal creada con éxito 🎉", "success");
            }
            catch (Exception ex)
            {
                MostrarMensaje("❌ Error: " + ex.Message, "danger");
            }
        }

        protected void btnCancelar_Click(object Sender, EventArgs e)
        {
            Response.Redirect("ReservasAdmin.aspx");
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            string color, icono, titulo;

            switch (tipo)
            {
                case "success":
                    color = "border-success text-success bg-light";
                    icono = "✅";
                    titulo = "¡Reserva confirmada!";
                    break;
                case "warning":
                    color = "border-warning text-warning bg-light";
                    icono = "⚠️";
                    titulo = "Atención";
                    break;
                case "danger":
                    color = "border-danger text-danger bg-light";
                    icono = "❌";
                    titulo = "Error";
                    break;
                default:
                    color = "border-info text-info bg-light";
                    icono = "ℹ️";
                    titulo = "Información";
                    break;
            }

            lblMensaje.Text = $@"
            <div class='alert {color} p-4 rounded-3 shadow-sm mx-auto text-center' 
                 style='max-width:500px; animation: fadeIn 0.6s ease-in-out;'>
                <h5 class='fw-bold mb-2'>{icono} {titulo}</h5>
                <p class='mb-0'>{texto}</p>
            </div>

            <style>
                @keyframes fadeIn {{
                    from {{ opacity: 0; transform: translateY(-10px); }}
                    to {{ opacity: 1; transform: translateY(0); }}
                }}
            </style>";
        }

    }
}