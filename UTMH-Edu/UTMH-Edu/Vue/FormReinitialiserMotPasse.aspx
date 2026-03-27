<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormReinitialiserMotPasse.aspx.cs" Inherits="UTMH_Edu.Vue.FormReinitialiserMotPasse" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="apple-touch-icon" sizes="76x76" href="../assets/img/apple-icon.png" />
    <link rel="icon" type="image/png" href="../assets/img/favicon.png" />
    <title>Réinitialisation mot de passe</title>

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
            <main class="main-content mt-0">
                <section>
                    <div class="page-header min-vh-100">
                        <div class="container">
                            <div class="row">
                                <div class="col-xl-4 col-lg-5 col-md-7 d-flex flex-column mx-lg-0 mx-auto">
                                    <div class="card card-plain">
                                        <div class="card-header pb-0 text-start">
                                            <h4 class=" text-center font-weight-bolder">Réinitialisation mot de passe</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="mb-3">
                                                <asp:Label ID="lblMessage" runat="server" Text="" style="color: #FF3300; background-color: #FFFFFF"></asp:Label>
                                                <asp:TextBox ID="txtMotPasse" class="form-control form-control-lg" placeholder="Mot de passe" runat="server" required="required" TextMode="Password" />
                                            </div>
                                            <div class="mb-3">
                                                <asp:TextBox ID="txtConfirmerMotPasse" class="form-control form-control-lg" placeholder="Confirmer mot de passe" runat="server" required="required" TextMode="Password" />
                                            </div>
                                            <div class="text-center">
                                                <asp:Button ID="btnReinitialiser" class="btn btn-lg btn-primary btn-lg w-100 mt-4 mb-0" ToolTip="Réinitialiser mot de passe" runat="server" Text="Réinitialiser" OnClick="btnReinitialiser_Click" />
                                            </div>
                                        </div>
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
        </div>
    </form>
</body>
</html>
