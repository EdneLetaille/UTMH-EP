<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormConnexion.aspx.cs" Inherits="UTMH_Edu.Vue.FormConnexion" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="apple-touch-icon" sizes="76x76" href="../assets/img/apple-icon.png">
    <link rel="icon" type="image/png" href="../assets/img/favicon.png">
    <title>UTMH | Union Des Techniciens Moderne d'Haiti
    </title>
    <!--     Fonts and icons     -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
    <!-- Nucleo Icons -->
    <link href="https://demos.creative-tim.com/argon-dashboard-pro/assets/css/nucleo-icons.css" rel="stylesheet" />
    <link href="https://demos.creative-tim.com/argon-dashboard-pro/assets/css/nucleo-svg.css" rel="stylesheet" />
    <!-- Font Awesome Icons -->
    <script src="https://kit.fontawesome.com/42d5adcbca.js" crossorigin="anonymous"></script>
    <!-- CSS Files -->
    <link id="pagestyle" href="../assets/css/argon-dashboard.css?v=2.1.0" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container position-sticky z-index-sticky top-0">
                <div class="row">
                    <div class="col-12">
                        <!-- Navbar -->
                        <nav class="navbar navbar-expand-lg blur border-radius-lg top-0 z-index-3 shadow position-absolute mt-4 py-2 start-0 end-0 mx-4">
                            <div class="container-fluid">
                                <a class="navbar-brand m-0" href="#">
                                    <img src="../SiteUtilisateur/img/logo1.png" width="150px" height="150px" class="navbar-brand-img h-100" alt="main_logo" />

                                </a>
                                <button class="navbar-toggler shadow-none ms-2" type="button" data-bs-toggle="collapse" data-bs-target="#navigation" aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
                                    <span class="navbar-toggler-icon mt-2">
                                        <span class="navbar-toggler-bar bar1"></span>
                                        <span class="navbar-toggler-bar bar2"></span>
                                        <span class="navbar-toggler-bar bar3"></span>
                                    </span>
                                </button>
                                <div class="collapse navbar-collapse" id="navigation">
                                    <ul class="navbar-nav mx-auto">
                                        <li class="nav-item">
                                            <a class="nav-link d-flex align-items-center me-2 active" aria-current="page" href="Default.aspx">
                                                <i class="fa fa-chart-pie opacity-6 text-dark me-1"></i>
                                                Accueil
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </nav>
                        <!-- End Navbar -->
                    </div>
                </div>
            </div>
            <main class="main-content  mt-0">
                <section>
                    <div class="page-header min-vh-100">
                        <div class="container">
                            <div class="row">
                                <div class="col-xl-4 col-lg-5 col-md-7 d-flex flex-column mx-lg-0 mx-auto">
                                    <div class="card card-plain">
                                        <div class="card-header pb-0 text-start">
                                            <h4 class="font-weight-bolder">Se connecter</h4>
                                            <p class="mb-0">Saisissez votre adresse e-mail et votre mot de passe pour vous connecter.</p>
                                        </div>
                                        <div class="card-body">
                                           
                                                <div class="mb-3">

                                                    <asp:TextBox ID="txtEmail" class="form-control form-control-lg" placeholder="Email" runat="server" TextMode="Email"></asp:TextBox>
                                                </div>
                                                <div class="mb-3">
                                                    <asp:TextBox ID="txtMotPasse" class="form-control form-control-lg" placeholder="Mot de Passe" runat="server" TextMode="Password"></asp:TextBox>
                                                </div>
                                                <div class="text-center">
                                                    <asp:Button ID="btnConnexion" runat="server" Text="Se connecter" class="btn btn-lg btn-primary btn-lg w-100 mt-4 mb-0" OnClick="btnConnexion_Click" />
                                                </div>
                                          
                                        </div>
                                        <div class="card-footer text-center pt-0 px-lg-2 px-1">
                                            <p class="mb-4 text-sm mx-auto">
                                                Vous n'avez pas de compte ?
                    <a href="/Vue/Default.aspx?open=register" class="text-primary text-gradient font-weight-bold">Incrivez-vous</a>
                                            </p>
                                             <p class="mb-4 text-sm mx-auto">
                                                Vous avez oublié votre mot de passe ?
                    <a href="/Vue/FormMotDePasseOublie.aspx" class="text-primary text-gradient font-weight-bold">Mot de passe oublié</a>
                                            </p>
                                                    
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6 d-lg-flex d-none h-100 my-auto pe-0 position-absolute top-0 end-0 text-center justify-content-center flex-column">
                                    <div class="position-relative bg-gradient-primary h-100 m-3 px-7 border-radius-lg d-flex flex-column justify-content-center overflow-hidden" style="background-image: url('../SiteUtilisateur/img/header-bg1.png'); background-size: cover;">
                                        <span class="mask bg-gradient-primary opacity-6"></span>
                                        <h4 class="mt-5 text-white font-weight-bolder position-relative">"Un pays, Une École, Notre avenir."</h4>
                                        <p class="text-white position-relative">L’UTMH est une école professionnelle dédiée à la formation pratique et théorique, offrant aux étudiants les compétences nécessaires pour réussir dans le monde du travail et contribuer au développement de leur communauté.</p>
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
          <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm.93 4.58a.5.5 0 0 1 .07.7L8.5 7.5v3a.5.5 0 0 1-1 0V7.5l-.5-.22a.5.5 0 1 1 .46-.88l.04.03L8 6.25l.03-.02zM8 12a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/>
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

       </div>
        </section>
          </main>

            
            <!--   Core JS Files   -->
            <script src="../assets/js/core/popper.min.js"></script>
            <script src="../assets/js/core/bootstrap.min.js"></script>
            <script src="../assets/js/plugins/perfect-scrollbar.min.js"></script>
            <script src="../assets/js/plugins/smooth-scrollbar.min.js"></script>
            <script>
                var win = navigator.platform.indexOf('Win') > -1;
                if (win && document.querySelector('#sidenav-scrollbar')) {
                    var options = {
                        damping: '0.5'
                    }
                    Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
                }
            </script>
            <!-- Github buttons -->
            <script async defer src="https://buttons.github.io/buttons.js"></script>
            <!-- Control Center for Soft Dashboard: parallax effects, scripts for the example pages etc -->
            <script src="../assets/js/argon-dashboard.min.js?v=2.1.0"></script>
            <!--MODAL Bootstrap JS Bundle  -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
        </div>
    </form>
    </body>
</html>
