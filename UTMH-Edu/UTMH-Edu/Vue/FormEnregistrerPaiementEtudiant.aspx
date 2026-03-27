<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormEnregistrerPaiementEtudiant.aspx.cs" Inherits="UTMH_Edu.Vue.FormEnregistrerPaiementEtudiant" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Paiement Etudiant</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
     <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <div class="card-header">
                    <h6>Liste Paiement</h6>
                </div>

                <div class="card-body">

                    <!-- FILTRES -->
                    <div class="row mb-3">

                        <div class="col-md-2">
                            <asp:DropDownList
                                ID="ddlAnneAcademique"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="Filtre_Changed" />
                        </div>

                        <div class="col-md-2">
                            <asp:DropDownList
                                ID="ddlOption"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="Filtre_Changed" />
                        </div>

                        <div class="col-md-3">
                            <asp:TextBox
                                ID="txtRecherche"
                                runat="server"
                                CssClass="form-control"
                                Placeholder="Nom, prénom ou code" />
                        </div>

                        <div class="col-md-auto">
                            <asp:LinkButton
                                ID="btnRecherche"
                                runat="server"
                                CssClass="btn btn-primary"
                                OnClick="btnRecherche_Click">
                                🔍 Rechercher
                            </asp:LinkButton>

                            <asp:LinkButton
                                ID="btnReinitialiser"
                                runat="server"
                                CssClass="btn btn-secondary"
                                OnClick="btnReinitialiser_Click">
                                ♻ Réinitialiser
                            </asp:LinkButton>
                        </div>

                    </div>

                    <asp:GridView
                        ID="GridView1"
                        runat="server"
                        CssClass="table table-bordered table-hover"
                        AutoGenerateColumns="False"
                        DataKeyNames="code">

                        <Columns>
                            <asp:BoundField DataField="code" HeaderText="Code" />
                            <asp:BoundField DataField="nom" HeaderText="Nom" />
                            <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                            <asp:BoundField DataField="NomOption" HeaderText="Option" />
                            <asp:BoundField DataField="montantAPayer" HeaderText="Montant à payer" />

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <a class="btn btn-sm btn-primary"
                                        href='<%# "FormPaiement.aspx?code=" + Eval("code") %>'>Enregistrer Paiement
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>







                </div>
            </div>
        </div>
    </div>

</asp:Content>