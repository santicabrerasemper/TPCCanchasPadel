<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contacto.aspx.cs" Inherits="TPCCanchasPadel.Contacto" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Contacto - Alquiler de Canchas</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-5">
            <h2 class="mb-3 fw-bold">
                ¿Interesado en alquilar <span class="text-success">tu cancha deportiva?</span>
            </h2>
            <p class="mb-4 fs-5">
                Completá el formulario y nos pondremos en contacto para ayudarte con la reserva.
            </p>
            <p>También podés escribirnos a <a href="mailto:consultas@dondejuego.com">consultas@dondejuego.com</a></p>

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
                    Placeholder="Ej. Estoy interesado en reservar una cancha para fútbol 5 los sábados por la tarde." />
            </div>

            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-dark" />
            <asp:Label ID="lblResultado" runat="server" CssClass="text-success d-block mt-3" />
        </div>
    </form>
    <script src="Scripts/bootstrap.bundle.js"></script>
</body>
</html>

