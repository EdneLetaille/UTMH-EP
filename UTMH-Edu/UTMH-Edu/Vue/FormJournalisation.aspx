<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master"
    AutoEventWireup="true" CodeBehind="FormJournalisation.aspx.cs"
    Inherits="UTMH_Edu.Vue.FormJournalisation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
          <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Journalisation</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
        <div class="row">
            <div class="col-12">
                <div class="card mb-4 ms-3">

                    <div class="card-header pb-3">
                        <div class="row align-items-center">

                            <!-- TITRE -->
                            <div class="col-md-6">
                                <h5 class="mb-1 fw-bold">
                                    <i class="fa-solid fa-clipboard-list me-2 text-primary"></i>
                                    Journalisation du système
                                </h5>
                                <small class="text-muted">Historique des connexions et actions des utilisateurs
                                </small>
                            </div>

                            <!-- FILTRES -->
                            <div class="col-md-6">
                                <div class="row g-2 justify-content-end">

                                    <div class="col-auto">
                                        <asp:TextBox
                                            ID="txtDate"
                                            runat="server"
                                            CssClass="form-control form-control-sm"
                                            TextMode="Date"
                                            AutoPostBack="True"
                                            OnTextChanged="FiltrerLogs" />
                                    </div>

                                    <div class="col-auto">
                                        <asp:TextBox
                                            ID="txtCode"
                                            runat="server"
                                            CssClass="form-control form-control-sm"
                                            Placeholder="Code utilisateur"
                                            AutoPostBack="True"
                                            OnTextChanged="FiltrerLogs" />
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

                                    <div class="col-auto">
                                        <asp:LinkButton
                                            ID="btnExport"
                                            runat="server"
                                            CssClass="btn btn-success btn-sm"
                                            OnClick="btnExport_Click">
                        <i class="fa fa-file-excel me-1"></i> Export
                                        </asp:LinkButton>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>



                    <!-- TABLE -->
                    <div class="table-responsive p-0">
                        <asp:GridView
                            ID="gvLogs"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table align-items-center justify-content-center mb-0"
                            HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                            RowStyle-CssClass="text-sm"
                            DataKeyNames="idLog"
                            AllowPaging="true"
                            PageSize="10"
                            OnPageIndexChanging="gvLogs_PageIndexChanging">

                            <PagerStyle CssClass="pagination justify-content-center mt-3" />

                            <Columns>
                                <asp:BoundField DataField="code" HeaderText="Code" />
                                <asp:BoundField DataField="page" HeaderText="Page" />
                                <asp:BoundField DataField="action" HeaderText="Action" />
                                <asp:BoundField DataField="role" HeaderText="Rôle" />
                                <asp:BoundField DataField="dateConnexion" HeaderText="Date Connexion" />
                                <asp:BoundField DataField="dateDeconnexion" HeaderText="Date Déconnexion" />
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
        </div>
    </main>
</asp:Content>
