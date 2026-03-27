<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormCours.aspx.cs" Inherits="UTMH_Edu.Vue.FormCours" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <style>        .checkbox-group li {
    list-style: none;
    margin-bottom: 12px;
    padding: 10px;
    border-radius: 4px;
    background-color: #f9f9f9;
    border: 1px solid #e0e0e0;
}

.checkbox-group input[type="checkbox"] {
    margin-right: 10px;
}
#optionsPanel {
    top: 100%;
}


.checkbox-group label {
    font-weight: 500;
    cursor: pointer;
    margin: 0;
}</style>

        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Cours</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->

        <div class="container-fluid py-4">
            <div class="row">
                <div class="col-md-8">
                    <div class="card">

                        <div class="card-body">
                            <p class="text-uppercase text-sm">Informations du cours</p>
                            <div class="row">
                               
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Nom</label>
                                        <asp:TextBox ID="txtNom" class="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        "></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Description</label>
                                        <asp:TextBox ID="txtDescription" class="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        "></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Duree</label>
                                        <asp:TextBox ID="txtDuree" class="form-control" runat="server" TextMode="Number"  AutoPostBack="true"  Min="1" Max="5"  onchange="calculHeureFin()"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Heure Debut</label>
                                        <asp:TextBox ID="txtHeureDebut" class="form-control" runat="server" TextMode="Time" OnTextChanged="CalculerHeureFin"  AutoPostBack="true"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Heure Fin</label>
                                        <asp:TextBox ID="txtHeureFin" class="form-control" runat="server" TextMode="Time" OnTextChanged="CalculerHeureFin" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>                            
                                     <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="form-control-label">Nom Professeur</label>
                                        <asp:DropDownList ID="ddlNomProfesseur" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>

                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="example-text-input" class="form-control-label">Coefficient</label>
                                        <asp:TextBox ID="txtCoefficient" class="form-control" runat="server" TextMode="Number" Min="1"></asp:TextBox>
                                    </div>
                                </div>
                                        <%--   <label for="ddlSexe" class="form-control-label">Statut</label>--%>
                                        <asp:DropDownList ID="ddlStatut" runat="server" CssClass="form-control" Width="100%" Visible="false">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Text="Actif" Value="Actif" />
                                            <asp:ListItem Text="Inactif" Value="Inactif" />                                       

                                        </asp:DropDownList>
                                
                                      <%--  <label for="example-text-input" class="form-control-label">Code</label>--%>
                                        <asp:TextBox ID="txtCode" class="form-control" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                  
    <!-- OPTIONS DU COURS -->
    <div class="col-md-6 position-relative">
        <div class="form-group">
            <label class="form-control-label">Options du cours</label>

            <button type="button"
                class="form-control text-start"
                onclick="toggleOptions()">
                Sélectionner les options
            </button>

            <div id="optionsPanel"
                class="border rounded p-2 bg-white shadow"
                style="
                    display:none;
                    position:absolute;
                    top:100%;
                    left:0;
                    width:100%;
                    max-height:200px;
                    overflow-y:auto;
                    z-index:1050;
                ">

                <asp:CheckBoxList
                    ID="chkOptions"
                    runat="server"
                    CssClass="checkbox-group">
                </asp:CheckBoxList>

            </div>
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
    function toggleOptions() {
        const panel = document.getElementById("optionsPanel");
        panel.style.display = panel.style.display === "none" ? "block" : "none";
    }
</script>

     


    </main>
</asp:Content>
