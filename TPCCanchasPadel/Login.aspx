<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPCCanchasPadel.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Iniciar sesión</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="mx-auto" style="max-width:420px; margin-top: 50px;">
            <h2 class="mb-3">Iniciar sesión</h2>

            <div class="mb-3">
                <label for="txtUsuario" class="form-label">Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
                <label for="txtPassword" class="form-label">Contraseña</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
            </div>

            <asp:Button ID="btnIngresar" runat="server" CssClass="btn btn-success w-100" Text="Ingresar" />
            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger d-block mt-3" />

            <div class="mt-3 text-center">
                <small>¿No tenés usuario?
                    <a href="RegistroCanchas.aspx">Crealo acá</a>.
                </small>
            </div>
        </div>
    </form>
    <script src="Scripts/bootstrap.bundle.js"></script>
</body>
</html>

