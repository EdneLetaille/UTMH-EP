<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormPaiement.aspx.cs" Inherits="UTMH_Edu.Vue.FormPaiement" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main>

        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <h6 class="font-weight-bolder text-white mb-0">Etudiants</h6>
                </nav>
            </div>
        </nav>
        <!-- End Navbar -->

        <div class="container-fluid py-4">
            <div class="row">
                <div class="col-md-8">

                    <div class="card">
                        <div class="card-body">
                            <p class="text-uppercase text-sm">Enregistrer un Paiement</p>

                            <!-- ====== IDENTITE ETUDIANT ====== -->
                            <div class="row">
                               

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Prenom</label>
                                        <asp:TextBox ID="txtPrenom" CssClass="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Nom</label>
                                        <asp:TextBox ID="txtNom" CssClass="form-control" runat="server" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>                              
                            </div>

                            <!-- ====== INFOS PAIEMENT ====== -->
                            <div class="row">
                              


                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Motif</label>
                                        <asp:DropDownList ID="ddlMotif" runat="server" CssClass="form-control" Width="100%"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlMotif_SelectedIndexChanged">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Value="ANNEE_ACADEMIQUE">Année Académique</asp:ListItem>
                                            <asp:ListItem Value="GRADUATION">Graduation</asp:ListItem>
                                            <asp:ListItem Value="PRATIQUEE">Pratiquee</asp:ListItem>
                                            <asp:ListItem Value="EXAMEN">Examen</asp:ListItem>
                                            <asp:ListItem Value="ATTESTATION">Attestation</asp:ListItem>
                                            <asp:ListItem Value="CERTIFICAT">Certificat</asp:ListItem>
                                            <asp:ListItem Value="STAGE">Stage</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Montant à Payer</label>
                                        <asp:TextBox ID="txtMontantAPayer" CssClass="form-control" runat="server"  TextMode="Number" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <!-- ====== MONTANTS ====== -->
                            <div class="row">
                               

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Montant payé</label>
                                        <asp:TextBox ID="txtMontantDu" CssClass="form-control" runat="server" TextMode="Number"
                                            AutoPostBack="True" OnTextChanged="txtMontantDu_TextChanged"  Min="1"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Balance</label>
                                        <asp:TextBox ID="txtBalance" CssClass="form-control" runat="server" TextMode="Number" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                                
                                    <div class="form-group">
                                       <%-- <label class="form-control-label">Versement</label>--%>
                                        <asp:DropDownList ID="ddlVersement" runat="server" CssClass="form-control" Width="100%" Visible="false">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Value="1">Versement 1</asp:ListItem>
                                            <asp:ListItem Value="2">Versement 2</asp:ListItem>
                                            <asp:ListItem Value="3">Versement 3</asp:ListItem>
                                            <asp:ListItem Value="4">Versement 4</asp:ListItem>
                                            <asp:ListItem Value="5">Versement 5</asp:ListItem>
                                        </asp:DropDownList>
                                  
                                </div>

                                
                               
                                    <div class="form-group">
                                       <%-- <label class="form-control-label">Année Académique</label>--%>
                                        <asp:DropDownList ID="ddlAnneeAcademique" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                                    </div>
                              
                                    <div class="form-group">
                                       <%-- <label class="form-control-label">Date Paiement</label>--%>
                                        <asp:TextBox ID="txtDatePaiement" CssClass="form-control" runat="server" TextMode="Date" visible="false"></asp:TextBox>
                                    </div>
                              
                            </div>

                        </div><!-- card-body -->
                    </div><!-- card -->

                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:TextBox ID="txtRole" CssClass="form-control" runat="server" Visible="False"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <hr class="horizontal dark" />

                    <div class="text-center">
                        <asp:Button ID="btnEnregistrer" CssClass="btn btn-primary btn-sm" runat="server"
                            Text="Enregistrer Paiement" OnClick="btnEnregistrer_Click" />

                        <asp:Button ID="btnAnnuler" CssClass="btn btn-secondary btn-sm me-2" runat="server"
                            Text="Annuler" OnClick="btnAnnuler_Click" />
                    </div>

                </div><!-- col-md-8 -->
            </div><!-- row -->

            <!-- Footer -->
            <footer class="footer pt-3">
                <div class="container-fluid">
                    <div class="row align-items-center justify-content-lg-between">
                        <div class="col-lg-6 mb-lg-0 mb-4">
                            <div class="copyright text-center text-sm text-muted text-lg-start">
                                © <script>document.write(new Date().getFullYear())</script>,
                                Crée par <a href="#" class="font-weight-bold" target="_blank">POWERFUL TECH</a>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <ul class="nav nav-footer justify-content-center justify-content-lg-end">
                                <li class="nav-item">
                                    <a href="#" class="nav-link text-muted" target="_blank">POWERFUL TECH</a>
                                </li>
                                <li class="nav-item">
                                    <a href="#" class="nav-link text-muted" target="_blank">À propos</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </footer>

        </div><!-- container-fluid -->

        <!-- ==================== MODAL MESSAGE ==================== -->
        <div class="modal fade" id="msgModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content d-flex align-items-center p-4 border-0 shadow-lg rounded-3">

                    <div class="me-4">
                        <svg id="msgIcon" xmlns="http://www.w3.org/2000/svg" width="50" height="50" fill="currentColor" class="text-warning" viewBox="0 0 16 16">
                            <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm.93 4.58a.5.5 0 0 1 .07.7L8.5 7.5v3a.5.5 0 0 1-1 0V7.5l-.5-.22a.5.5 0 1 1 .46-.88l.04.03L8 6.25l.03-.02zM8 12a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                        </svg>
                    </div>

                    <div class="flex-grow-1 text-start">
                        <p id="msgText" class="mb-0 text-gray-700 fs-5"></p>
                    </div>

                    <div class="ms-4">
                        <button type="button" class="btn btn-primary" data-bs-dismiss="modal">OK</button>
                    </div>

                </div>
            </div>
        </div>

        <!-- Bootstrap JS Bundle -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    </main>

</asp:Content>