<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/AccueilEtudiant.Master" AutoEventWireup="true" CodeBehind="FormBulletinEtudiant.aspx.cs" Inherits="UTMH_Edu.Vue.FormBulletinEtudiant" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
     <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Bulletin</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- TITRE -->
                <div class="card-header pb-0 d-flex justify-content-between align-items-center">
                    <h6 class="mb-0">Bulletin</h6>

                    <!-- ✅ Impression PDF -->
                    <asp:LinkButton
                        ID="btnPrintAll"
                        runat="server"
                        CssClass="btn btn-sm btn-danger"
                        CausesValidation="false"
                        UseSubmitBehavior="false"
                        OnClick="btnPrintAll_Click">
                        <i class="fa fa-print"></i> Imprimer
                    </asp:LinkButton>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- INFOS ETUDIANT -->
                    <div class="row mb-3">
                        <div class="col-md-4">
                            <small class="text-muted">Étudiant</small><br />
                            <strong><asp:Label ID="lbNomEtudiant" runat="server" Text="-" /></strong>
                        </div>
                        <div class="col-md-4">
                            <small class="text-muted">Code</small><br />
                            <strong><asp:Label ID="lbCodeEtudiant" runat="server" Text="-" /></strong>
                        </div>
                        <div class="col-md-4">
                            <small class="text-muted">Option</small><br />
                            <strong><asp:Label ID="lbOption" runat="server" Text="-" /></strong>
                        </div>
                    </div>

                    <!-- MESSAGE -->
                    <asp:Label ID="lbMsg" runat="server" CssClass="text-danger"></asp:Label>

                    <!-- TABLE -->
                    <div class="table-responsive">
                        <asp:GridView
                            ID="gvBulletin"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover text-center align-middle"
                            GridLines="None"
                            ShowFooter="true"
                            OnRowDataBound="gvBulletin_RowDataBound"
                            EmptyDataText="Aucune note d’examen enregistrée.">

                            <Columns>
                                <asp:BoundField DataField="Cours" HeaderText="NOM DU COURS"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="NoteObtenue" HeaderText="NOTE OBTENUE"
                                    DataFormatString="{0:0.00}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />

                                <asp:BoundField DataField="Coefficient" HeaderText="COEFFICIENT"
                                    DataFormatString="{0:0.##}"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center"
                                    ItemStyle-CssClass="text-center" />
                            </Columns>

                            <FooterStyle CssClass="fw-bold" />
                        </asp:GridView>
                    </div>

                    <div class="mt-3">
                        <span class="fw-bold">Moyenne :</span>
                        <span class="badge bg-primary">
                            <asp:Label ID="lbMoyenne" runat="server" Text="0.00" />
                        </span>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>