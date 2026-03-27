<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master"
    AutoEventWireup="true"
    CodeBehind="FormListeAdministrateur.aspx.cs"
    Inherits="UTMH_Edu.Vue.FormListeAdministrateur" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
          <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Liste des membres</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
        <div class="row">
            <div class="col-12">

                <div class="card mb-4 ms-3">

                    <!-- TITRE -->
                    <div class="card-header pb-0">
                        <h6>Liste des administrateurs</h6>
                    </div>

                    <div class="card-body px-3 pt-3 pb-2">

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
                                    <asp:ListItem Text="Actif" Value="Actif" />
                                    <asp:ListItem Text="Inactif" Value="Inactif" />
                                </asp:DropDownList>
                            </div>

                            <!-- Boutons -->
                            <div class="col-12 col-md-auto d-flex gap-2">
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
                            </div>
                        </div>



                        <!-- MESSAGE -->
                        <asp:Label ID="Lb2" runat="server" CssClass="text-danger"></asp:Label>

                        <!-- TABLE -->
                        <div class="table-responsive p-0">

                            <asp:GridView
                                ID="GridView1"
                                runat="server"
                                AutoGenerateColumns="False"
                                   DataKeyNames="code"
                                OnRowDataBound="GridView1_RowDataBound"
                                CssClass="table align-items-center justify-content-center mb-0"
                                HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                                RowStyle-CssClass="text-sm">

                                <Columns>
                                    <asp:BoundField DataField="code" HeaderText="Code" />
                                    <asp:BoundField DataField="nom" HeaderText="Nom" />
                                    <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                                    <asp:BoundField DataField="email" HeaderText="Email" />
                                    <asp:BoundField DataField="telephone" HeaderText="Téléphone" />

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <a class="btn btn-sm btn-primary"
                                                href='<%# "FormDossierAdministrateur.aspx?code=" + Eval("code") %>'>Voir Plus
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
