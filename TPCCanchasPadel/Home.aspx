<%@ Page Title="" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TPCCanchasPadel.Home" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
  <style runat="server">
    .hero-bg{
      background-color:#0b1f17; 
      min-height:70vh;
      display:flex;
      align-items:center;
      text-align:center;
      color:#fff;
    }
    .hero-bg h1{
      font-weight:800;
      letter-spacing:.5px;
      margin-bottom:.75rem;
    }
    .hero-bg h1 span{ color:#ffd000; } 
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="container mt-3" style="max-width: 960px;">
    <asp:PlaceHolder ID="phAlerta" runat="server"></asp:PlaceHolder>
  </div>

  <section class="hero-bg">
    <div class="container py-5">
      <h1 class="mb-3">
        ¿DÓNDE <span>JUEGO?</span>
      </h1>
      <p class="lead mb-4">La forma simple de encontrar y reservar tu cancha.</p>
      <a href="Contacto.aspx" class="btn btn-success px-3">Contactanos</a>
    </div>
  </section>

  <div class="row text-center g-4 py-5 bg-white">
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
