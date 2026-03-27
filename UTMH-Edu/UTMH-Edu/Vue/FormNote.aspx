<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Vue/Accueil.Master"
    AutoEventWireup="true"
    CodeBehind="FormNote.aspx.cs"
    Inherits="UTMH_Edu.Vue.FormNote1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
         <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Notes</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
        <div class="row">
            <div class="col-12">
                <div class="card mb-4 ms-3">

                    <!-- ===================== -->
                    <!-- ENTÊTE -->
                    <!-- ===================== -->
                    <div class="card-header pb-3">
                        <div class="row align-items-center">

                            <!-- TITRE -->
                            <div class="col-md-6">
                                <h5 class="mb-1 fw-bold">
                                    <i class="fa-solid fa-pen-to-square me-2 text-primary"></i>
                                    Gestion des notes
                                </h5>
                                <small class="text-muted">
                                    Saisie et consultation des notes par cours et option
                                </small>
                            </div>

                            <!-- FILTRES -->
                            <div class="col-md-6">
                                <div class="row g-2 justify-content-end">

                                   
                                    <div class="col-auto">
                                        <asp:DropDownList
                                            ID="ddlAnneAcademique"
                                            runat="server"
                                            CssClass="form-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAnneAcademique_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                          
                                    <div class="col-auto">
                                        <asp:DropDownList
                                            ID="ddlOption"
                                            runat="server"
                                            CssClass="form-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-auto">
                                        <asp:DropDownList
                                            ID="ddlCours"
                                            runat="server"
                                            CssClass="form-select"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCours_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                       
                                    <div class="col-auto">
                                        <asp:LinkButton
                                            ID="btnReinitialiser"
                                            runat="server"
                                            CssClass="btn btn-outline-secondary btn-sm"
                                            OnClick="btnReinitialiser_Click">
                                            <i class="fa fa-rotate-left"></i>
                                        </asp:LinkButton>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <!-- ===================== -->
                    <!-- CONTENU -->
                    <!-- ===================== -->
                    <div class="card-body px-3 pt-3 pb-2">

                        <!-- MESSAGE -->
                        <asp:Label ID="Lb2" runat="server" />

                        <!-- DATE (cachée si nécessaire) -->
                        <asp:TextBox
                            ID="txtDatePresence"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Date"
                            Visible="false" />

                        <!-- TABLE -->
                        <div class="table-responsive mt-3">

                            <asp:GridView
                                ID="GridView1"
                                runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-bordered table-hover align-middle"
                                DataKeyNames="idEtudiant,idCours"
                                OnRowCommand="GridView1_RowCommand">

                                <HeaderStyle CssClass="table-light text-uppercase text-xs fw-bold" />
                                <RowStyle CssClass="text-sm" />

                                <Columns>

                        
                                    <asp:BoundField
                                        DataField="NomCours"
                                        HeaderText="Cours" />

                                    <asp:BoundField
                                        DataField="NomEtudiant"
                                        HeaderText="Étudiant" />

                                 
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <%# DateTime.Now.ToString("dd/MM/yyyy") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Note">
                                        <ItemTemplate>
                                            <asp:TextBox
                                                ID="txtNote"
                                                runat="server"
                                                CssClass="form-control form-control-sm text-center"
                                                Width="80px"
                                                TextMode="Number"
                                                Min="0"
                                                Max="100" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:DropDownList
                                                ID="ddlTypeNote"
                                                runat="server"
                                                CssClass="form-select form-select-sm">
                                                <asp:ListItem Value="">-- Type --</asp:ListItem>
                                                <asp:ListItem Value="Examen">Examen</asp:ListItem>
                                                <asp:ListItem Value="Devoir">Devoir</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    
                                    <asp:TemplateField HeaderText="Libellé">
                                        <ItemTemplate>
                                            <asp:TextBox
                                                ID="txtLibelleNote"
                                                runat="server"
                                                CssClass="form-control form-control-sm"
                                                Placeholder="Ex : Devoir 1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                   
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button
                                                ID="btnEnregistrer"
                                                runat="server"
                                                Text="Enregistrer"
                                                CssClass="btn btn-success btn-sm"
                                                CommandName="EnregistrerNote"
                                                CommandArgument="<%# Container.DataItemIndex %>" />
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
