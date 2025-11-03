<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TPCCanchasPadel.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Iniciar sesión</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server" class="needs-validation" novalidate>
    <div class="mx-auto" style="max-width:420px; margin-top: 50px;">
        <h2 class="mb-3">Iniciar sesión</h2>

        <div class="mb-3">
            <label for="txtUsuario" class="form-label">Usuario</label>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"
                MaxLength="20" minlength="4" required="required"
                autocomplete="username" inputmode="text" spellcheck="false"
                pattern="^(?![._-])(?:[A-Za-z0-9]|[._-](?![._-])){3,18}[A-Za-z0-9]$" />
            <div class="invalid-feedback">Por favor, ingresá datos correctos.</div>
        </div>

        <div class="mb-3">
            <label for="txtPassword" class="form-label">Contraseña</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                TextMode="Password" required="required"
                autocomplete="current-password" spellcheck="false" />
            <div class="invalid-feedback">Por favor, ingresá datos correctos.</div>
        </div>

        <asp:Button ID="btnIngresar" runat="server"
            CssClass="btn btn-success w-100" Text="Ingresar"
            OnClick="btnIngresar_Click" />
        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger d-block mt-3" />

        <div class="mt-3 text-center">
            <small>¿No tenés usuario? <a href="Registro.aspx">Crealo acá</a>.</small>
        </div>
    </div>
</form>

<script src="Scripts/bootstrap.bundle.js"></script>
<script>
    (() => {
        'use strict';
        const form = document.getElementById('form1');
        const user = document.getElementById('<%= txtUsuario.ClientID %>');
  const pass = document.getElementById('<%= txtPassword.ClientID %>');

        function trimField(el) {
            if (!el) return;
            const v = el.value;
            const t = v.replace(/^\s+|\s+$/g, '').replace(/\s{2,}/g, ' ');
            if (t !== v) el.value = t;
        }

        if (user) {
            user.addEventListener('input', () => {
                user.value = user.value.replace(/\s+/g, '');
            });
            user.addEventListener('blur', () => trimField(user));
        }

        function blockSpaces(el) {
            if (!el) return;
            el.addEventListener('keydown', (e) => { if (e.key === ' ') e.preventDefault(); });
            el.addEventListener('input', () => { el.value = el.value.replace(/\s+/g, ''); });
            el.addEventListener('paste', (e) => {
                e.preventDefault();
                const text = (e.clipboardData || window.clipboardData).getData('text') || '';
                el.value += text.replace(/\s+/g, '');
            });
        }
        blockSpaces(pass);

        form.addEventListener('submit', (e) => {
            [user].forEach(trimField);

            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    })();
</script>
</body>
</html>
