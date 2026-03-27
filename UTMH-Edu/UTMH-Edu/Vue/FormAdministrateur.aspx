<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormAdministrateur.aspx.cs" Inherits="UTMH_Edu.Vue.FormAdministrateur" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Membres</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->

        <div class="container-fluid py-4">
            <div class="row">
                <div class="col-md-8">
                    <div class="card">

                        <div class="card-body">
                            <p class="text-uppercase text-sm">Informations Sur le Membre</p>
                            <div class="row">
                               
                                <div class="col-md-6">
                                    <div class="form-group">
                                    <label>Prénom</label>
                                    <asp:TextBox ID="txtPrenom" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                </div>

                                 </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                    <label>Nom</label>
                                    <asp:TextBox ID="txtNom" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                </div>
                                     </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="textEmail" class="form-control-label">Email</label>
                                        <asp:TextBox ID="txtEmail" class="form-control" runat="server" TextMode="Email"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label  ">Telephone</label>
                                        <asp:TextBox ID="txtTelephone" CssClass="form-control phone" runat="server"></asp:TextBox>
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
                                        <label for="ddlSexe" class="form-control-label">Sexe</label>
                                        <asp:DropDownList ID="ddlSexe" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem>Feminin</asp:ListItem>
                                            <asp:ListItem>Masculin</asp:ListItem>
                                        </asp:DropDownList>
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
                                    <label for="ddlSexe" class="form-control-label">Role</label>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" Width="100%">
                                        <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>

                                        <asp:ListItem>Comptable</asp:ListItem>
                                        <asp:ListItem>Secretaire</asp:ListItem>
                                        <asp:ListItem>Responsable</asp:ListItem>
                                        <asp:ListItem>Administrateur</asp:ListItem>

                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:TextBox ID="txtDateEmbauche" runat="server" TextMode="Date" Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtStatut" runat="server" Visible="false"></asp:TextBox>

                        </div>
                    </div>


                    <hr class="horizontal dark" />
                    <div class="text-center">
                        <asp:Button ID="btnInscrire" class="btn btn-primary btn-sm" runat="server" Text="Inscrire" OnClick="btnInscrire_Click" />
                        <asp:Button ID="btnAnnuler" class="btn btn-secondary btn-sm me-2" runat="server" Text="Annuler" />
                    </div>
                                       <%-- <label for="example-text-input" class="form-control-label">Code</label>--%>
                                        <asp:TextBox ID="txtCode" class="form-control" runat="server" visible="false"></asp:TextBox>
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

                      const allowed = ["ArrowLeft", "ArrowRight", "Tab", "Backspace", "Delete", "Home", "End"];
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


    </main>
</asp:Content>
