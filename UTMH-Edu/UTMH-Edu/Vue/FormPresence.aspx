<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master"
    AutoEventWireup="true"
    CodeBehind="FormPresence.aspx.cs"
    Inherits="UTMH_Edu.Vue.FormPresence" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- TITRE -->
                <div class="card-header pb-0">
                    <h6>Présences</h6>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- FILTRES -->
                    <div class="row mb-3 align-items-center">



                        <!-- Année académique -->
                        <div class="col-md-2">
                            <asp:DropDownList
                                ID="ddlAnneAcademique"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAnneAcademique_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <!-- COURS -->
                        <div class="col-md-3">
                            <asp:DropDownList
                                ID="ddlCours"
                                runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCours_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <!-- OPTION -->
                        <div class="col-md-2">
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
                    <asp:TextBox
                        ID="txtDatePresence"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Date" Visible="false" />

                    <div class="table-responsive">

                        <asp:GridView
                            ID="GridView1"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover"
                            EmptyDataText="Aucune présence trouvée"
                            ShowHeader="true"
                            DataKeyNames="idEtudiant,idCours,idProf"
                            OnRowCommand="GridView1_RowCommand">



                            <Columns>


                                <asp:BoundField DataField="NomCours" HeaderText="Cours" />
                                <asp:BoundField DataField="NomProfesseur" HeaderText="Professeur" />
                                <asp:BoundField DataField="NomEtudiant" HeaderText="Étudiant" />

                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <%# 
            Eval("datePresence") == DBNull.Value 
            ? "" 
            : Convert.ToDateTime(Eval("datePresence")).ToString("MMM dd yyyy") 
                                        %>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:BoundField
                                    DataField="heurePresence"
                                    HeaderText="Heure" />

                                <asp:TemplateField HeaderText="Statut">
                                    <ItemTemplate>
                                        <asp:DropDownList
                                            ID="ddlStatut"
                                            runat="server"
                                            CssClass="form-select form-select-sm"
                                            SelectedValue='<%# Bind("status") %>'>
                                            <asp:ListItem>Présent</asp:ListItem>
                                            <asp:ListItem>Absent</asp:ListItem>
                                            <asp:ListItem>Retard</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button
                                            ID="btnValider"
                                            runat="server"
                                            Text="Valider"
                                            CssClass="btn btn-success btn-sm"
                                            CommandName="ValiderPresence"
                                            CommandArgument="<%# Container.DataItemIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="✔">
                                    <ItemTemplate>
                                        <asp:Label
                                            runat="server"
                                            Text="✔"
                                            ToolTip="Présence déjà validée"
                                            CssClass="text-success fw-bold"
                                            Visible='<%# Eval("DejaValide").ToString() == "1" %>' />
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
