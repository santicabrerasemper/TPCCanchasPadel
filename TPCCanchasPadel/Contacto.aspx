<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contacto.aspx.cs" Inherits="TPCCanchasPadel.Contacto" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8" />
  <title>Contacto - Alquiler de Canchas</title>
  <link href="Content/bootstrap.css" rel="stylesheet" />
  <style>
    .contact-hero{
      background-color:#0b1f17; 
      min-height:40vh;
      display:flex;
      align-items:center;
      text-align:center;
      color:#fff;
    }
    .contact-hero h1{
      font-weight:800; letter-spacing:.5px; margin-bottom:.5rem;
    }
    .contact-hero h1 span{ color:#ffd000; }

    .contact-wrap{
      max-width: 760px;
    }
    .contact-card{
      background:#fff;
      border:1px solid rgba(0,0,0,.08);
      border-radius:.5rem;
      box-shadow:0 6px 24px rgba(0,0,0,.08);
      padding:1.5rem;
    }
  </style>
</head>
<body>
  <form id="form1" runat="server">
    
    <section class="contact-hero">
      <div class="container py-4">
        <h1 class="mb-2">Contactanos <span>ahora</span></h1>
        <p class="lead mb-0">Te ayudamos a reservar tu cancha en minutos.</p>
      </div>
    </section>

    <section class="py-5">
      <div class="container contact-wrap">
        <div class="contact-card">
          <h2 class="mb-3 fw-bold">Dejanos tu consulta</h2>
          <p class="mb-4">
            Completá el formulario y nos pondremos en contacto para ayudarte con la reserva.
            También podés escribirnos a
            <a href="mailto:consultas@dondejuego.com">consultas@dondejuego.com</a>.
          </p>

          <div class="mb-3">
            <label for="txtNombre" class="form-label">Nombre y apellido *</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Placeholder="Ej. María González" />
          </div>

          <div class="mb-3">
            <label for="txtEmail" class="form-label">Correo electrónico *</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" Placeholder="tuemail@ejemplo.com" />
          </div>

          <div class="row">
            <div class="col-md-4 mb-3">
              <label class="form-label">País</label>
              <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-select">
                <asp:ListItem Text="Argentina (+54)" Value="+54" />
              </asp:DropDownList>
            </div>
            <div class="col-md-8 mb-3">
              <label for="txtTelefono" class="form-label">Teléfono *</label>
              <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" Placeholder="Ej. 3412345678" />
            </div>
          </div>

          <div class="mb-3">
            <label for="txtMensaje" class="form-label">Mensaje *</label>
            <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"
              Placeholder="Ej. Estoy interesado en reservar una cancha de padel por mi cumpleaños." />
          </div>

          <div class="d-flex align-items-center gap-3">
            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-success px-4"
              OnClick="btnEnviar_Click" UseSubmitBehavior="false" />
            <asp:Label ID="lblResultado" runat="server" CssClass="text-success d-block" />
          </div>
        </div>
      </div>
    </section>

  </form>
  <script src="Scripts/bootstrap.bundle.js"></script>
</body>
</html>
