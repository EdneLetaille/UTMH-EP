<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/AccueilEtudiant.Master" AutoEventWireup="true" CodeBehind="FormAccueilEtudiant.aspx.cs" EnableSessionState="True" Inherits="UTMH_Edu.Vue.FormAccueilEtudiant" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>
        <style>.calendar-card {
    border-radius: 16px;
}

.calendar-weekdays,
.calendar-days {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 8px;
}

.calendar-weekdays div {
    text-align: center;
    font-size: 13px;
    font-weight: 700;
    color: #67748e;
    padding: 8px 0;
}

.calendar-days div {
    height: 42px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 12px;
    font-size: 14px;
    font-weight: 600;
    background: #f8f9fa;
    color: #344767;
    transition: all 0.2s ease;
}

.calendar-days div:hover {
    background: #e9ecef;
}

.calendar-days .empty {
    background: transparent;
}

.calendar-days .today {
    background: linear-gradient(310deg, #5e72e4, #825ee4);
    color: #fff;
    box-shadow: 0 4px 12px rgba(94,114,228,0.35);
}

.calendar-days .selected-month {
    background: #eef2ff;
    color: #5e72e4;
}

@media (max-width: 768px) {
    .calendar-days div {
        height: 38px;
        font-size: 13px;
    }
}</style>
               <!-- Navbar -->
   
<%--<nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
    <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">

            <h6 class="font-weight-bolder text-white mb-0">Bienvenue  <strong><asp:Label ID="lbUser" runat="server" Text="..."></asp:Label></strong> sur la plateforme de l'UTMH</h6>
        </nav>

    </div>
</nav>--%>
    <!-- End Navbar -->  
      
   



        <!-- End Navbar -->
    <div class="container-fluid py-4">

      <div class="row mt-4">
 <div class="col-lg-7 mb-lg-0 mb-4">
  <div class="card z-index-2 h-100">
    <div class="card-header pb-0 pt-3 bg-transparent">
      <h6 class="text-capitalize">Progression Académique</h6>
      <p class="text-sm mb-0">
        <i class="fa fa-arrow-up text-success"></i>
        <span class="font-weight-bold">Activité d’apprentissage</span>
      </p>
    </div>
    <div class="card-body p-3">
      <div class="chart">
        <canvas id="chart-progress" class="chart-canvas" height="300"></canvas>
      </div>
    </div>
  </div>
</div>
      <div class="col-lg-5 mb-lg-0 mb-4">
    <div class="card h-100 calendar-card">
        <div class="card-header pb-0 pt-3 bg-transparent d-flex justify-content-between align-items-center">
            <div>
                <h6 class="text-capitalize mb-0">Calendrier</h6>
                <p class="text-sm mb-0 text-muted">Organisation académique</p>
            </div>

            <div class="d-flex align-items-center">
                <button type="button" class="btn btn-sm btn-outline-primary mb-0 me-2" id="prevMonth">
                    <i class="fas fa-chevron-left"></i>
                </button>
                <button type="button" class="btn btn-sm btn-outline-primary mb-0" id="nextMonth">
                    <i class="fas fa-chevron-right"></i>
                </button>
            </div>
        </div>

        <div class="card-body p-3">
            <div class="text-center mb-3">
                <h5 id="calendarMonth" class="mb-0 font-weight-bolder"></h5>
            </div>

            <div class="calendar-weekdays">
                <div>Dim</div>
                <div>Lun</div>
                <div>Mar</div>
                <div>Mer</div>
                <div>Jeu</div>
                <div>Ven</div>
                <div>Sam</div>
            </div>

            <div id="calendarDays" class="calendar-days"></div>
        </div>
    </div>
</div>
      </div>
      <div class="row mt-4">
        <div class="col-lg-7 mb-lg-0 mb-4">
          <div class="card ">
            <div class="card-header pb-0 p-3">
              <div class="d-flex justify-content-between">
                <h6 class="mb-2">Liste des notes</h6>
              </div>
            </div>
                    <!-- TABLE -->

               <div class="table-responsive">
                                <asp:GridView ID="GridView2" runat="server"
                                    AutoGenerateColumns="False"                                 
                                    CssClass="table align-items-center mb-0"
                                    HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7"
                                    RowStyle-CssClass="align-middle text-sm"
                                    AlternatingRowStyle-CssClass="align-middle text-sm bg-light">

                                    <Columns>
                                        
                                        <asp:BoundField DataField="code" HeaderText="Code" SortExpression="code" HeaderStyle-ForeColor="Black" />                  
                                        <asp:BoundField DataField="nomOption" HeaderText="Option" SortExpression="nomOption" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="nomCours" HeaderText="Nom Cours" SortExpression="nomCours" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="noteObtenue" HeaderText="Note" SortExpression="noteObtenue" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="typeNote" HeaderText="Type Note" SortExpression="typeNote" HeaderStyle-ForeColor="Black" />
                                        <asp:BoundField DataField="dateNote" HeaderText=" Date Note" SortExpression="dateNote"   DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-ForeColor="Black" />
                                    </Columns>

                                </asp:GridView>


                               
                            </div>
          </div>
        </div>
        <div class="col-lg-5">
<div class="card">
    <div class="card-header pb-0 p-3">
        <h6 class="mb-0">Informations utiles</h6>
    </div>
    <div class="card-body p-3">
        <ul class="list-group">

            <li class="list-group-item border-0 d-flex justify-content-between ps-0 mb-2 border-radius-lg">
                <div class="d-flex align-items-center">
                    <div class="icon icon-shape icon-sm me-3 bg-gradient-dark shadow text-center">
                        <i class="ni ni-badge text-white opacity-10"></i>
                    </div>
                    <div class="d-flex flex-column">
                        <h6 class="mb-1 text-dark text-sm">Cin</h6>
                        <span class="text-xs font-weight-bold">
                            <asp:Label ID="lbMatricule" runat="server" Text="..."></asp:Label>
                        </span>
                    </div>
                </div>
            </li>

            <li class="list-group-item border-0 d-flex justify-content-between ps-0 mb-2 border-radius-lg">
                <div class="d-flex align-items-center">
                    <div class="icon icon-shape icon-sm me-3 bg-gradient-dark shadow text-center">
                        <i class="ni ni-books text-white opacity-10"></i>
                    </div>
                    <div class="d-flex flex-column">
                        <h6 class="mb-1 text-dark text-sm">Option</h6>
                        <span class="text-xs font-weight-bold">
                            <asp:Label ID="lbOption" runat="server" Text="..."></asp:Label>
                        </span>
                    </div>
                </div>
            </li>

            <li class="list-group-item border-0 d-flex justify-content-between ps-0 mb-2 border-radius-lg">
                <div class="d-flex align-items-center">
                    <div class="icon icon-shape icon-sm me-3 bg-gradient-dark shadow text-center">
                        <i class="ni ni-calendar-grid-58 text-white opacity-10"></i>
                    </div>
                    <div class="d-flex flex-column">
                        <h6 class="mb-1 text-dark text-sm">Session active</h6>
                        <span class="text-xs font-weight-bold">
                            <asp:Label ID="lbSessionActive" runat="server" Text="..."></asp:Label>
                        </span>
                    </div>
                </div>
            </li>         

        </ul>
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
  <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

<script>
document.addEventListener("DOMContentLoaded", function () {

    fetch("FormAccueilEtudiant.aspx/GetProgressionData", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: "{}"
    })
    .then(response => response.json())
    .then(result => {
        const chartData = JSON.parse(result.d);

        if (!chartData.labels || chartData.labels.length === 0) {
            document.querySelector("#chart-progress").parentElement.innerHTML =
                "<p class='text-center text-muted'>Aucune donnée disponible</p>";
            return;
        }

        const ctx = document.getElementById("chart-progress").getContext("2d");

        const gradientBlue = ctx.createLinearGradient(0, 0, 0, 300);
        gradientBlue.addColorStop(0, "rgba(94,114,228,0.4)");
        gradientBlue.addColorStop(1, "rgba(94,114,228,0)");

        const gradientPink = ctx.createLinearGradient(0, 0, 0, 300);
        gradientPink.addColorStop(0, "rgba(255,99,132,0.4)");
        gradientPink.addColorStop(1, "rgba(255,99,132,0)");

        new Chart(ctx, {
            type: "line",
            data: {
                labels: chartData.labels,
                datasets: [
                    {
                        label: "Cours",
                        data: chartData.cours,
                        borderColor: "#5e72e4",
                        backgroundColor: gradientBlue,
                        tension: 0.4,
                        fill: true,
                        pointRadius: 4,
                        pointBackgroundColor: "#5e72e4"
                    },
                    {
                        label: "Examens",
                        data: chartData.examens,
                        borderColor: "#f5365c",
                        backgroundColor: gradientPink,
                        tension: 0.4,
                        fill: true,
                        pointRadius: 4,
                        pointBackgroundColor: "#f5365c"
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: "top",
                        align: "start"
                    }
                },
                scales: {
                    x: {
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        min: 0,
                        max: 100,
                        ticks: {
                            stepSize: 20
                        }
                    }
                }
            }
        });
    })
    .catch(error => {
        console.error("Erreur graphique :", error);
    });
});
</script>

        <script>
document.addEventListener("DOMContentLoaded", function () {

    const monthLabel = document.getElementById("calendarMonth");
    const daysContainer = document.getElementById("calendarDays");
    const prevBtn = document.getElementById("prevMonth");
    const nextBtn = document.getElementById("nextMonth");

    if (!monthLabel || !daysContainer || !prevBtn || !nextBtn) return;

    let currentDate = new Date();

    function renderCalendar(date) {
        const year = date.getFullYear();
        const month = date.getMonth();

        const firstDay = new Date(year, month, 1).getDay();
        const lastDate = new Date(year, month + 1, 0).getDate();

        const today = new Date();
        const isCurrentMonth =
            today.getFullYear() === year && today.getMonth() === month;

        const monthNames = [
            "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
            "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
        ];

        monthLabel.textContent = monthNames[month] + " " + year;
        daysContainer.innerHTML = "";

        for (let i = 0; i < firstDay; i++) {
            const emptyCell = document.createElement("div");
            emptyCell.className = "empty";
            daysContainer.appendChild(emptyCell);
        }

        for (let day = 1; day <= lastDate; day++) {
            const dayCell = document.createElement("div");
            dayCell.textContent = day;

            if (isCurrentMonth && day === today.getDate()) {
                dayCell.classList.add("today");
            } else {
                dayCell.classList.add("selected-month");
            }

            daysContainer.appendChild(dayCell);
        }
    }

    prevBtn.addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() - 1);
        renderCalendar(currentDate);
    });

    nextBtn.addEventListener("click", function () {
        currentDate.setMonth(currentDate.getMonth() + 1);
        renderCalendar(currentDate);
    });

    renderCalendar(currentDate);
});
</script>
    </main>


</asp:Content>
