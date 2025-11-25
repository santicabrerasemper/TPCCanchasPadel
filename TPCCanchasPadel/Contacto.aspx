<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Canchas.master" CodeBehind="Contacto.aspx.cs" Inherits="TPCCanchasPadel.Contacto" %>

<asp:Content ID="headContacto" ContentPlaceHolderID="head" runat="server">

    <style>
        .contact-hero{ background-color:#0b1f17; min-height:40vh; display:flex; align-items:center; text-align:center; color:#fff; }
        .contact-hero h1{ font-weight:800; letter-spacing:.5px; margin-bottom:.5rem; }
        .contact-hero h1 span{ color:#ffd000; }
        .contact-wrap{ max-width:760px; }
        .contact-card{ background:#fff; border:1px solid rgba(0,0,0,.08); border-radius:.5rem; box-shadow:0 6px 24px rgba(0,0,0,.08); padding:1.5rem; }
    </style>
</asp:Content>

<asp:Content ID="contenidoContacto" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <section class="contact-hero">
        <div class="container py-4">
            <h1 class="mb-2">Contactanos <span>ahora</span></h1>
            <p class="lead mb-0">Te ayudamos a reservar tu cancha en minutos.</p>
        </div>
    </section>

    <div class="container py-5 contact-wrap">
        <div class="contact-card">
            <h2 class="mb-3 fw-bold">Dejanos tu consulta</h2>
            <p class="mb-4"> Completá el formulario y nos pondremos en contacto. También podés escribir a <a href="mailto:consultas@dondejuego.com">consultas@dondejuego.com</a>.</p>

            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre y apellido *</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej. María González" MaxLength="80" minlength="2" required="required" inputmode="text" pattern="^(?! )(?!.* {2,})(?!.* $)[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:[ '\-][A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$" />
                <div class="invalid-feedback">Ingresá un nombre válido (solo letras, sin dobles espacios).</div>
            </div>

            <div class="mb-3">
                <label for="txtEmail" class="form-label">Correo electrónico *</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="tuemail@ejemplo.com" required="required" autocomplete="email" MaxLength="120" />
                <div class="invalid-feedback">Ingresá un email válido.</div>+
            </div>

            <div class="row">
                <div class="col-md-4 mb-3">
                    <label class="form-label">País</label>
                    <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-select"> <asp:ListItem Text="Argentina (+54)" Value="+54" /></asp:DropDownList>
                </div>
                <div class="col-md-8 mb-3">
                    <label for="txtTelefono" class="form-label">Teléfono *</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej. 3412345678" required="required" inputmode="tel" MaxLength="15" pattern="^\d{6,15}$" />
                    <div class="invalid-feedback">Ingresá solo números (6 a 15 dígitos).</div>
                </div>
            </div>

            <div class="mb-3">
                <label for="txtMensaje" class="form-label">Mensaje *</label>
                <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="800" minlength="5" required="required" placeholder="Contanos brevemente tu consulta" />
                <div class="invalid-feedback">El mensaje es obligatorio (mínimo 5 caracteres).</div>
            </div>

            <div class="d-flex align-items-center gap-3">
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-success px-4" OnClick="btnEnviar_Click" UseSubmitBehavior="false" />
                <asp:Label ID="lblResultado" runat="server" CssClass="text-success d-block" />
            </div>
        </div>
    </div>

    <script>
        (() => {
            'use strict';
            const formulario = document.getElementById('form1');
            const nombre = document.getElementById('<%= txtNombre.ClientID %>');
            const email = document.getElementById('<%= txtEmail.ClientID %>');
            const telefono = document.getElementById('<%= txtTelefono.ClientID %>');
            const mensaje  = document.getElementById('<%= txtMensaje.ClientID %>');

            function recortarCampo(el) {
                if (!el) return;
                const v = el.value;
                const t = v.replace(/^\s+|\s+$/g, '').replace(/\s{2,}/g, ' ');
                if (t !== v) el.value = t;
            }

            function bloquearEspacios(el) {
                if (!el) return;
                el.addEventListener('keydown', e => { if (e.key === ' ') e.preventDefault(); });
                el.addEventListener('input', () => { el.value = el.value.replace(/\s+/g, ''); });
                el.addEventListener('paste', e => {
                    e.preventDefault();
                    const texto = (e.clipboardData || window.clipboardData).getData('text') || '';
                    el.value += texto.replace(/\s+/g, '');
                });
            }

            function soloDigitos(el) {
                if (!el) return;
                el.addEventListener('input', () => { el.value = el.value.replace(/\D+/g, ''); });
                el.addEventListener('paste', e => {
                    e.preventDefault();
                    const texto = (e.clipboardData || window.clipboardData).getData('text') || '';
                    el.value += texto.replace(/\D+/g, '');
                });
            }

            nombre && nombre.addEventListener('blur', () => recortarCampo(nombre));
            if (email) { bloquearEspacios(email); email.addEventListener('blur', () => recortarCampo(email)); }
            soloDigitos(telefono);
            mensaje && mensaje.addEventListener('blur', () => recortarCampo(mensaje));

            if (formulario) {
                formulario.addEventListener('submit', (e) => {
                    [nombre, email, telefono, mensaje].forEach(recortarCampo);
                    if (!formulario.checkValidity()) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    formulario.classList.add('was-validated');
                });
            }
        })();
    </script>
</asp:Content>
