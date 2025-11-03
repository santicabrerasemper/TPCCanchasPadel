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
        const formulario = document.getElementById('form1');

        const usuario = document.getElementById('<%= txtUsuario.ClientID %>');
    const nombreEl = document.getElementById('<%= txtNombre.ClientID %>');
    const apellidoEl = document.getElementById('<%= txtApellido.ClientID %>');
  const contrasena = document.getElementById('<%= txtPassword.ClientID %>');
  const confirmar  = document.getElementById('<%= txtConfirm.ClientID %>');
        const fbConfirmacion = document.getElementById('confirmFeedback');

        function recortarCampo(el) {
            if (!el) return;
            const valor = el.value;
            const recortado = valor.replace(/^\s+|\s+$/g, '').replace(/\s{2,}/g, ' ');
            if (recortado !== valor) el.value = recortado;
        }

        if (usuario) {
            usuario.addEventListener('input', () => {
                usuario.value = usuario.value.replace(/\s+/g, '');
            });
            usuario.addEventListener('blur', () => recortarCampo(usuario));
        }

        [nombreEl, apellidoEl].forEach(el => el && el.addEventListener('blur', () => recortarCampo(el)));

        function actualizarEstadoConfirmacion() {
            if (!contrasena.value || !contrasena.checkValidity()) {
                confirmar.setCustomValidity('');
                fbConfirmacion.classList.add('d-none');
                if (formulario.classList.contains('was-validated')) {
                    confirmar.classList.remove('is-valid', 'is-invalid');
                }
                return;
            }

            if (!confirmar.value) {
                confirmar.setCustomValidity('no-match');
                fbConfirmacion.classList.add('d-none');
                if (formulario.classList.contains('was-validated')) {
                    confirmar.classList.remove('is-valid', 'is-invalid');
                }
                return;
            }

            const ok = (confirmar.value === contrasena.value);
            confirmar.setCustomValidity(ok ? '' : 'no-match');
            fbConfirmacion.classList.toggle('d-none', ok);

            if (formulario.classList.contains('was-validated')) {
                confirmar.classList.toggle('is-valid', ok);
                confirmar.classList.toggle('is-invalid', !ok);
            }
        }

        function bloquearEspacios(el) {
            if (!el) return;

            el.addEventListener('keydown', (e) => {
                if (e.key === ' ') e.preventDefault();
            });

            el.addEventListener('input', () => {
                const limpio = el.value.replace(/\s+/g, '');
                if (el.value !== limpio) {
                    el.value = limpio;
                    actualizarEstadoConfirmacion();
                }
            });

            el.addEventListener('paste', (e) => {
                e.preventDefault();
                const texto = (e.clipboardData || window.clipboardData).getData('text') || '';
                el.value += texto.replace(/\s+/g, '');
                actualizarEstadoConfirmacion();
            });
        }

        bloquearEspacios(contrasena);
        bloquearEspacios(confirmar);

        contrasena.addEventListener('input', actualizarEstadoConfirmacion);
        confirmar.addEventListener('input', actualizarEstadoConfirmacion);

        formulario.addEventListener('submit', (e) => {
            [usuario, nombreEl, apellidoEl].forEach(recortarCampo);

            actualizarEstadoConfirmacion();
            if (!formulario.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            formulario.classList.add('was-validated');
        });
    })();
</script>
</body>
</html>
