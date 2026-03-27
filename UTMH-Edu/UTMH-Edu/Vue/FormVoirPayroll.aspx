<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/AccueilProfesseur.Master" AutoEventWireup="true" CodeBehind="FormVoirPayroll.aspx.cs" Inherits="UTMH_Edu.Vue.FormVoirPayroll" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <h6 class="font-weight-bolder text-white mb-0">Payroll</h6>
                </nav>
            </div>
        </nav>
    
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- TITRE -->
                <div class="card-header pb-0 d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">Payroll (par mois)</h6>

                    <asp:DropDownList
                        ID="ddlAnnee"
                        runat="server"
                        CssClass="form-select form-select-sm"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlAnnee_SelectedIndexChanged"
                        Style="max-width: 140px;">
                    </asp:DropDownList>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- MESSAGE -->
                    <asp:Label ID="lbMsg" runat="server" CssClass="text-danger"></asp:Label>

                    <!-- TABLE -->
                    <div class="table-responsive">
                        <asp:GridView
                            ID="gvPayroll"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover text-center align-middle"
                            GridLines="None"
                            EmptyDataText="Aucune donnée."
                            OnRowDataBound="gvPayroll_RowDataBound">

                            <Columns>
                                <asp:BoundField DataField="Mois" HeaderText="Mois"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="Annee" HeaderText="Année"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="Montant" HeaderText="Montant"
                                    DataFormatString="{0:N0}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="DatePayroll" HeaderText="Date payroll"
                                    DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:TemplateField HeaderText="Statut"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <span runat="server" id="spStatut" class="badge bg-secondary">
                                            <%# Eval("Statut") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </div>

                    <div class="mt-3">
                        <span class="me-3">
                            <strong>Total payé :</strong>
                            <asp:Label ID="lbTotalPaye" runat="server" Text="0" />
                        </span>

                        <span>
                            <strong>Mois payés :</strong>
                            <asp:Label ID="lbMoisPayes" runat="server" Text="0" />
                        </span>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>