<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormEtudiant.aspx.cs" Inherits="UTMH_Edu.Vue.FormEtudiant" %>

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
                            <div class="row">
                                      <%--  <label for="example-text-input" class="form-control-label">Code</label>--%>
                                        <asp:TextBox ID="txtCode" class="form-control" runat="server" Visible="false"></asp:TextBox>
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
                                        <label for="example-text-input" class="form-control-label">Date de naissance</label>
                                        <asp:TextBox ID="txtDateNaissance" class="form-control" runat="server" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Sexe</label>
                                        <asp:DropDownList ID="ddlSexe" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem>Féminin</asp:ListItem>
                                            <asp:ListItem>Masculin</asp:ListItem>
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
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Adresse</label>
                                        <asp:TextBox ID="txtAdresse" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="tEmail" class="form-control-label">Email</label>
                                        <asp:TextBox ID="txtEmail" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                               
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label ">Cin</label>
                                        <asp:TextBox ID="txtCin" CssClass="form-control cin" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="ddlSexe" class="form-control-label">Statut Paiement</label>
                                        <asp:DropDownList ID="ddlStatutPaiement" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Text="Payé" Value="Paye" />
                                            <asp:ListItem Text="Non payé" Value="NonPaye" />

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label  ">Telephone</label>
                                        <asp:TextBox ID="txtTelephone" CssClass="form-control phone" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label>Personne Responsable</label>
                                    <asp:TextBox ID="txtPerResponsable" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
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
                                        <label for="example-text-input" class="form-control-label  ">Telephone Responsable</label>
                                        <asp:TextBox ID="txtTelephoneRes" CssClass="form-control phone" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                               <!-- Photo Etudiant -->
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
                </div>

                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnect %>" SelectCommand="SELECT * FROM [option]"></asp:SqlDataSource>

                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <%--   <label for="ddlSexe" class="form-control-label">Statut</label>--%>
                                        <asp:DropDownList ID="ddlStatut" runat="server" CssClass="form-control" Width="100%" Visible="false">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Text="Actif" Value="Actif" />
                                            <asp:ListItem Text="Inactif" Value="Inactif" />
                                            <asp:ListItem Text="En attente" Value="EnAttente" />

                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <div class="col-md-4">
                                    <div class="form-group">
                                        <%-- <label for="example-text-input" class="form-control-label">Annee Academique</label>--%>
                                        <asp:DropDownList ID="ddlAnneeAcademique" class="form-control" runat="server" TextMode="Date" Visible="false"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <%-- <label for="ddlSexe" class="form-control-label">Mode Paiement</label>--%>
                                        <asp:DropDownList ID="ddlModePaiement" runat="server" CssClass="form-control" Width="100%" Visible="false">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group">
                                        <%--    <label for="example-text-input" class="form-control-label">Date Inscription</label>--%>
                                        <asp:TextBox ID="txtDateInscription" class="form-control" runat="server" TextMode="Date" Visible="false"></asp:TextBox>
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



                            </div>

                            <hr class="horizontal dark" />
                            <div class="text-center">
                                <asp:Button ID="btnInscrire" class="btn btn-primary btn-sm" runat="server" Text="Inscrire" OnClick="btnInscrire_Click" />
                                <asp:Button ID="btnAnnuler" class="btn btn-secondary btn-sm me-2" runat="server" Text="Annuler" OnClick="btnAnnuler_Click" />
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
                const maxDigits = 8; // après +509

                function normalizePhone(input) {
                    // Récupère uniquement les chiffres tapés
                    let digits = input.value.replace(/\D/g, "");

                    // Si l'utilisateur a tapé/collé 509 au début, on l'enlève
                    if (digits.startsWith("509")) digits = digits.slice(3);

                    // Limite à 8 chiffres
                    digits = digits.slice(0, maxDigits);

                    // Reconstruit la valeur finale
                    input.value = prefix + digits;
                }

                phoneInputs.forEach(input => {

                    // Focus: impose le prefix
                    input.addEventListener("focus", () => {
                        if (!input.value.startsWith(prefix)) input.value = prefix;
                        // place le curseur à la fin
                        setTimeout(() => input.setSelectionRange(input.value.length, input.value.length), 0);
                    });

                    // Keydown: empêche de supprimer/modifier +509 + empêche non-chiffres
                    input.addEventListener("keydown", (e) => {
                        const pos = input.selectionStart;

                        // Bloquer suppression dans le prefix
                        if (pos <= prefix.length && (e.key === "Backspace" || e.key === "Delete")) {
                            e.preventDefault();
                            return;
                        }

                        // Autoriser navigation / contrôle
                        const allowed = ["ArrowLeft", "ArrowRight", "Tab", "Backspace", "Delete", "Home", "End"];
                        if (allowed.includes(e.key) || e.ctrlKey || e.metaKey) return;

                        // Après le prefix: uniquement chiffres
                        if (pos >= prefix.length && !/^\d$/.test(e.key)) {
                            e.preventDefault();
                            return;
                        }

                        // Bloquer si déjà 8 chiffres après prefix
                        const digitsCount = input.value.slice(prefix.length).replace(/\D/g, "").length;
                        if (pos >= prefix.length && digitsCount >= maxDigits) {
                            e.preventDefault();
                        }
                    });

                    // Input: sécurité totale (bloque lettres même si elles passent autrement)
                    input.addEventListener("input", () => normalizePhone(input));

                    // Paste: normalisation
                    input.addEventListener("paste", (e) => {
                        e.preventDefault();
                        const pasted = (e.clipboardData || window.clipboardData).getData("text");
                        input.value = pasted;
                        normalizePhone(input);
                    });

                    // Au chargement si champ prérempli
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

       <script>
let stream = null;

function openCamera() {
    navigator.mediaDevices.getUserMedia({ video: true })
        .then(function (mediaStream) {
            stream = mediaStream;
            document.getElementById("video").srcObject = mediaStream;

            // reset preview chak fwa
            document.getElementById("shotPreview").style.display = "none";
            document.getElementById("shotPreview").src = "";
            document.getElementById("btnUse").disabled = true;
        })
        .catch(function (err) {
            console.log(err);
            alert("Impossible d'accéder à la caméra. Vérifiez les permissions du navigateur.");
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

    // appliquer photo
    document.getElementById('<%= imgPreview.ClientID %>').src = imageData;
    document.getElementById('<%= hfPhotoBase64.ClientID %>').value = imageData;

    closeCamera();
    forceCloseModal("cameraModal"); // ✅ ferme proprement
}

function closeCamera() {
    if (stream) {
        stream.getTracks().forEach(t => t.stop());
        stream = null;
    }

    const video = document.getElementById("video");
    if (video) video.srcObject = null;
}

// ✅ fermeture robuste (nettoie backdrop + modal-open)
function forceCloseModal(modalId) {
    const el = document.getElementById(modalId);
    if (!el) return;

    let instance = bootstrap.Modal.getInstance(el);
    if (!instance) instance = new bootstrap.Modal(el);

    instance.hide();

    // attendre un peu puis nettoyer overlay si besoin
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

// optionnel: quand user ferme modal la (X / Fermer)
document.addEventListener("hidden.bs.modal", function (e) {
    if (e.target && e.target.id === "cameraModal") {
        closeCamera();
    }
});
</script>


        <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.8/jquery.inputmask.min.js"></script>


    </main>
</asp:Content>
