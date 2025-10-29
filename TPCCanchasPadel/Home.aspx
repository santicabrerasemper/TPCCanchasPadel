<%@ Page Title="" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TPCCanchasPadel.Home" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="text-center py-5">
    <h1 class="mb-3" style="font-weight:800;letter-spacing:.5px;">
      ¿DÓNDE <span style="color:#ffd000;">JUEGO?</span>
    </h1>
    <p class="lead mb-4">La forma simple de encontrar y reservar tu cancha.</p>
    <a href="Contacto.aspx" class="btn btn-success px-3">Contactanos</a>
  </div>

  <div class="row text-center g-4">
    <div class="col-md-4">
      <h5>Buscá sedes</h5>
      <p class="text-muted mb-0">Elegí por localidad o sucursal.</p>
    </div>
    <div class="col-md-4">
      <h5>Horarios claros</h5>
      <p class="text-muted mb-0">Solo turnos disponibles.</p>
    </div>
    <div class="col-md-4">
      <h5>Reservá en 1 paso</h5>
      <p class="text-muted mb-0">Confirmación al instante.</p>
    </div>
  </div>
</asp:Content>
