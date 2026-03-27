<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormAccueilAdm.aspx.cs" Inherits="UTMH_Edu.Vue.FormAccueilAdm" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div>
            <!-- Navbar -->
            <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
                <div class="container-fluid py-1 px-3">
                    <nav aria-label="breadcrumb">

                        <h6 class="font-weight-bolder text-white mb-0">Accueil</h6>
                    </nav>

                </div>
            </nav>
            <!-- End Navbar -->
            <div class="container-fluid py-4">
                <div class="row mt-4">
                    <div class="col-lg-7 mb-lg-0 mb-4">
                        <div class="card z-index-2 h-100">
                            <div class="card-header pb-0 pt-3 bg-transparent">
                                <h6 class="text-capitalize">Statistiques générales</h6>
                                <p class="text-sm mb-0">
                                    <i class="fa fa-chart-bar text-primary"></i>
                                    Total Étudiants, Professeurs, Cours & Options
                                </p>
                            </div>
                            <div class="card-body p-3">
                                <div class="chart">
                                    <!-- 🔥 GRAPHE BATON -->
                                    <canvas id="chart-global" class="chart-canvas" height="300"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-5">
                        <div class="card card-carousel overflow-hidden h-100 p-0">
                            <div id="carouselExampleCaptions" class="carousel slide h-100" data-bs-ride="carousel">
                                <div class="carousel-inner border-radius-lg h-100">
                                    <div class="carousel-item h-100 active" style="background-image: url('../assets/img/carousel-1.png'); background-size: cover;">
                                        <div class="carousel-caption d-none d-md-block bottom-0 text-start start-0 ms-5">
                                            <div class="icon icon-shape icon-sm bg-white text-center border-radius-md mb-3">
                                                <i class="ni ni-camera-compact text-dark opacity-10"></i>
                                            </div>
                                           
                                        </div>
                                    </div>
                                    <div class="carousel-item h-100" style="background-image: url('../assets/img/carousel-2.png'); background-size: cover;">
                                        <div class="carousel-caption d-none d-md-block bottom-0 text-start start-0 ms-5">
                                            <div class="icon icon-shape icon-sm bg-white text-center border-radius-md mb-3">
                                                <i class="ni ni-bulb-61 text-dark opacity-10"></i>
                                            </div>
                                          
                                        </div>
                                    </div>
                                    <div class="carousel-item h-100" style="background-image: url('../assets/img/carousel-3.png'); background-size: cover;">
                                        <div class="carousel-caption d-none d-md-block bottom-0 text-start start-0 ms-5">
                                            <div class="icon icon-shape icon-sm bg-white text-center border-radius-md mb-3">
                                                <i class="ni ni-trophy text-dark opacity-10"></i>
                                            </div>
                                            
                                        </div>
                                    </div>
                                </div>
                                <button class="carousel-control-prev w-5 me-3" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="prev">
                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Previous</span>
                                </button>
                                <button class="carousel-control-next w-5 me-3" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="next">
                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Next</span>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-7 mb-lg-0 mb-4">
                        <div class="card z-index-2 h-100">
                            <div class="card-header pb-0 pt-3 bg-transparent">
                                <h6 class="text-capitalize">Étudiants par option</h6>
                                <p class="text-sm mb-0">
                                    <i class="fa fa-chart-pie text-primary"></i>
                                    Répartition par filière
                                </p>
                            </div>

                            <div class="card-body p-3">
                                <div class="chart">
                                    <canvas id="chart-options" class="chart-canvas" height="300"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="col-lg-5 mb-4">
                        <div class="card h-100">
                            <div class="card-header pb-0 bg-transparent">
                                <h6 class="text-capitalize">Étudiants – Statut</h6>
                                <p class="text-sm mb-0">Actifs / Inactifs / En attente</p>
                            </div>
                            <div class="chart">
                                <canvas id="chart-etudiants-statut" height="180"></canvas>
                            </div>

                        </div>
                    </div>

                </div>


                <div class="row mt-4">
                    <div class="col-lg-7 mb-lg-0 mb-4">
    <div class="card">
        <div class="card-header pb-0 p-3">
            <div class="d-flex justify-content-between">
                <h6 class="mb-2">Liste Etudiants</h6>
            </div>
        </div>

        <div class="table-responsive">

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="False"
                DataSourceID="SqlDataSource1"
                CssClass="table align-items-center mb-0"
                HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                RowStyle-CssClass="align-middle text-sm"
                AlternatingRowStyle-CssClass="align-middle text-sm bg-light">

                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" SortExpression="code" HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="nom" HeaderText="Nom" SortExpression="nom" HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="prenom" HeaderText="Prénom" SortExpression="prenom" HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="sexe" HeaderText="Sexe" SortExpression="sexe" HeaderStyle-ForeColor="Black" />
                    <asp:BoundField DataField="telephone" HeaderText="Téléphone" SortExpression="telephone" HeaderStyle-ForeColor="Black" />
                </Columns>

            </asp:GridView>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                ConnectionString="<%$ ConnectionStrings:dbConnect %>"
                SelectCommand="SELECT TOP 5 * FROM etudiant">
            </asp:SqlDataSource>

        </div>
    </div>
</div>

                    <div class="col-lg-5">
                        <div class="card">
                            <div class="card-header pb-0 p-3">
                                <h6 class="mb-0">Liste Professeur</h6>
                            </div>

                            <div class="card-body p-3 table-responsive">
                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource2"
                                    CssClass="table align-items-center mb-0"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                                    RowStyle-CssClass="align-middle text-sm"
                                    AlternatingRowStyle-CssClass="align-middle text-sm bg-light">

                                    <Columns>
                                        <asp:BoundField DataField="code" HeaderText="Code" SortExpression="code" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="nom" HeaderText="Nom" SortExpression="nom" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="prenom" HeaderText="Prénom" SortExpression="prenom" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="cin" HeaderText="Cin" SortExpression="cin" HeaderStyle-ForeColor="Black" />
                                    </Columns>
                                </asp:GridView>

                                <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:dbConnect %>"
                                    SelectCommand="SELECT TOP 5 * FROM [professeur]"></asp:SqlDataSource>
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
            <div class="modal fade" id="msgModal1" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-lg">
                    <div class="modal-content d-flex align-items-center p-4 border-0 shadow-lg rounded-3">

                        <!-- icône -->
                        <div class="me-4">
                            <svg id="msgIcon" xmlns="http://www.w3.org/2000/svg" width="50" height="50"
                                fill="currentColor" class="text-danger" viewBox="0 0 16 16">
                                <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm.93 4.58a.5.5 0 0 1 .07.7L8.5 7.5v3a.5.5 0 0 1-1 0V7.5l-.5-.22a.5.5 0 1 1 .46-.88l.04.03L8 6.25l.03-.02zM8 12a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                            </svg>
                        </div>

                        <!-- Texte -->
                        <div class="flex-grow-1 text-start">
                            <h5 class="mb-1 text-danger fw-bold">Erreur</h5>
                            <p id="msgText" class="mb-0 text-gray-700 fs-5">
                                Voulez-vous quitter l'application ?
                            </p>
                        </div>

                        <!-- Boutons -->
                        <div class="ms-4 d-flex gap-2">
                            <button type="button" class="btn btn-secondary" id="btnNon">
                                Non
                            </button>
                            <button type="button" class="btn btn-danger" id="btnOui">
                                Oui
                            </button>
                        </div>

                    </div>
                </div>
            </div>
            </div>

    </main>
    <script>
        document.addEventListener("DOMContentLoaded", function () {

            let modal = new bootstrap.Modal(
                document.getElementById("msgModal1"),
                { backdrop: 'static', keyboard: false }
            );

            // Vérifie si c'est la page d'accueil
            let isHome = true; // vrai seulement pour FormAccueilAdm

            if (isHome) {
                // Ajouter un état au début pour empêcher le back direct
                history.replaceState({ home: true }, '', location.href);

                window.addEventListener("popstate", function (event) {
                    // Si on revient vers la page d'accueil
                    if (event.state && event.state.home) {
                        modal.show();
                        history.pushState({ home: true }, '', location.href);
                    }
                });
            }

            // Boutons modal
            document.getElementById("btnOui").addEventListener("click", function () {
                window.location.href = "FormDeconnexion.aspx";
            });

            document.getElementById("btnNon").addEventListener("click", function () {
                modal.hide();
            });

        });
    </script>

    <script>
        var ctx = document.getElementById("chart-global").getContext("2d");

        new Chart(ctx, {
            type: "bar",
            data: {
                labels: ["Étudiants", "Professeurs", "Cours", "Options"],
                datasets: [{
                    label: "Totaux",
                    data: [
                        <%= totalEtudiants %>,
                    <%= totalProfesseurs %>,
                    <%= totalCours %>,
                    <%= totalOptions %>
                    ],
                    backgroundColor: [
                        "#5e72e4",
                        "#2dce89",
                        "#fb6340",
                        "#11cdef"
                    ],
                    borderRadius: 8
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            stepSize: 1
                        }
                    }
                }
            }
        });
    </script>
    <script>
        new Chart(document.getElementById("chart-options"), {
            type: "doughnut",
            data: {
                labels: <%= labelsOptions %>,
                datasets: [{
                    data: <%= dataOptions %>,
                    backgroundColor: [
                        "#5E72E4",
                        "#11CDEF",
                        "#2DCE89",
                        "#FB6340",
                        "#F5365C",
                        "#8965E0",
                        "#FFD600",
                        "#8898AA",
                        "#172B4D",
                        "#5603AD"
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: "65%",
                plugins: {
                    legend: {
                        position: "bottom",
                        labels: {
                            boxWidth: 12,
                            padding: 15
                        }
                    }
                }
            }
        });


    </script>

    <script>
        function afficherHeure() {
            const now = new Date();

            const jours = ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"];
            const mois = [
                "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
                "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
            ];

            const jour = jours[now.getDay()];
            const date = now.getDate();
            const moisNom = mois[now.getMonth()];
            const annee = now.getFullYear();

            const heure = String(now.getHours()).padStart(2, '0');
            const minute = String(now.getMinutes()).padStart(2, '0');
            const seconde = String(now.getSeconds()).padStart(2, '0');

            document.getElementById("heureActuelle").innerHTML =
                `${jour}, ${date} ${moisNom} ${annee} <br> ${heure}:${minute}:${seconde}`;
        }

        afficherHeure();
        setInterval(afficherHeure, 1000);
    </script>
    <script>
        const ctxStatut = document.getElementById("chart-etudiants-statut");

        new Chart(ctxStatut, {
            type: "doughnut",
            data: {
                labels: ["Actifs", "Inactifs", "En attente"],
                datasets: [{
                    data: [
                        <%= etuActifs %>,
                <%= etuInactifs %>,
                <%= etuEnAttente %>
                    ],
                    backgroundColor: [
                        "#2dce89", // vert
                        "#f5365c", // rouge
                        "#fb6340"  // orange
                    ],
                    borderWidth: 0
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: "70%",
                plugins: {
                    legend: {
                        position: "bottom",
                        labels: {
                            boxWidth: 12,
                            padding: 15
                        }
                    },
                    tooltip: {
                        enabled: true
                    }
                }
            }
        });
    </script>





</asp:Content>
