<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarSucursal.aspx.cs" Inherits="TPCCanchasPadel.AgregarSucursal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Crear Nueva Sucursal</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <style>
      .registro-hero{
        background-color:#0b1f17; 
        min-height:40vh;
        display:flex;
        align-items:center;
        text-align:center;
        color:#fff;
      }
      .registro-hero h1{
        font-weight:800; letter-spacing:.5px; margin-bottom:.5rem;
      }
      .registro-hero h1 span{ color:#ffd000; }
    
      .registro-wrap{ max-width: 640px; }
      .registro-card{
        background:#fff;
        border:1px solid rgba(0,0,0,.08);
        border-radius:.5rem;
        box-shadow:0 6px 24px rgba(0,0,0,.08);
        padding:1.5rem;
      }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="needs-validation">
        <section class="py-5">
          <div class="container registro-wrap">
            <div class="registro-card">
              <h2 class="mb-3 fw-bold">Nueva Sucursal</h2>
        
              <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="20" minlength="4" inputmode="text" spellcheck="false" placeholder="ej: Sucursal Nueva" />
              </div>
        
              <div class="mb-3">
                <label for="txtLocalidad" class="form-label">Localidad</label>
                <asp:TextBox ID="txtLocalidad" runat="server" CssClass="form-control" MaxLength="20" minlength="4" inputmode="text" spellcheck="false" placeholder="ej: Palermo" />
              </div>
           
              <div class="mb-3">
                <label for="txtFoto" class="form-label">Foto</label>
                <asp:TextBox ID="txtFoto" runat="server" CssClass="form-control" placeholder="Link Foto" />
              </div>
        
              <asp:Button ID="btnConfirmar" runat="server" CssClass="btn btn-success w-100" Text="Confirmar"  OnClick="btnConfirmar_Click" />
        
              <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger w-100" Text="Cancelar"  OnClick="btnCancelar_Click" CausesValidation="false"/>
                <asp:Button ID="btnVolver" runat="server" 
    CssClass="btn btn-primary w-100 mt-2" 
    Text="Volver al Panel Admin" 
    OnClick="btnVolver_Click" />
        
            </div>
          </div>
        </section>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </form>

    <script src="Scripts/bootstrap.bundle.js"></script>
    <script>
        (() => {
            'use strict';
            const formulario = document.getElementById('form1');
    
            const usuario = document.getElementById('<%= txtNombre.ClientID %>');
            const nombreEl = document.getElementById('<%= txtLocalidad.ClientID %>');
            const apellidoEl = document.getElementById('<%= txtFoto.ClientID %>');
    
        })();
    </script>
</body>
</html>
