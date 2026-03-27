<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormDossierEtudiant.aspx.cs" Inherits="UTMH_Edu.Vue.FormDossierEtudiant" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>

        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
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
                            <p class="text-uppercase text-sm">Informations de l’étudiant</p>
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
                                      <%--  <label for="example-text-input" class="form-control-label">Code</label>--%>
                                        <asp:TextBox ID="txtCode" class="form-control" runat="server" visible="false"></asp:TextBox>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Prenom</label>
                                        <asp:TextBox ID="txtPrenom" class="form-control" runat="server"  oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        "></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Nom</label>
                                        <asp:TextBox ID="txtNom" class="form-control" runat="server"  oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        "></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Date de naissance</label>
                                        <asp:TextBox ID="txtDateNaissance" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Sexe</label>
                                        <asp:DropDownList ID="ddlSexe" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Value="Féminin">Féminin</asp:ListItem>
                                            <asp:ListItem Value="Masculin">Masculin</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
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
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Adresse</label>
                                        <asp:TextBox ID="txtAdresse" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="tEmail" class="form-control-label">Email</label>
                                        <asp:TextBox ID="txtEmail" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">cin</label>
                                        <asp:TextBox ID="txtCin" class="form-control cin" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Telephone</label>
                                        <asp:TextBox ID="txtTelephone" class="form-control phone" runat="server" TextMode="Phone"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Personne Responsable</label>
                                        <asp:TextBox ID="txtPerResponsable" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Option choisie</label>
                                        <asp:DropDownList ID="ddlOption" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Telephone Responsable</label>
                                        <asp:TextBox ID="txtTelephoneRes" class="form-control phone" runat="server" TextMode="Phone"></asp:TextBox>
                                    </div>
                                </div>
                               
                                 <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Statut</label>
                                        <asp:DropDownList ID="ddlStatut" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Text="Actif" Value="Actif" />
                                            <asp:ListItem Text="Inactif" Value="Inactif" />
                                            <asp:ListItem Text="En attente" Value="En attente" />

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                
                                        <asp:FileUpload ID="fuPhotoProfil" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtDateInscription"  runat="server" TextMode="Date" visible="false"></asp:TextBox>


                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnect %>" SelectCommand="SELECT * FROM [option]"></asp:SqlDataSource>
                               


                                     <%--   <label for="example-text-input" class="form-control-label">Annee Academique</label>--%>
                                        <asp:DropDownList ID="ddlAnneeAcademique" runat="server" CssClass="form-control" Visible="false">
                                        </asp:DropDownList>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Statut Paiement</label>
                                        <asp:DropDownList ID="ddlStatutPaiement" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Text="Payé" Value="Payé" />
                                            <asp:ListItem Text="Non payé" Value="NonPaye" />

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                 <div class="mb-3 photo-card">
                    <label class="form-label">Photo Etudiant</label>

                    <div class="d-flex align-items-center gap-3 flex-wrap">
                        <div class="avatar-wrap">
                            <asp:Image ID="imgPreview" runat="server" CssClass="avatar"
                                ImageUrl="~/image/user-default.png" AlternateText="Photo Etudiant" />

                            <div class="camera-btn" title="Caméra"
                                 data-bs-toggle="modal" data-bs-target="#cameraModal"
                                 onclick="openCamera();">
                                <i class="bi bi-camera-fill"></i>
                            </div>
                        </div>

                        <div class="d-flex gap-2 flex-wrap">
                            <button type="button" class="btn btn-soft" onclick="triggerUpload();">
                                <i class="bi bi-upload"></i> Importer une photo
                            </button>

                            <button type="button" class="btn btn-soft"
                                    data-bs-toggle="modal" data-bs-target="#cameraModal"
                                    onclick="openCamera();">
                                <i class="bi bi-camera"></i> Prendre une photo
                            </button>
                        </div>
                    </div>

                    <asp:FileUpload ID="fuPhoto" runat="server" Style="display:none;" accept="image/*" />
                    <asp:HiddenField ID="hfPhotoBase64" runat="server" />
                </div>v
                                


                            </div>
                            <div class="row">






                                <div class="col-md-4">
                                    <div class="form-group">
                                        <!-- <label for="example-text-input" class="form-control-label">Role</label> -->
                                        <asp:TextBox ID="txtRole" class="form-control" runat="server" Visible="False"></asp:TextBox>
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

         <!-- Modal Caméra -->
        <div class="modal fade" id="cameraModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content" style="border-radius: 16px; overflow: hidden;">
                    <div class="modal-header">
                        <h5 class="modal-title fw-bold text-primary">
                            <i class="bi bi-camera"></i>Prendre une photo
                        </h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"
                            aria-label="Close" onclick="closeCamera();">
                        </button>
                    </div>

                    <div class="modal-body">
                        <div class="row g-3">
                            <div class="col-md-7">
                                <video id="video" autoplay="autoplay" playsinline="playsinline"
                                    style="width: 100%; border-radius: 14px; background: #000;">
                                </video>
                            </div>
                            <div class="col-md-5">
                                <canvas id="canvas" style="display: none;"></canvas>

                                <div class="text-muted mb-2">
                                    Cliquez sur <b>Capturer</b>, puis sur <b>Utiliser</b> pour appliquer la photo.
                                </div>

                                <img id="shotPreview" src="" alt="Aperçu"
                                    style="width: 100%; border-radius: 14px; border: 1px solid rgba(0,0,0,.08); display: none;" />

                                <div class="d-flex gap-2 mt-3 flex-wrap">
                                    <button type="button" class="btn btn-primary fw-bold" onclick="capturePhoto();">
                                        <i class="bi bi-camera-fill"></i>Capturer
                                    </button>

                                    <button type="button" class="btn btn-success fw-bold"
                                        onclick="usePhoto();" id="btnUse" disabled="disabled">
                                        <i class="bi bi-check2-circle"></i>Utiliser
                                    </button>

                                    <button type="button" class="btn btn-outline-danger fw-bold"
                                        onclick="closeCamera();" data-bs-dismiss="modal">
                                        Fermer
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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

       <script>
document.addEventListener("DOMContentLoaded", function () {

    const phoneInputs = document.querySelectorAll('.phone');
    const prefix = "+509";
    const maxDigits = 8;
    const firstDigits = ["2", "3", "4", "5"]; // chiffres autorisés

    function normalizePhone(input) {

        let digits = input.value.replace(/\D/g, "");

        if (digits.startsWith("509")) digits = digits.slice(3);

        digits = digits.slice(0, maxDigits);

        // Vérifier premier chiffre valide
        if (digits.length > 0 && !firstDigits.includes(digits[0])) {
            digits = "";
        }

        input.value = prefix + digits;
    }

    phoneInputs.forEach(input => {

        input.addEventListener("focus", () => {
            if (!input.value.startsWith(prefix)) input.value = prefix;
            setTimeout(() => input.setSelectionRange(input.value.length, input.value.length), 0);
        });

        input.addEventListener("keydown", (e) => {

            const pos = input.selectionStart;

            if (pos <= prefix.length && (e.key === "Backspace" || e.key === "Delete")) {
                e.preventDefault();
                return;
            }

            const allowed = ["ArrowLeft","ArrowRight","Tab","Backspace","Delete","Home","End"];
            if (allowed.includes(e.key) || e.ctrlKey || e.metaKey) return;

            if (pos >= prefix.length && !/^\d$/.test(e.key)) {
                e.preventDefault();
                return;
            }

            let digits = input.value.slice(prefix.length);

            // Vérifier premier chiffre
            if (digits.length === 0 && !firstDigits.includes(e.key)) {
                e.preventDefault();
                return;
            }

            if (digits.length >= maxDigits) {
                e.preventDefault();
            }

        });

        input.addEventListener("input", () => normalizePhone(input));

        input.addEventListener("paste", (e) => {
            e.preventDefault();
            const pasted = (e.clipboardData || window.clipboardData).getData("text");
            input.value = pasted;
            normalizePhone(input);
        });

        if (input.value.trim() !== "") normalizePhone(input);
    });

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
        <!-- 1. D'abord, définir le modal -->
        <%-- modal suppression --%>
        <div class="modal fade" id="msgModal1" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content d-flex align-items-center p-4 border-0 shadow-lg rounded-3">

                    <!-- Texte -->
                    <div class="flex-grow-1 text-start">
                        <h5 class="mb-1 text-danger fw-bold">Confirmation de suppression</h5>
                        <p class="mb-0 text-gray-700 fs-5">
                            Êtes-vous sûr de vouloir supprimer l'étudiant sélectionné ?
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
document.addEventListener("DOMContentLoaded", function () {

    // =========================
    // 📂 UPLOAD IMAGE
    // =========================
    const fileInput = document.getElementById('<%= fuPhoto.ClientID %>');

    if (fileInput) {
        fileInput.addEventListener('change', function () {
            const file = this.files[0];
            if (!file) return;

            const reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById('<%= imgPreview.ClientID %>').src = e.target.result;
                document.getElementById('<%= hfPhotoBase64.ClientID %>').value = e.target.result;
            };
            reader.readAsDataURL(file);
        });
    }

    // =========================
    // 🗑️ MODAL SUPPRESSION
    // =========================
    const btnOui = document.getElementById('btnOui');
    const btnNon = document.getElementById('btnNon');

    if (btnOui) {
        btnOui.addEventListener('click', function () {
            const modalEl = document.getElementById('msgModal1');
            const modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) modal.hide();

            __doPostBack('<%= btnSupprimer.UniqueID %>', '');
        });
    }

    if (btnNon) {
        btnNon.addEventListener('click', function () {
            const modal = bootstrap.Modal.getInstance(document.getElementById('msgModal1'));
            if (modal) modal.hide();
        });
    }

});


// =========================
// 📸 CAMERA
// =========================
let stream = null;

function openCamera() {
    navigator.mediaDevices.getUserMedia({ video: true })
        .then(function (mediaStream) {
            stream = mediaStream;
            document.getElementById("video").srcObject = mediaStream;

            resetCameraPreview();
        })
        .catch(function (err) {
            console.log(err);
            alert("Impossible d'accéder à la caméra.");
        });
}

function capturePhoto() {
    const video = document.getElementById("video");
    const canvas = document.getElementById("canvas");
    const context = canvas.getContext("2d");

    canvas.width = video.videoWidth || 640;
    canvas.height = video.videoHeight || 480;

    context.drawImage(video, 0, 0, canvas.width, canvas.height);

    const imageData = canvas.toDataURL("image/jpeg", 0.9);

    document.getElementById("shotPreview").src = imageData;
    document.getElementById("shotPreview").style.display = "block";
    document.getElementById("btnUse").disabled = false;
}

function usePhoto() {
    const imageData = document.getElementById("shotPreview").src;
    if (!imageData) return;

    document.getElementById('<%= imgPreview.ClientID %>').src = imageData;
    document.getElementById('<%= hfPhotoBase64.ClientID %>').value = imageData;

    closeCamera();
    forceCloseModal("cameraModal");
}


// =========================
// 📂 TRIGGER UPLOAD
// =========================
function triggerUpload() {
    resetCameraPreview();
    document.getElementById('<%= fuPhoto.ClientID %>').click();
}


// =========================
// 🔄 RESET CAMERA PREVIEW
// =========================
function resetCameraPreview() {
    document.getElementById("shotPreview").style.display = "none";
    document.getElementById("shotPreview").src = "";
    document.getElementById("btnUse").disabled = true;
}


// =========================
// ❌ CLOSE CAMERA
// =========================
function closeCamera() {
    if (stream) {
        stream.getTracks().forEach(t => t.stop());
        stream = null;
    }

    const video = document.getElementById("video");
    if (video) video.srcObject = null;
}


// =========================
// 🔒 FORCE CLOSE MODAL
// =========================
function forceCloseModal(modalId) {
    const el = document.getElementById(modalId);
    if (!el) return;

    let instance = bootstrap.Modal.getInstance(el);
    if (!instance) instance = new bootstrap.Modal(el);

    instance.hide();

    setTimeout(() => {
        document.body.classList.remove("modal-open");
        document.body.style.removeProperty("padding-right");
        document.querySelectorAll(".modal-backdrop").forEach(b => b.remove());

        el.classList.remove("show");
        el.style.display = "none";
        el.removeAttribute("aria-modal");
        el.setAttribute("aria-hidden", "true");
    }, 200);
}


// =========================
// 📴 AUTO STOP CAMERA
// =========================
document.addEventListener("hidden.bs.modal", function (e) {
    if (e.target && e.target.id === "cameraModal") {
        closeCamera();
    }
});


// =========================
// 🗑️ OPEN DELETE MODAL
// =========================
function showDeleteModal() {
    const modal = new bootstrap.Modal(document.getElementById('msgModal1'));
    modal.show();
}
</script>




    </main>
</asp:Content>
