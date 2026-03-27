<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormBadge.aspx.cs" Inherits="UTMH_Edu.Vue.FormBadge" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Badge</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
        <div class="row">
            <div class="col-12">
                <div class="card mb-4 ms-3">
                    <div class="card-header pb-0">
                        <h6>Génération badge étudiant</h6>
                    </div>

                    <div class="card-body px-0 pt-0 pb-2">
                        <!-- Champ recherche + bouton -->
                        <div class="row px-3 mb-3">

                            <div class="col-md-4">
                                <asp:TextBox
                                    ID="txtRecherche"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    OnTextChanged="btnRecherche_Click"
                                    Placeholder="Rechercher par nom, prénom ou code">
                                </asp:TextBox>
                            </div>

                            <div class="col-md-2">
                                <asp:LinkButton
                                    ID="btnReinitialiser"
                                    runat="server"
                                    CssClass="btn btn-secondary w-100"
                                    OnClick="btnReinitialiser_Click">
            <i class="fa fa-rotate-left"></i>
            Réinitialiser
                                </asp:LinkButton>
                            </div>

                        </div>

                        <div class="table-responsive p-0">
                            <asp:Label ID="Lb2" runat="server"></asp:Label>

                            <asp:GridView
                                ID="GridView1"
                                runat="server"
                                AutoGenerateColumns="False"
                                DataKeyNames="idEtudiant"
                                OnRowCommand="GridView1_RowCommand"
                                CssClass="table align-items-center justify-content-center mb-0"
                                HeaderStyle-CssClass="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 header-bold"
                                RowStyle-CssClass="text-sm">

                                <Columns>
                                    <asp:BoundField DataField="code" HeaderText="Code" HeaderStyle-ForeColor="Black" />
                                    <asp:BoundField DataField="nom" HeaderText="Nom" HeaderStyle-ForeColor="Black" />
                                    <asp:BoundField DataField="prenom" HeaderText="Prénom" HeaderStyle-ForeColor="Black" />
                                    <asp:BoundField DataField="cin" HeaderText="CIN" HeaderStyle-ForeColor="Black" />

                                    <asp:TemplateField HeaderText="Action" HeaderStyle-ForeColor="Black">
                                        <ItemTemplate>
                                            <asp:LinkButton
                                                ID="btnBadge"
                                                runat="server"
                                                CommandName="GenererBadge"
                                                CommandArgument="<%# Container.DataItemIndex %>"
                                                CssClass="btn btn-sm btn-primary">
                                                Générer Badge
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>

                    </div>
                </div>
            </div>
        </div>

     <div id="toastBox" style="position: fixed; top: 20px; right: 20px; z-index: 9999;"></div>

        <script>
    function showToast(type, message) {

        var toast = document.createElement("div");

        // styles de base
        toast.style.minWidth = "250px";
        toast.style.marginBottom = "10px";
        toast.style.padding = "15px 20px";
        toast.style.borderRadius = "8px";
        toast.style.color = "#fff";
        toast.style.fontWeight = "500";
        toast.style.boxShadow = "0 5px 15px rgba(0,0,0,0.2)";
        toast.style.opacity = "0";
        toast.style.transition = "all 0.5s";

        // couleurs selon type
        if (type === "success") {
            toast.style.background = "#28a745";
            message = "✅ " + message;
        } else if (type === "error") {
            toast.style.background = "#dc3545";
            message = "❌ " + message;
        } else {
            toast.style.background = "#ffc107";
            toast.style.color = "#000";
            message = "⚠️ " + message;
        }

        toast.innerHTML = message;

        document.getElementById("toastBox").appendChild(toast);

        // animation entrée
        setTimeout(() => {
            toast.style.opacity = "1";
            toast.style.transform = "translateX(-10px)";
        }, 100);

        // suppression après 3s
        setTimeout(() => {
            toast.style.opacity = "0";
            setTimeout(() => {
                toast.remove();
            }, 500);
        }, 3000);
    }
</script>
    </main>
</asp:Content>
