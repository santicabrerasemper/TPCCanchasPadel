<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="TPCCanchasPadel.Registro" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8" />
  <title>Crear cuenta</title>
  <link href="Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server" class="needs-validation" novalidate>
  <div class="mx-auto" style="max-width:420px; margin-top: 50px;">
    <h2 class="mb-3">Crear cuenta</h2>

    <div class="mb-3">
      <label for="txtUsuario" class="form-label">Usuario</label>
      <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"
        MaxLength="20" minlength="4" required="required"
        autocomplete="username" inputmode="text" spellcheck="false"
        pattern="^(?![._-])(?:[A-Za-z0-9]|[._-](?![._-])){3,18}[A-Za-z0-9]$"
        placeholder="ej: juan.perez" />
      <div class="valid-feedback">Se ve bien.</div>
      <div class="invalid-feedback">4–20 caracteres. Solo letras, números, punto, guion y guion bajo.</div>
    </div>

    <div class="mb-3">
      <label for="txtNombre" class="form-label">Nombre</label>
      <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"
        MaxLength="50" minlength="2" required="required" inputmode="text"
        autocomplete="given-name"
        pattern="^(?! )(?!.* {2,})(?!.* $)[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ '\-][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$"
        placeholder="Nombre" />
      <div class="invalid-feedback">
        Solo letras (con tildes). Se permiten espacios simples, guion y apóstrofe.    
      </div>
    </div>

    <div class="mb-3">
      <label for="txtApellido" class="form-label">Apellido</label>
      <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"
        MaxLength="50" minlength="2" required="required" inputmode="text"
        autocomplete="family-name"
        pattern="^(?! )(?!.* {2,})(?!.* $)[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ '\-][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$"
        placeholder="Apellido" />
      <div class="invalid-feedback">
        Solo letras (con tildes). Se permiten espacios simples, guion y apóstrofe.
      </div>
    </div>

    <div class="mb-3">
      <label for="txtPassword" class="form-label">Contraseña</label>
      <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
        TextMode="Password" required="required" autocomplete="new-password" spellcheck="false"
        pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*_+\-])[A-Za-z\d!@#$%^&*_+\-]{6,50}$"
        placeholder="ej: Prog3#" />
      <div class="form-text">
        Debe tener al menos 6 caracteres e incluir una mayúscula, una minúscula, un número y un caracter especial
        (! @ # $ % ^ & * _ + -).
      </div>
      <div class="invalid-feedback">La contraseña no cumple los requisitos.</div>
    </div>

    <div class="mb-3">
      <label for="txtConfirm" class="form-label">Confirmar contraseña</label>
      <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control"
        TextMode="Password" required="required" placeholder="Repetí la contraseña" />
      <div id="confirmFeedback" class="invalid-feedback d-none" aria-live="polite">
        Las contraseñas deben coincidir.
      </div>
    </div>

    <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-success w-100" Text="Crear cuenta"  OnClick="btnRegistrar_Click" />
    <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mt-3"></asp:Label>

    <div class="mt-3 text-center">
      <small>¿Ya tenés cuenta? <a href="Login.aspx">Iniciá sesión</a>.</small>
    </div>
  </div>
</form>

<script src="Scripts/bootstrap.bundle.js"></script>
<script>
    (() => {
        'use strict';
        const form = document.getElementById('form1');

        const user = document.getElementById('<%= txtUsuario.ClientID %>');
        const nameEl = document.getElementById('<%= txtNombre.ClientID %>');
        const lastEl = document.getElementById('<%= txtApellido.ClientID %>');
        const pass = document.getElementById('<%= txtPassword.ClientID %>');
        const confirm = document.getElementById('<%= txtConfirm.ClientID %>');
        const cfb = document.getElementById('confirmFeedback');

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

        [nameEl, lastEl].forEach(el => el && el.addEventListener('blur', () => trimField(el)));

        function updateConfirmState() {
            if (!pass.value || !pass.checkValidity()) {
                confirm.setCustomValidity('');
                cfb.classList.add('d-none');
                if (form.classList.contains('was-validated')) {
                    confirm.classList.remove('is-valid', 'is-invalid');
                }
                return;
            }

            if (!confirm.value) {       
                confirm.setCustomValidity('no-match');
                cfb.classList.add('d-none');
                if (form.classList.contains('was-validated')) {
                    confirm.classList.remove('is-valid', 'is-invalid');
                }
                return;
            }

            const ok = (confirm.value === pass.value);
            confirm.setCustomValidity(ok ? '' : 'no-match');
            cfb.classList.toggle('d-none', ok);

            if (form.classList.contains('was-validated')) {
                confirm.classList.toggle('is-valid', ok);
                confirm.classList.toggle('is-invalid', !ok);
            }
        }

        function blockSpaces(el) {
            if (!el) return;

            el.addEventListener('keydown', (e) => {
                if (e.key === ' ') e.preventDefault();
            });

            el.addEventListener('input', () => {
                const cleaned = el.value.replace(/\s+/g, '');
                if (el.value !== cleaned) {
                    el.value = cleaned;
                    updateConfirmState();
                }
            });

            el.addEventListener('paste', (e) => {
                e.preventDefault();
                const text = (e.clipboardData || window.clipboardData).getData('text') || '';
                el.value += text.replace(/\s+/g, '');
                updateConfirmState();
            });
        }

        blockSpaces(pass);
        blockSpaces(confirm);

        pass.addEventListener('input', updateConfirmState);
        confirm.addEventListener('input', updateConfirmState);

        form.addEventListener('submit', (e) => {
            [user, nameEl, lastEl].forEach(trimField);

            updateConfirmState();
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
