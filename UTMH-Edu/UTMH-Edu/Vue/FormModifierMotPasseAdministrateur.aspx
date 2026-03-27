<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormModifierMotPasseAdministrateur.aspx.cs" Inherits="UTMH_Edu.Vue.FormModifierMotPasseAdministrateur" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />

    <style>
        .card-soft {
            border-radius: 18px;
            border: 0;
            box-shadow: 0 10px 26px rgba(0,0,0,.06);
        }

        .page-title {
            font-weight: 800;
            color: #fff;
            margin: 0;
        }

        .section-title {
            font-weight: 700;
            letter-spacing: .08em;
            font-size: .75rem;
            color: #6b7280;
            text-transform: uppercase;
            margin-bottom: 10px;
        }

        .input-icon {
            position: relative;
        }

        .input-icon i {
            position: absolute;
            left: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: #6b7280;
            font-size: 1.1rem;
        }

        .input-icon .form-control {
            padding-left: 40px;
            padding-right: 46px;
        }

        .toggle-eye {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            border: 0;
            background: transparent;
            color: #111827;
            cursor: pointer;
            padding: 6px;
        }

        .hint {
            font-size: .85rem;
            color: #6b7280;
        }
    </style>

    <main>

        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <h6 class="page-title"><asp:Label ID="lbNomComplet" runat="server" Text="Membre"></asp:Label></h6>
            </div>
        </nav>

        <div class="container-fluid py-4">
            <div class="row">
                <div class="col-lg-7 col-xl-6 mx-auto">

                    <div class="card card-soft">
                        <div class="card-body p-4">

                            <div class="d-flex align-items-center justify-content-between mb-3">
                                <div>
                                    <div class="section-title mb-1">Sécurité du compte</div>
                                    <h5 class="mb-0" style="font-weight:800;color:#111827;">Changement de mot de passe</h5>
                                </div>

                                <div class="d-none d-md-block">
                                    <span class="badge bg-dark">
                                        <i class="bi bi-shield-lock me-1"></i> Membre
                                    </span>
                                </div>
                            </div>

                            <p class="hint mb-4">
                                Pour sécuriser ton compte, choisis un mot de passe fort.  
                                Idéalement : <b>8 caractères+</b>, une <b>majuscule</b>, un <b>chiffre</b>.
                            </p>

                            <div class="row g-3">
                                <!-- Ancien mot de passe -->
                                <div class="col-md-12">
                                    <label class="form-control-label">Ancien mot de passe</label>
                                    <div class="input-icon">
                                        <i class="bi bi-lock"></i>
                                        <asp:TextBox ID="txtAncienPasse" runat="server" TextMode="Password" CssClass="form-control" placeholder="Entrez votre ancien mot de passe"></asp:TextBox>
                                        <button type="button" class="toggle-eye" onclick="togglePwd('<%= txtAncienPasse.ClientID %>', this)" title="Afficher/Masquer">
                                            <i class="bi bi-eye"></i>
                                        </button>
                                    </div>
                                </div>

                                <!-- Nouveau mot de passe -->
                                <div class="col-md-12">
                                    <label class="form-control-label">Nouveau mot de passe</label>
                                    <div class="input-icon">
                                        <i class="bi bi-key"></i>
                                        <asp:TextBox ID="txtNouveauPasse" runat="server" TextMode="Password" CssClass="form-control" placeholder="Choisissez un nouveau mot de passe"></asp:TextBox>
                                        <button type="button" class="toggle-eye" onclick="togglePwd('<%= txtNouveauPasse.ClientID %>', this)" title="Afficher/Masquer">
                                            <i class="bi bi-eye"></i>
                                        </button>
                                    </div>
                                </div>

                                <!-- Confirmer -->
                                <div class="col-md-12">
                                    <label class="form-control-label">Confirmer le mot de passe</label>
                                    <div class="input-icon">
                                        <i class="bi bi-check2-circle"></i>
                                        <asp:TextBox ID="txtConfPasse" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirmez le mot de passe"></asp:TextBox>
                                        <button type="button" class="toggle-eye" onclick="togglePwd('<%= txtConfPasse.ClientID %>', this)" title="Afficher/Masquer">
                                            <i class="bi bi-eye"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <hr class="horizontal dark my-4" />

                            <div class="d-flex justify-content-center gap-2">
                                <asp:Button ID="btnModifierMotPasseEtudiant" runat="server"
                                    CssClass="btn btn-primary px-4"
                                    Text="Modifier"
                                    OnClick="btnModifierMotPasse_Click" />

                                <asp:Button ID="btnAnnuler" runat="server"
                                    CssClass="btn btn-outline-secondary px-4"
                                    Text="Annuler"
                                    OnClick="btnAnnuler_Click" />
                            </div>

                        </div>
                    </div>

                </div>
            </div>

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

            <script>
                function togglePwd(inputId, btn) {
                    const input = document.getElementById(inputId);
                    if (!input) return;

                    if (input.type === "password") {
                        input.type = "text";
                        btn.innerHTML = '<i class="bi bi-eye-slash"></i>';
                    } else {
                        input.type = "password";
                        btn.innerHTML = '<i class="bi bi-eye"></i>';
                    }
                }
            </script>

        </div>
    </main>

</asp:Content>
