using ConexionBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class TestReservas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var reservasBD = new ReservasCanchas();
                gvReservas.DataSource = reservasBD.ObtenerTodasLasReservas();
                gvReservas.DataBind();
            }
        }
    }
}