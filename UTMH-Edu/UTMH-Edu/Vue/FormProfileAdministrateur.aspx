<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormProfileAdministrateur.aspx.cs" Inherits="UTMH_Edu.Vue.FormProfileAdministrateur" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />

    <style>
        .profile-cover {
            position: relative;
            border-radius: 18px;
            overflow: hidden;
            min-height: 190px;
            background: linear-gradient(120deg, rgba(17,24,39,.95), rgba(59,130,246,.70));
        }
        .profile-cover::after {
            content: "";
            position: absolute;
            inset: 0;
            background-image:
                radial-gradient(circle at 20% 20%, rgba(255,255,255,.10), transparent 55%),
                radial-gradient(circle at 80% 30%, rgba(255,255,255,.09), transparent 50%),
                radial-gradient(circle at 60% 90%, rgba(255,255,255,.06), transparent 55%);
        }
        .profile-header {
            position: relative;
            margin-top: -70px;
            display: flex;
            gap: 18px;
            align-items: end;
            padding: 0 18px 12px 18px;
        }
        .avatar-wrap {
            position: relative;
            width: 135px;
            height: 135px;
            flex: 0 0 135px;
        }
        .avatar-img {
            width: 135px;
            height: 135px;
            object-fit: cover;
            border-radius: 50%;
            border: 4px solid #fff;
            box-shadow: 0 10px 26px rgba(0,0,0,.18);
            background: #f3f4f6;
        }
        .camera-btn {
            position: absolute;
            right: 8px;
            bottom: 8px;
            width: 42px;
            height: 42px;
            border-radius: 50%;
            border: 0;
            display: grid;
            place-items: center;
            background: #111827;
            color: #fff;
            box-shadow: 0 10px 22px rgba(0,0,0,.20);
            cursor: pointer;
            z-index: 2;
        }
        .camera-btn:hover { filter: brightness(1.1); }
        .profile-title h5 {
            margin: 0;
            font-weight: 800;
            color: #111827;
        }
        .profile-title small {
            color: #6b7280;
            display: inline-block;
            margin-top: 2px;
        }
        .card-soft {
            border-radius: 18px;
            border: 0;
            box-shadow: 0 10px 26px rgba(0,0,0,.06);
        }
        .section-title {
            font-weight: 700;
            letter-spacing: .08em;
            font-size: .75rem;
            color: #6b7280;
            text-transform: uppercase;
            margin-top: 8px;
            margin-bottom: 10px;
        }
    </style>

    <main>

        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <h6 class="font-weight-bolder text-white mb-0">Profil Membre</h6>
                </nav>
            </div>
        </nav>

        <div class="container-fluid py-4">
            <div class="row">
                <div class="col-lg-10 col-xl-9 mx-auto">

                    <div class="card card-soft">
                        <div class="card-body">

                            <!-- Cover -->
                            <div class="profile-cover mb-3"></div>

                            <!-- Header -->
                            <div class="profile-header">

                                <!-- Avatar -->
                                <div class="avatar-wrap">
                                    <asp:Image ID="imgEtudiant" runat="server" CssClass="avatar-img" ImageUrl="~/uploads/default-user.png" />

                                    <button type="button" class="camera-btn" onclick="openFilePicker(); return false;" title="Changer la photo">
                                        <i class="bi bi-camera"></i>
                                    </button>

                                    <!-- FileUpload caché -->
                                    <asp:FileUpload ID="fuPhotoProfil" runat="server" Style="display:none;" />

                                    <!-- Bouton caché postback upload auto -->
                                    <asp:Button ID="btnAutoUpload" runat="server" Text="Upload"
                                        OnClick="btnAutoUpload_Click" Style="display:none;" UseSubmitBehavior="true" />
                                </div>

                                <!-- Infos à droite -->
                                <div class="profile-title">
                                    <h5><asp:Label ID="lblNomComplet" runat="server" Text=""></asp:Label></h5>                                  
                                </div>

                            </div><!-- /profile-header -->

                            <div class="section-title">Informations personnelles</div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Code</label>
                                        <asp:TextBox ID="txtCode" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Prénom</label>
                                        <asp:TextBox ID="txtPrenom" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Nom</label>
                                        <asp:TextBox ID="txtNom" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>                            
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Email</label>
                                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                                    <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Telephone</label>
                                        <asp:TextBox ID="txtTelephone" class="form-control" runat="server" TextMode="Number" ReadOnly="True"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="ddlRole" class="form-control-label">Role</label>
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>

                                            <asp:ListItem>Comptable</asp:ListItem>
                                            <asp:ListItem>Secretaire</asp:ListItem>
                                            <asp:ListItem>Responsable</asp:ListItem>
                                            <asp:ListItem>Administrateur</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Date d'embauche</label>
                                        <asp:TextBox ID="txtDateEmbauche" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>

                              

                                <div class="col-md-4 d-flex align-items-end">
                                    <a href="FormModifierMotPasseAdministrateur.aspx" class="btn btn-dark w-100">
                                        <i class="bi bi-shield-lock me-1"></i> Modifier mot de passe
                                    </a>
                                </div>
                            </div>

                            <hr class="horizontal dark" />

                            <div class="d-flex justify-content-center gap-2">
                                <asp:Button ID="btnRetour" CssClass="btn btn-outline-secondary" runat="server" Text="Retour" OnClick="btnRetour_Click" />
                            </div>

                        </div>
                    </div>

                </div>
            </div>

            <!-- Modal message -->
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
                function openFilePicker() {
                    const fu = document.getElementById('<%= fuPhotoProfil.ClientID %>');
                    if (fu) fu.click();
                }

                document.addEventListener('DOMContentLoaded', function () {
                    const fu = document.getElementById('<%= fuPhotoProfil.ClientID %>');
                    const img = document.getElementById('<%= imgEtudiant.ClientID %>');
                    const btnUpload = document.getElementById('<%= btnAutoUpload.ClientID %>');

                    if (!fu || !img || !btnUpload) return;

                    fu.addEventListener('change', function () {
                        if (!fu.files || !fu.files[0]) return;

                        const file = fu.files[0];

                        // Vérifier image
                        if (!file.type.startsWith('image/')) {
                            alert('Veuillez choisir une image (jpg/png).');
                            fu.value = '';
                            return;
                        }

                        // Preview immédiat
                        img.src = URL.createObjectURL(file);

                        // Upload auto = postback
                        btnUpload.click();
                    });
                });
            </script>

        </div>
    </main>

</asp:Content>
