<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/AccueilEtudiant.Master" AutoEventWireup="true" CodeBehind="FormVoirPaiement.aspx.cs" Inherits="UTMH_Edu.Vue.FormVoirPaiement" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <h6 class="font-weight-bolder text-white mb-0">Paiement</h6>
                </nav>
            </div>
        </nav>
    
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- TITRE -->
                <div class="card-header pb-0">
                    <h6>Mes paiements</h6>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- MESSAGE -->
                    <asp:Label ID="lbMsg" runat="server" CssClass="text-danger"></asp:Label>

                    <!-- GRID -->
                    <div class="table-responsive">
                        <asp:GridView
                            ID="gvPaiements"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover text-center align-middle"
                            GridLines="None"
                            EmptyDataText="Aucun paiement enregistré."
                            ShowHeader="true">

                            <Columns>
                                <asp:BoundField DataField="versement" HeaderText="Versement"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="motif" HeaderText="Motif"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="datePaiement" HeaderText="Date"
                                    DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="montantPaye" HeaderText="Montant payé"
                                    DataFormatString="{0:N0}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="balance" HeaderText="Balance"
                                    DataFormatString="{0:N0}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />
                            </Columns>

                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>