<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormListeEtudiant.aspx.cs" Inherits="UTMH_Edu.Vue.FormListeEtudiant" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
          <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Liste des etudiants</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
        <div class="row">
            <div class="col-12">
                <div class="card mb-4 ms-3">
                    <div class="card-header pb-0">
                        <h6>Liste des étudiants</h6>
                    </div>
                    <div class="card-body px-0 pt-0 pb-2">

                        <!-- ZONE DE RECHERCHE -->
                        <!-- ZONE DE RECHERCHE + FILTRE STATUT -->
                        <div class="row mb-3 align-items-center">
                            <!-- Champ recherche -->
                            <div class="col-12 col-md-3 mb-2 mb-md-0">
                                <asp:TextBox
                                    ID="txtRecherche"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    OnTextChanged="btnRecherche_Click"
                                    Placeholder="Rechercher par nom, prénom ou code">
                                </asp:TextBox>


                            </div>

                            <!-- Filtre statut -->
                            <div class="col-12 col-md-2 mb-2 mb-md-0">
                                <asp:DropDownList
                                    ID="ddlStatut"
                                    AutoPostBack="true"
                                    runat="server"
                                    CssClass="form-select">
                                    <asp:ListItem Text="Tous" Value="" Selected="True" />
                                    <asp:ListItem Text="En attente" Value="En attente" />
                                    <asp:ListItem Text="Actif" Value="Actif" />
                                    <asp:ListItem Text="Inactif" Value="Inactif" />
                                </asp:DropDownList>
                            </div>

                            <!-- Boutons -->
                            <div class="col-12 col-md-auto d-flex gap-4">
                                <asp:LinkButton
                                    ID="btnRecherche"
                                    runat="server"
                                    CssClass="btn btn-primary flex-fill"
                                    OnClick="btnRecherche_Click">
            <i class="fa fa-search"></i>
            <span class="d-none d-md-inline"> Rechercher</span>
                                </asp:LinkButton>

                                <asp:LinkButton
                                    ID="btnReinitialiser"
                                    runat="server"
                                    CssClass="btn btn-secondary flex-fill"
                                    OnClick="btnReinitialiser_Click">
            <i class="fa fa-rotate-left"></i>
            <span class="d-none d-md-inline"> Réinitialiser</span>
                                </asp:LinkButton>
                                <!-- Bouton Ajouter Étudiant -->
                                <asp:LinkButton ID="btnAjouterEtudiant" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnAjouterEtudiant_Click">
        <i class="fa-solid fa-user-plus me-1"></i> 
        <span class="d-none d-md-inline"> Ajouter Étudiant </span>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="table-responsive p-0">
                            <asp:Label ID="Lb2" runat="server"></asp:Label>
                            <asp:GridView ID="GridView1"
                                runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-bordered table-hover"
                                DataKeyNames="code"
                                OnRowDataBound="GridView1_RowDataBound">

                                <Columns>
                                    <asp:BoundField DataField="code" HeaderText="Code" />
                                    <asp:BoundField DataField="nom" HeaderText="Nom" />
                                    <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                                    <asp:BoundField DataField="sexe" HeaderText="Sexe" />
                                    <asp:BoundField DataField="adresse" HeaderText="Adresse" />
                                    <asp:BoundField DataField="telephone" HeaderText="Téléphone" />
                                    <asp:BoundField DataField="cin" HeaderText="cin " />

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <a class="btn btn-sm btn-primary"
                                                href='<%# "FormDossierEtudiant.aspx?code=" + Eval("code") %>'>Voir Plus
                                            </a>
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
