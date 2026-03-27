<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="RapportListeAdministrateur.aspx.cs" Inherits="UTMH_Edu.Vue.RapportListeAdministrateur" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- 🔹 STYLE -->
    <style>
        main {
            padding: 20px;
        }

               /* Navbar moderne */
        .custom-navbar {
            background: linear-gradient(90deg, #667eea, #764ba2);
            border-radius: 12px;
            padding: 15px;
            margin-bottom: 20px;
        }

        .page-title {
            font-size: 22px;
            font-weight: bold;
            color: white;
        }

        /* Card container */
        .report-card {
            margin-top: 20px;
            border-radius: 15px;
            padding: 20px;
            background: #ffffff;
            box-shadow: 0px 4px 15px rgba(0, 0, 0, 0.1);
        }

        /* ReportViewer */
        #ReportViewer1 {
            width: 100% !important;
            border-radius: 10px;
            overflow: hidden;
        }

        /* Titre interne */
        .card-title {
            font-weight: bold;
            margin-bottom: 15px;
            color: #333;
        }
    </style>

    <main>

        <!-- 🔹 NAVBAR -->
        <nav class="navbar custom-navbar">
            <div class="container-fluid">
                <h6 class="page-title">📋 Liste des menmbres</h6>
            </div>
        </nav>

        <!-- 🔹 CONTENU -->
        <div class="report-card">

            <h5 class="card-title">Liste complète des membres</h5>

            <!-- ScriptManager obligatoire -->
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <!-- ReportViewer -->
            <rsweb:ReportViewer 
                ID="ReportViewer1" 
                runat="server"
                Width="100%" 
                Height="600px"
                Font-Names="Segoe UI"
                Font-Size="10pt">

                <LocalReport ReportPath="Vue\RapportListeAdministrateur.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource 
                            DataSourceId="SqlDataSource1" 
                            Name="DataSet1" />
                    </DataSources>
                </LocalReport>

            </rsweb:ReportViewer>

        </div>

        <!-- 🔹 DATASOURCE -->
        <asp:SqlDataSource 
            ID="SqlDataSource1" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:dbConnect %>" 
            SelectCommand="SELECT [code], [nom], [prenom], [email], [dateNaissance], [sexe], [telephone], [cin], [statut], [role], [dateEmbauche] FROM [administrateur]">
        </asp:SqlDataSource>

    </main>

</asp:Content>