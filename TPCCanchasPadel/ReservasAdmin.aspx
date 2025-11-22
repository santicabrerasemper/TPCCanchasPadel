<%@ Page Title="" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="ReservasAdmin.aspx.cs" Inherits="TPCCanchasPadel.ReservasAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 class="text-center mb-4 fw-bold">Reservas Registradas</h2>
    
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div class="d-flex gap-3">
                <div class="col-md-6">
                    <label for="ddlSucursal" class="form-label fw-bold">Seleccionar Sucursal</label>
                    <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-6">
                    <label for="ddlCancha" class="form-label fw-bold">Seleccionar Cancha</label>
                    <asp:DropDownList ID="ddlCancha" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCancha_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-6">
                    <label for="ddlFecha" class="form-label fw-bold">Seleccionar Fecha</label>
                    <asp:DropDownList ID="ddlFecha" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlFecha_SelectedIndexChanged"></asp:DropDownList>
                </div>
                
            </div>
    
        </div>

        <asp:GridView ID="gvCanchas" runat="server" 
    CssClass="table table-bordered table-hover text-center"
    AutoGenerateColumns="False"
    OnRowDataBound="gvCanchas_RowDataBound">


    <Columns>

        <asp:BoundField DataField="ReservaID" HeaderText="ID" />

        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("ReservaID") %>' />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Sucursal" HeaderText="Sucursal" />
        <asp:BoundField DataField="Cancha" HeaderText="Cancha" />
        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
        <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio" />
        <asp:BoundField DataField="HoraFin" HeaderText="Hora Fin" />
        <asp:BoundField DataField="Estado" HeaderText="Estado" />

    </Columns>
</asp:GridView>
       
    
        <div class="text-center mt-4">
            
            <asp:Button ID="btnAgregarReserva" runat="server" Text="Agregar Reserva" CssClass="btn btn-info me-2" OnClick="btnAgregarReserva_Click" />
            
            <asp:Button ID="btnCambiarEstado" runat="server" Text="Eliminar Reserva" CssClass="btn btn-danger me-2" OnClick="btnEliminarReserva_Click" />
            <asp:Button ID="btnConfirmarReserva" 
    runat="server" 
    Text="Confirmar Reserva" 
    CssClass="btn btn-success me-2" 
    OnClick="btnConfirmarReserva_Click" />
            <!--<asp:Button ID="btnEditarReserva" runat="server" Text="Editar Reserva" CssClass="btn btn-info me-2" OnClick="btnEditar_Click" />-->
            <asp:Button ID="btnEditarSucursales" runat="server" Text="Editar Sucursales" CssClass="btn btn-success me-2" OnClick="btnEditarSucursales_Click" />
        </div>
        
        
        <asp:HiddenField ID="hiddenSucursalNombre" runat="server" />
    
    </div>
    
</asp:Content>
