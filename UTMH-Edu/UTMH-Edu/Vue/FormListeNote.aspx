<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master"
    AutoEventWireup="true"
    CodeBehind="FormListeNote.aspx.cs"
    Inherits="UTMH_Edu.Vue.FormListeNote" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>

          <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Liste des notes</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- EN-TÊTE -->
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">Liste des notes</h6>

                    <asp:LinkButton
                        ID="btnAjouterNote"
                        runat="server"
                        Visible="false"
                        CssClass="btn btn-sm btn-primary"
                        OnClick="btnAjouterNote_Click">
                        <i class="fa fa-plus"></i> Ajouter
                    </asp:LinkButton>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- FILTRES -->
                    <div class="row mb-3 align-items-end g-2">

                        <div class="col-md-2">
                            <label class="form-label small fw-bold">Année académique</label>
                            <asp:DropDownList
                                ID="ddlAnneAcademique"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAnneAcademique_SelectedIndexChanged" />
                        </div>

                        <div class="col-md-2">
                            <label class="form-label small fw-bold">Option</label>
                            <asp:DropDownList
                                ID="ddlOption"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlOption_SelectedIndexChanged" />
                        </div>

                        <div class="col-md-3">
                            <label class="form-label small fw-bold">Cours</label>
                            <asp:DropDownList
                                ID="ddlCours"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCours_SelectedIndexChanged" />
                        </div>

                        <div class="col-md-3">
                            <label class="form-label small fw-bold">Recherche étudiant</label>
                            <asp:TextBox
                                ID="txtRecherche"
                                runat="server"
                                CssClass="form-control"
                                Placeholder="Nom ou prénom"
                                AutoPostBack="true"
                                OnTextChanged="txtRecherche_TextChanged" />
                        </div>

                        <div class="col-md-2">
                            <asp:LinkButton
                                ID="btnReinitialiser"
                                runat="server"
                                CssClass="btn btn-secondary w-100"
                                OnClick="btnReinitialiser_Click">
                                <i class="fa fa-rotate-left"></i> Réinitialiser
                            </asp:LinkButton>
                        </div>

                    </div>

                    <!-- MESSAGE -->
                    <asp:Label
                        ID="Lb2"
                        runat="server"
                        CssClass="text-danger fw-bold mb-2 d-block">
                    </asp:Label>

                    <!-- TABLE -->
                    <div class="table-responsive">

                        <asp:GridView
                            ID="GridView1"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover align-middle"
                            DataKeyNames="idNote"
                            OnRowEditing="GridView1_RowEditing"
                            OnRowCancelingEdit="GridView1_RowCancelingEdit"
                            OnRowUpdating="GridView1_RowUpdating"
                            OnRowDeleting="GridView1_RowDeleting"
                            OnRowDataBound="GridView1_RowDataBound">

                            <Columns>

                                <asp:BoundField DataField="nom" HeaderText="Nom" ReadOnly="true" />
                                <asp:BoundField DataField="prenom" HeaderText="Prénom" ReadOnly="true" />
                                <asp:BoundField DataField="nomOption" HeaderText="Option" ReadOnly="true" />
                                <asp:BoundField DataField="nomCours" HeaderText="Cours" ReadOnly="true" />

                                <asp:TemplateField HeaderText="Libellé">
                                    <ItemTemplate>
                                        <%# Eval("libelleNote") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox
                                            ID="txtLibelle"
                                            runat="server"
                                            Text='<%# Bind("libelleNote") %>'
                                            CssClass="form-control" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Note">
                                    <ItemTemplate>
                                        <%# Eval("noteObtenue") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox
                                            ID="txtNote"
                                            runat="server"
                                            Text='<%# Bind("noteObtenue") %>'
                                            CssClass="form-control" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <%# Eval("typeNote") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList
                                            ID="ddlType"
                                            runat="server"
                                            CssClass="form-select">
                                            <asp:ListItem>Examen</asp:ListItem>
                                            <asp:ListItem>Devoir</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="btnEdit"
                                            runat="server"
                                            CommandName="Edit"
                                            CssClass="btn btn-sm btn-warning me-1"
                                            ToolTip="Modifier">
                                            <i class="fa fa-pen"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton
                                            ID="btnDelete"
                                            runat="server"
                                            CommandName="Delete"
                                            CssClass="btn btn-sm btn-danger"
                                            ToolTip="Supprimer"
                                            OnClientClick="return confirm('Voulez-vous vraiment supprimer cette note ?');">
                                            <i class="fa fa-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:LinkButton
                                            ID="btnUpdate"
                                            runat="server"
                                            CommandName="Update"
                                            CssClass="btn btn-sm btn-success me-1"
                                            ToolTip="Enregistrer">
                                            <i class="fa fa-save"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton
                                            ID="btnCancel"
                                            runat="server"
                                            CommandName="Cancel"
                                            CssClass="btn btn-sm btn-secondary"
                                            ToolTip="Annuler">
                                            <i class="fa fa-times"></i>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
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
