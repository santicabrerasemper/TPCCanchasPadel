<%@ Page Title="Editar" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="TPCCanchasPadel.Editar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="container mt-4">
    <h2 class="text-center mb-4 fw-bold">Editar Canchas por Sucursal</h2>

    <div class="d-flex justify-content-between align-items-center mb-4">
        <div class="col-md-4">
            <label for="ddlSucursal" class="form-label fw-bold">Seleccionar Sucursal</label>
            <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
        </div>

        <a href="NuevaSucursal.aspx" class="btn btn-success">Agregar Nueva Sucursal</a>
    </div>

    <asp:GridView ID="gvCanchas" runat="server" CssClass="table table-bordered table-hover text-center" AutoGenerateColumns="False">
        <Columns>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" />
                    <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("CanchaID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="CanchaID" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Cancha" />
        </Columns>
    </asp:GridView>

    <div class="text-center mt-4">
        <asp:Button ID="btnDelete" runat="server" Text="Eliminar Canchas" CssClass="btn btn-danger me-2" OnClick="btnDelete_Click" />
        <asp:Button ID="btnAgregar" runat="server" Text="Agregar Cancha" CssClass="btn btn-success me-2" OnClick="btnAgregar_Click" />
        <a href="ClienteCanchas.aspx" class="btn btn-secondary">Volver</a>
    </div>
</div>

</asp:Content>


