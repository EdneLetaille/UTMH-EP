<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormDossierProfesseur.aspx.cs" Inherits="UTMH_Edu.Vue.FormDossierProfesseur" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>

        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Professeurs</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->

        <div class="container-fluid py-4">
            <div class="row">
                <div class="col-md-8">
                    <div class="card">

                        <div class="card-body">
                            <p class="text-uppercase text-sm">Informations sur le professeur</p>
                            <!-- icon print -->

                            <div class="text-end mb-2">
                                <asp:LinkButton ID="btnPrint"
                                    runat="server"
                                    CssClass="text-info fs-5"
                                    ToolTip="Imprimer"
                                    OnClick="btnPrint_Click">
            <i class="fa-solid fa-print"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="row">
                              
                                <div class="col-md-6">
                                    <label>Prénom</label>
                                    <asp:TextBox ID="txtPrenom" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                </div>
                                <div class="col-md-6">
                                    <label>Nom</label>
                                    <asp:TextBox ID="txtNom" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Departement</label>
                                        <asp:DropDownList
                                            ID="ddlDepartement"
                                            runat="server"
                                            CssClass="form-control"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlDepartement_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Ville</label>
                                        <asp:DropDownList
                                            ID="ddlVille"
                                            runat="server"
                                            CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="tEmail" class="form-control-label">Email</label>
                                        <asp:TextBox ID="txtEmail" class="form-control" runat="server" TextMode="Email"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label ">Cin</label>
                                        <asp:TextBox ID="txtCin" CssClass="form-control cin" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                               

                                <div class="col-md-6">
                                   <div class="form-group">
                                      <label for="ddlSexe" class="form-control-label">Statut</label>
                                        <asp:DropDownList ID="ddlStatut" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Text="Actif" Value="Actif" />
                                            <asp:ListItem Text="Inactif" Value="Inactif" />                                       

                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Liste des Cours</label>
                                        <asp:DropDownList ID="ddlCours" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                     <%--   <label for="example-text-input" class="form-control-label">Date de l'embauche</label>--%>
                                        <asp:TextBox ID="txtDateEmbauche" class="form-control" runat="server" TextMode="Date" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <!-- <label for="example-text-input" class="form-control-label">Role</label> -->
                                        <asp:TextBox ID="txtRole" class="form-control" runat="server" Visible="False"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-md-6">
                                    <div class="form-group">
                                      <%--  <label for="example-text-input" class="form-control-label">Salaire</label>--%>
                                        <asp:TextBox ID="txtSalaire" class="form-control" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                                  <div class="col-md-6">
                                    <div class="form-group">
                                      <%--  <label for="example-text-input" class="form-control-label">Code</label>--%>
                                        <asp:TextBox ID="txtCode" class="form-control" runat="server" Visible="False" ></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <hr class="horizontal dark" />
                            <div class="text-center">
                                <asp:Button ID="btnModifier" class="btn btn-primary btn-sm" runat="server" Text="Modifier" OnClick="btnModifier_Click" />
                                <asp:Button ID="btnSupprimer"
                                    runat="server"
                                    Text="Supprimer"
                                    CssClass="btn btn-danger"
                                    UseSubmitBehavior="false"
                                    OnClientClick="showDeleteModal(); return false;"
                                    OnClick="btnSupprimer_Click" />

                                <asp:Button ID="btnAnnuler" class="btn btn-secondary btn-sm me-2" runat="server" Text="Annuler" OnClick="btnAnnuler_Click" />
                                <asp:Button ID="btnResetPassword"
                                    runat="server"
                                    CssClass="btn btn-warning"
                                    Text="Envoie mot de passe"
                                    OnClick="btnResetPassword_Click"
                                    OnClientClick="return confirm('Voulez-vous vraiment envoyer le mot de passe ?');" />

                            </div>

                        </div>
                    </div>

                </div>
                <footer class="footer pt-3  ">
                    <div class="container-fluid">
                        <div class="row align-items-center justify-content-lg-between">
                            <div class="col-lg-6 mb-lg-0 mb-4">
                                <div class="copyright text-center text-sm text-muted text-lg-start">
                                    ©
                                    <script>
                                        document.write(new Date().getFullYear())
                                    </script>
                                    ,
                Crée par 
                                    <a href="#" class="font-weight-bold" target="_blank">POWERFUL TECH</a>

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
            </div>

            <!-- ==================== MODAL MESSAGE ==================== -->
            <div class="modal fade" id="msgModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-lg">
                    <div class="modal-content d-flex align-items-center p-4 border-0 shadow-lg rounded-3">

                        <!-- icon belle -->
                        <div class="me-4">
                            <svg id="msgIcon" xmlns="http://www.w3.org/2000/svg" width="50" height="50" fill="currentColor" class="text-warning" viewBox="0 0 16 16">
                                <!-- Default: alerte -->
                                <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm.93 4.58a.5.5 0 0 1 .07.7L8.5 7.5v3a.5.5 0 0 1-1 0V7.5l-.5-.22a.5.5 0 1 1 .46-.88l.04.03L8 6.25l.03-.02zM8 12a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                            </svg>
                        </div>

                        <!-- Texte -->
                        <div class="flex-grow-1 text-start">
                            <p id="msgText" class="mb-0 text-gray-700 fs-5"></p>
                        </div>

                        <!-- Bouton OK -->
                        <div class="ms-4">
                            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">OK</button>
                        </div>

                    </div>
                </div>
            </div>

            <!-- Bootstrap JS Bundle -->
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>


            <!-- 1. D'abord, définir le modal -->
            <%-- modal suppression --%>
            <div class="modal fade" id="msgModal1" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-lg">
                    <div class="modal-content d-flex align-items-center p-4 border-0 shadow-lg rounded-3">

                        <!-- Texte -->
                        <div class="flex-grow-1 text-start">
                            <h5 class="mb-1 text-danger fw-bold">Confirmation de suppression</h5>
                            <p class="mb-0 text-gray-700 fs-5">
                                Êtes-vous sûr de vouloir supprimer le professeur sélectionné ?
                    Cette action est irréversible.
                            </p>
                        </div>

                        <!-- Boutons -->
                        <div class="ms-4 d-flex gap-2">
                            <button type="button" class="btn btn-secondary" id="btnNon">
                                Annuler
                            </button>
                            <button type="button" class="btn btn-danger" id="btnOui">
                                Supprimer
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <script>
                function showDeleteModal() {
                    const modal = new bootstrap.Modal(document.getElementById('msgModal1'));
                    modal.show();
                }

                document.getElementById('btnOui').addEventListener('click', function () {

                    // fermer le modal
                    const modalEl = document.getElementById('msgModal1');
                    const modal = bootstrap.Modal.getInstance(modalEl);
                    if (modal) modal.hide();

                    // POSTBACK ASP.NET (clé)
                    __doPostBack('<%= btnSupprimer.UniqueID %>', '');
                });

                document.getElementById('btnNon').addEventListener('click', function () {
                    const modal = bootstrap.Modal.getInstance(document.getElementById('msgModal1'));
                    if (modal) modal.hide();
                });
            </script>

            <script>
                document.addEventListener("DOMContentLoaded", function () {

                    const cinInputs = document.querySelectorAll('.cin');

                    cinInputs.forEach(input => {

                        // 🔢 Autoriser seulement les chiffres et limiter à 10
                        input.addEventListener('input', function () {

                            // Supprimer tout ce qui n'est pas chiffre
                            this.value = this.value.replace(/\D/g, '');

                            // Limiter à 10 chiffres
                            if (this.value.length > 10) {
                                this.value = this.value.slice(0, 10);
                            }
                        });

                    });

                });
            </script>
    </main>
</asp:Content>
