<%@ Page Title="Canchas Disponibles" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="ClienteCanchas.aspx.cs" Inherits="TPCCanchasPadel.ClienteCanchas" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
  <style>
    .canchas-hero{
      background-color:#0b1f17; 
      min-height:40vh;
      display:flex;
      align-items:center;
      color:#fff;
      text-align:center;
    }
    .canchas-hero h1{ font-weight:800; letter-spacing:.5px; margin-bottom:.5rem; }
    .canchas-hero h1 span{ color:#ffd000; }

    .canchas-wrap{ max-width: 980px; } 
    .canchas-card{
      background:#fff;              
      border:1px solid rgba(0,0,0,.08);
      border-radius:.5rem;
      box-shadow:0 6px 24px rgba(0,0,0,.08);
      padding:1.25rem;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <section class="canchas-hero">
    <div class="container py-4">
      <h1 class="mb-2">Consultar <span>Canchas</span> disponibles</h1>
      <p class="lead mb-0">Filtrá por sucursal, fecha y horario para ver opciones en tiempo real.</p>
    </div>
  </section>

  <section class="py-5">
    <div class="container canchas-wrap">

      <div class="canchas-card mb-4">
        <h2 class="h4 fw-bold mb-3">Buscador</h2>

        <div class="row g-3 align-items-end">
          <div class="col-md-3">
            <label for="ddlSucursal" class="form-label">Sucursal</label>
            <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-select"></asp:DropDownList>
          </div>

          <div class="col-md-3">
            <label for="txtFecha" class="form-label">Fecha</label>
            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" required="required"></asp:TextBox>
          </div>

          <div class="col-md-3">
            <label for="txtHoraInicio" class="form-label">Hora inicio</label>
            <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" TextMode="Time" required="required"></asp:TextBox>
          </div>

          <div class="col-md-3">
            <label for="txtHoraFin" class="form-label">Hora fin</label>
            <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" TextMode="Time" required="required"></asp:TextBox>
          </div>

          <div class="col-12 col-md-3">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-success w-100" OnClick="btnBuscar_Click"/>
          </div>

          <div class="col-12 col-md-3 d-none">
            <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-info w-100" PostBackUrl="Editar.aspx" Visible="false"/>
          </div>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="fw-semibold d-block mt-3"></asp:Label>
      </div>
   
      <div class="canchas-card">
        <h3 class="h5 fw-bold mb-3">Resultados</h3>

        <asp:GridView ID="gvCanchas" runat="server"
          CssClass="table table-striped table-hover text-center mb-0"
          AutoGenerateColumns="false"
          OnRowCommand="gvCanchas_RowCommand"
          Visible="false">
          <Columns>
            <asp:BoundField DataField="CanchaID" HeaderText="ID" Visible="false" />

            <asp:BoundField DataField="Nombre" HeaderText="Cancha" />

            <asp:BoundField DataField="NombreSucursal" HeaderText="Sucursal" />

            <asp:BoundField DataField="TotalEstimado" HeaderText="Total estimado" DataFormatString="{0:C}" />

            <asp:TemplateField HeaderText="Acción">
              <ItemTemplate>
                <asp:Button ID="btnReservar" runat="server" Text="Reservar"
                  CommandName="Reservar"
                  CommandArgument='<%# Eval("CanchaID") %>'
                  CssClass="btn btn-success btn-sm" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>

    </div>
  </section>

</asp:Content>
