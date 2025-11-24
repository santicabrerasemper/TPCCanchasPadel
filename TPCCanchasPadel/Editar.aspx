<%@ Page Title="Editar" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="TPCCanchasPadel.Editar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h2 class="text-center mb-4 fw-bold">Editar Canchas por Sucursal</h2>
    
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div class="d-flex gap-3">
                <div class="col-md-6">
                    <label for="ddlSucursal" class="form-label fw-bold">Seleccionar Sucursal</label>
                    <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
                </div>
        
                <div class="col-md-6">
                    <label for="ddlEstado" class="form-label fw-bold">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                        <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Activos" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Inactivos" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        
            <asp:Button ID="btnNuevaSucursal" runat="server" Text="Agregar Nueva Sucursal" CssClass="btn btn-success" OnClick="btnAgregarSucursal_OnClick" />
        </div>
    
        <asp:GridView ID="gvCanchas" runat="server" CssClass="table table-bordered table-hover text-center" AutoGenerateColumns="False" DataKeyNames="CanchaID">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("CanchaID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
        
                <asp:BoundField DataField="Nombre" HeaderText="Cancha" />
        
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("EstadoID")) == 1 ? "Activo" : "Desactivado" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    
        <div class="text-center mt-4">
            <asp:Button ID="btnCambiarEstado" runat="server" Text="Cambiar Estado" CssClass="btn btn-warning me-2" OnClick="btnCambiarEstado_Click" />
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar Cancha" CssClass="btn btn-info me-2" OnClick="btnAgregar_Click" />
            <a href="ReservasAdmin.aspx" class="btn btn-secondary">Volver</a>
        </div>
        
        <script type="text/javascript">
            function agregarSucursal() {
                var nombre = prompt("Ingrese el nombre de la nueva sucursal:");
                if (nombre == null || nombre.trim() === "") {
                    alert("Debe ingresar un nombre válido.");
                    return;
                }
    
                var localidad = prompt("Ingrese la localidad de la nueva sucursal:");
                if (localidad == null || localidad.trim() === "") {
                    alert("Debe ingresar una localidad válida.");
                    return;
                }
    
                __doPostBack('AgregarSucursal', nombre + '|' + localidad);
            }
        </script>
        
        <asp:HiddenField ID="hiddenSucursalNombre" runat="server" />
    </div>
</asp:Content>


