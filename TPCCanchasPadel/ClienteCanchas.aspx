<%@ Page Title="Canchas Disponibles" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="ClienteCanchas.aspx.cs" Inherits="TPCCanchasPadel.ClienteCanchas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-5">
        <h2 class="text-center mb-4 fw-bold">Consultar Canchas Disponibles</h2>

        <div class="row g-3 justify-content-center mb-4">
            <div class="col-md-3">
    <label for="ddlSucursal" class="form-label">Sucursal</label>
    <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-select"></asp:DropDownList>
</div>


            <div class="col-md-3">

                <label for="txtFecha" class="form-label">Fecha</label>
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label for="txtHoraInicio" class="form-label">Hora inicio</label>
                <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label for="txtHoraFin" class="form-label">Hora fin</label>
                <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
            </div>
            <div class="col-md-3 d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-success w-100" OnClick="btnBuscar_Click"/>
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-info me-2" PostBackUrl="Editar.aspx" Visible="false"/>
            </div>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="text-center fw-semibold d-block mb-3"></asp:Label>

        <asp:GridView ID="gvCanchas" runat="server" CssClass="table table-striped table-hover text-center"
            AutoGenerateColumns="false" OnRowCommand="gvCanchas_RowCommand" Visible="false">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre de Cancha" />
            
                <asp:BoundField DataField="NombreSucursal" HeaderText="Sucursal" />
            
                <asp:BoundField DataField="NombreLocalidad" HeaderText="Localidad" />
            
                <asp:BoundField DataField="TotalEstimado" HeaderText="Total Estimado" DataFormatString="{0:C}" />
            
                <asp:TemplateField HeaderText="Acción">
                    <ItemTemplate>
                        <asp:Button ID="btnReservar" runat="server" Text="Reservar"
                            CommandName="Reservar"
                            CommandArgument='<%# Eval("CanchaID") %>'
                            CssClass="btn btn-success btn-sm"
                            Visible='<%# Convert.ToBoolean(Eval("Activa")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>