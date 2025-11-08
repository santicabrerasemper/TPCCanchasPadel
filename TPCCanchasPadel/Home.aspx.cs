using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TPCCanchasPadel
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && string.Equals(Request["contacto"], "ok", StringComparison.OrdinalIgnoreCase))
            {
                var html = @"<div class='alert alert-success alert-dismissible fade show' role='alert'>
                       ¡Mensaje enviado! A la brevedad nos estaremos contactando contigo.
                       <button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>
                     </div>";
                phAlerta.Controls.Add(new Literal { Text = html });
            }
        }
    }
}