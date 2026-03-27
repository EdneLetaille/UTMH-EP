<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormListeCours.aspx.cs" Inherits="UTMH_Edu.Vue.FormListeCours" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
         <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Liste des cours</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
        <div class="row">
            <div class="col-12">
                <div class="card mb-4 ms-3">
                    <div class="card-header pb-0">
                        <h6>Liste des cours</h6>
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
                                    Placeholder="Rechercher par nom ou code">
                                </asp:TextBox>
                            </div>

                            <!-- Filtre statut -->
                            <div class="col-12 col-md-2 mb-2 mb-md-0">
                                <asp:DropDownList
                                    ID="ddlStatut"
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
                        <div class="table-responsive p-0">
                            <asp:Label ID="Lb2" runat="server"></asp:Label>
                            <asp:GridView ID="GridView1"
                                runat="server"
                                AutoGenerateColumns="False"
                                DataKeyNames="code"
                                OnRowDataBound="GridView1_RowDataBound"
                                CssClass="table align-items-center justify-content-center mb-0"
                                HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 header-bold"
                                RowStyle-CssClass="text-sm">

                                <Columns>

                                    <asp:BoundField DataField="code" HeaderText="code" HeaderStyle-ForeColor="Black" SortExpression="code" />
                                    <asp:BoundField DataField="nom" HeaderText="nom" HeaderStyle-ForeColor="Black" SortExpression="nom" />
                                    <asp:BoundField DataField="heureDebut" HeaderText="heureDebut" HeaderStyle-ForeColor="Black" SortExpression="heureDebut" />
                                    <asp:BoundField DataField="heureFin" HeaderText="heureFin" HeaderStyle-ForeColor="Black" SortExpression="heureFin" />
                                    <asp:BoundField DataField="coefficient" HeaderText="coefficient" HeaderStyle-ForeColor="Black" SortExpression="coefficient" />

                                    <asp:TemplateField HeaderText="Action" HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <a class="btn btn-sm btn-primary"
                                                href='<%# "FormDossierCours.aspx?code=" + Eval("code") %>'>Voir Plus
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <HeaderStyle CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 header-bold"></HeaderStyle>

                                <RowStyle CssClass="text-sm"></RowStyle>

                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </main>
</asp:Content>
