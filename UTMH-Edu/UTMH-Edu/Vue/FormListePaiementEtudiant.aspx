<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormListePaiementEtudiant.aspx.cs" Inherits="UTMH_Edu.Vue.FormListePaiementEtudiant" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
         <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Liste des paiements des étudiants</h6>
                </nav>

            </div>
        </nav>
   <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- TITRE -->
                <div class="card-header pb-0">
                    <h6>Paiements</h6>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- FILTRES -->
                    <div class="row mb-3 align-items-center">

                        <!-- MOTIF -->
                        <div class="col-md-3">
                            <asp:DropDownList
                                ID="ddlMotif"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlMotif_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <!-- OPTION -->
                        <div class="col-md-3">
                            <asp:DropDownList
                                ID="ddlOption"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <asp:LinkButton
                                ID="btnReinitialiser"
                                runat="server"
                                CssClass="btn btn-secondary flex-fill"
                                OnClick="btnReinitialiser_Click">
                                <i class="fa fa-rotate-left"></i>
                                <span class="d-none d-md-inline"> Réinitialiser</span>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <!-- MESSAGE -->
                    <asp:Label ID="Lb2" runat="server"></asp:Label>

                    <!-- TABLE -->
                    <div class="table-responsive">
                        <asp:GridView
                            ID="GridView1"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover"
                            DataKeyNames="idEtudiant"
                            OnRowCommand="GridView1_RowCommand">

                            <Columns>
                                <asp:BoundField DataField="code" HeaderText="Code" />
                                <asp:BoundField DataField="nom" HeaderText="Nom" />
                                <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                                <asp:BoundField DataField="nomOption" HeaderText="Option" />

                                <asp:BoundField DataField="montantPayeTotal" HeaderText="Montant payé" DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="balanceReste" HeaderText="Balance" DataFormatString="{0:N0}" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="btnPrint"
                                            runat="server"
                                            CssClass="btn btn-sm btn-danger"
                                            CommandName="PrintPdf"
                                            CommandArgument="<%# Container.DataItemIndex %>">
                                            <i class="fa fa-print"></i> Enprime
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>
        </main>
</asp:Content>