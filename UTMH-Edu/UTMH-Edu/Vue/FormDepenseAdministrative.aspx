<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormDepenseAdministrative.aspx.cs" Inherits="UTMH_Edu.Vue.FormDepenseAdministrative" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Depense administrative</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">
                <div class="card-header pb-0">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6 class="mb-0">Dépenses administratives</h6>
                    </div>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <asp:HiddenField ID="hfIdDepense" runat="server" Value="0" />

                    <div class="row g-2">
                        <div class="col-md-3">
                            <label class="form-label fw-bold">Motif</label>
                            <asp:TextBox ID="txtMotif" runat="server" CssClass="form-control" placeholder="Ex: Achat fournitures" />
                        </div>

                        <div class="col-md-4">
                            <label class="form-label fw-bold">Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Détails (optionnel)" />
                        </div>

                        <div class="col-md-2">
                            <label class="form-label fw-bold">Date</label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="True" />
                        </div>

                        <div class="col-md-1">
                            <label class="form-label fw-bold">Quantité</label>
                            <asp:TextBox ID="txtQuantite" runat="server" CssClass="form-control" Text="1" TextMode="Number" />
                        </div>

                        <div class="col-md-2">
                            <label class="form-label fw-bold">Mode de paiement</label>
                            <asp:DropDownList ID="ddlModePaiement" runat="server" CssClass="form-select">
                                <asp:ListItem Value="">-- Choisir --</asp:ListItem>
                                <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                <asp:ListItem Value="Chèque">Chèque</asp:ListItem>
                                <asp:ListItem Value="Virement">Virement</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <label class="form-label fw-bold">Montant</label>
                            <asp:TextBox ID="txtMontant" runat="server" CssClass="form-control" placeholder="Ex: 2500" TextMode="Number" />
                        </div>
                    </div>

                    <div class="d-flex gap-2 mt-3">
                        <asp:LinkButton ID="btnAjouter" runat="server" CssClass="btn btn-success"
                            OnClick="btnAjouter_Click"
                            OnClientClick="return confirm('Confirmer l’enregistrement ?');">
                            <i class="fa fa-plus"></i> Enregistrer
                        </asp:LinkButton>

                        <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-secondary"
                            OnClick="btnReset_Click">
                            <i class="fa fa-rotate-left"></i> Réinitialiser
                        </asp:LinkButton>
                    </div>

                    <asp:Label ID="Lb2" runat="server" CssClass="text-danger fw-bold d-block mt-2"></asp:Label>

                    <hr />

                    <!-- FILTRE -->
                    <div class="row g-2 align-items-end">
                        <div class="col-md-2">
                            <label class="form-label fw-bold">Du</label>
                            <asp:TextBox ID="txtDu" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        <div class="col-md-2">
                            <label class="form-label fw-bold">Au</label>
                            <asp:TextBox ID="txtAu" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        <div class="col-md-3 d-flex gap-2">
                            <asp:LinkButton ID="btnCharger" runat="server" CssClass="btn btn-outline-primary w-100"
                                OnClick="btnCharger_Click">
                                <i class="fa fa-search"></i> Charger
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnResetFiltre" runat="server" CssClass="btn btn-outline-secondary w-100"
                                OnClick="btnResetFiltre_Click">
                                <i class="fa fa-rotate-left"></i> Reset filtre
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="table-responsive mt-3">
                        <asp:GridView ID="GridView1" runat="server"
                            CssClass="table table-bordered table-hover"
                            AutoGenerateColumns="False"
                            DataKeyNames="idDepense"
                            OnRowCommand="GridView1_RowCommand">

                            <Columns>
                                <asp:BoundField DataField="idDepense" HeaderText="ID" />
                                <asp:BoundField DataField="motif" HeaderText="Motif" />
                                <asp:BoundField DataField="description" HeaderText="Description" />
                                <asp:BoundField DataField="dateDepense" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="quantite" HeaderText="Quantité" />
                                <asp:BoundField DataField="modePaiement" HeaderText="Mode de paiement" />
                                <asp:BoundField DataField="montant" HeaderText="Montant" DataFormatString="{0:N2}" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <div class="d-flex gap-2">
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-primary"
                                                CommandName="EDITER" CommandArgument="<%# Container.DataItemIndex %>">
                                                Modifier
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-sm btn-danger"
                                                CommandName="SUPPRIMER" CommandArgument="<%# Container.DataItemIndex %>"
                                                OnClientClick="return confirm('Supprimer cette dépense ?');">
                                                Supprimer
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>