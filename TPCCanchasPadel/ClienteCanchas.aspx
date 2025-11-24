<%@ Page Title="Canchas Disponibles" Language="C#" MasterPageFile="~/Canchas.Master" AutoEventWireup="true" CodeBehind="ClienteCanchas.aspx.cs" Inherits="TPCCanchasPadel.ClienteCanchas" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <style>
        .canchas-hero {
            background-color: #0b1f17;
            min-height: 40vh;
            display: flex;
            align-items: center;
            color: #fff;
            text-align: center;
        }

        .canchas-hero h1 {
            font-weight: 800;
            letter-spacing: .5px;
            margin-bottom: .5rem;
        }

        .canchas-hero h1 span {
            color: #ffd000;
        }

        .img-sucursal-rounded {
            border-radius: 1rem;
            overflow: hidden;
        }

        .canchas-wrap {
            max-width: 980px;
        }

        .canchas-card {
            background: #fff;
            border: 1px solid rgba(0,0,0,.08);
            border-radius: .5rem;
            box-shadow: 0 6px 24px rgba(0,0,0,.08);
            padding: 1.25rem;
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
                        <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSucursal_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="col-12 mt-3">
                        <div class="row g-3 align-items-center">

                            <div class="col-md-5">
                                <asp:Image ID="imgSucursal" runat="server" Visible="false" Width="350px" CssClass="img-fluid img-sucursal-rounded shadow-sm border" />
                            </div>

                            <div class="col-md-7 d-flex flex-column justify-content-center">
                                <asp:Label ID="lblDescCancha" runat="server" CssClass="d-block text-muted mb-1">
                                </asp:Label>

                                <asp:Label ID="lblUbicacion" runat="server" CssClass="d-block fw-semibold">
                                </asp:Label>
                            </div>
                        </div>
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

                    <div class="col-12 col-md-3">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-success w-100" OnClick="btnBuscar_Click" />
                    </div>

                    <div class="col-12 col-md-3">
                        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary w-100" OnClick="btnVolver_Click" CausesValidation="false" />
                    </div>

                    <div class="col-md-3 d-flex align-items-end">
                        <asp:Button ID="btnNuevaBusqueda" runat="server" Text="Nueva búsqueda" CssClass="btn btn-info w-100" Visible="false" OnClick="btnNuevaBusqueda_Click" />
                    </div>

                    <div class="col-12 col-md-3 d-none"></div>
                </div>

                <asp:Label ID="lblPromo" runat="server" CssClass="alert alert-info py-2 px-3 d-inline-block mb-2" Visible="false" />
                <asp:HiddenField ID="hidPromoId" runat="server" />
                <asp:Label ID="lblMensaje" runat="server" CssClass="fw-semibold d-block mt-3"></asp:Label>
            </div>

            <div class="canchas-card mb-4">
                <h3 class="h5 fw-bold mb-3">Resultados</h3>

                <asp:GridView ID="gvCanchas" runat="server" CssClass="table table-striped table-hover text-center mb-0" AutoGenerateColumns="false" OnRowCommand="grillaCanchas_ComandoReserva" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre de Cancha" />
                        <asp:BoundField DataField="NombreSucursal" HeaderText="Sucursal" />
                        <asp:BoundField DataField="NombreLocalidad" HeaderText="Localidad" />
                        <asp:BoundField DataField="TotalEstimado" HeaderText="Total Estimado" DataFormatString="{0:C}" />

                        <asp:TemplateField HeaderText="Acción">
                            <ItemTemplate>
                                <asp:Button ID="btnReservar" runat="server" Text="Reservar" CommandName="Reservar" CommandArgument='<%# Eval("CanchaID") %>' CssClass="btn btn-success btn-sm" Visible='<%# Convert.ToBoolean(Eval("Activa")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div class="canchas-card mb-4">
                <h2 class="h5 fw-bold mb-3">Mis Reservas</h2>

                <asp:Label ID="lblEstadoReserva" runat="server" CssClass="alert alert-success fw-bold" Visible="false"></asp:Label>

                <asp:Button ID="btnMisReservas" runat="server" Text="Ver mis reservas" CssClass="btn btn-outline-primary w-100" OnClick="btnMisReservas_Click" CausesValidation="false" UseSubmitBehavior="false" />

                <asp:Label ID="lblMisReservasMsg" runat="server" CssClass="text-danger fw-semibold d-block mb-3"></asp:Label>

                <asp:GridView ID="gvMisReservas" runat="server" AutoGenerateColumns="False" CssClass="table table-striped text-center" Visible="false" OnRowDataBound="gvMisReservas_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="FechaReserva" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                        <asp:TemplateField HeaderText="Inicio">
                            <ItemTemplate>
                                <%# ((TimeSpan)Eval("HoraInicio")).ToString(@"hh\:mm") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Fin">
                            <ItemTemplate>
                                <%# ((TimeSpan)Eval("HoraFin")).ToString(@"hh\:mm") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Cancha.Nombre" HeaderText="Cancha" />

                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <%# ((Dominio.Reserva)Container.DataItem).Estado.Nombre %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Sucursal.Nombre" HeaderText="Sucursal" />
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblCantidadReservas" runat="server" CssClass="fw-semibold mt-2 d-block text-end"></asp:Label>
            </div>
        </div>
    </section>
</asp:Content>
