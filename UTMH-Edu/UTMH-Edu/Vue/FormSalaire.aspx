<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormSalaire.aspx.cs" Inherits="UTMH_Edu.Vue.FormSalaire" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Gestion des salaires</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
 <div class="row">
    <div class="col-12">
        <div class="card mb-4 ms-3">
            <div class="card-header pb-0">
                <h6>Gestion des salaires</h6>
            </div>

            <div class="card-body px-3 pt-3 pb-2">
                <asp:Label ID="Lb2" runat="server" CssClass="fw-bold text-danger d-block mb-3"></asp:Label>

                <div class="row mb-3">
                    <div class="col-md-4">
                        <label class="fw-bold">Type de personne</label>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                            <asp:ListItem Value="0">-- Sélectionner --</asp:ListItem>
                            <asp:ListItem Value="PROF">Professeur</asp:ListItem>
                            <asp:ListItem Value="ADM">Administration</asp:ListItem>
                            
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2 d-flex align-items-end">
                        <asp:LinkButton ID="btnReinitialiser" runat="server"
                            CssClass="btn btn-secondary w-100"
                            OnClick="btnReinitialiser_Click">
                            Réinitialiser
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="GridView1" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover"
                        DataKeyNames="typePersonne,idPersonne"
                        OnRowCommand="GridView1_RowCommand">

                        <Columns>
                            <asp:BoundField DataField="code" HeaderText="Code" />
                            <asp:BoundField DataField="nom" HeaderText="Nom" />
                            <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                            <asp:BoundField DataField="role" HeaderText="Rôle" />
                            <asp:BoundField DataField="montantMensuel" HeaderText="Salaire" DataFormatString="{0:N0}" />
                            <asp:BoundField DataField="actif" HeaderText="Actif" />

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server"
                                        CssClass="btn btn-sm btn-warning"
                                        CommandName="EditSalaire"
                                        CommandArgument="<%# Container.DataItemIndex %>">
                                        Modifier
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <hr class="horizontal dark" />

                <h6>Ajouter / Modifier un salaire</h6>

                <div class="row g-3">
                    <div class="col-md-4">
                        <label class="fw-bold">Salaire mensuel</label>
                        <asp:TextBox ID="txtSalaire" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    </div>

                   
                    <div class="col-md-4 d-flex align-items-end">
                        <asp:Button ID="btnEnregistrer" runat="server"
                            Text="Enregistrer"
                            CssClass="btn btn-primary w-100"
                            OnClick="btnEnregistrer_Click" />
                    </div>

                     <div class="col-md-2">
                   <%--     <label class="fw-bold">Actif</label>--%>
                        <asp:DropDownList ID="ddlActif" runat="server" CssClass="form-select" Visible="false">
                            <asp:ListItem Value="1">Oui</asp:ListItem>
                            <asp:ListItem Value="0">Non</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

            </div>
        </div>
    </div>
</div>

</asp:Content>