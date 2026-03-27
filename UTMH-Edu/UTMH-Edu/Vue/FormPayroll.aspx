<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master"
    AutoEventWireup="true" CodeBehind="FormPayroll.aspx.cs"
    Inherits="UTMH_Edu.Vue.FormPayroll" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Payroll</h6>
                </nav>

            </div>
        </nav>
        <!-- End Navbar -->
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <div class="card-header pb-0">
                    <div class="d-flex justify-content-between align-items-center">
                        <h6 class="mb-0">Payroll</h6>
                        <a class="btn btn-sm btn-outline-primary" href="FormSalaire.aspx">
                            <i class="fa fa-money-bill"></i>Gérer les salaires
                        </a>
                    </div>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <!-- ================= FILTRES ================= -->

                    <div class="row g-2 mb-3 align-items-end">

                        <div class="col-md-2">
                            <label class="form-label fw-bold">Mois</label>
                            <asp:DropDownList ID="ddlMois" runat="server" CssClass="form-select" />
                        </div>

                        <div class="col-md-2">
                            <label class="form-label fw-bold">Année</label>
                            <asp:DropDownList ID="ddlAnnee" runat="server" CssClass="form-select" />
                        </div>

                        <div class="col-md-3">
                            <label class="form-label fw-bold">Type</label>
                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                <asp:ListItem Value="0">Tous</asp:ListItem>
                                <asp:ListItem Value="PROF">Professeur</asp:ListItem>
                                <asp:ListItem Value="ADM">Administration</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-5 d-flex gap-2">
                            <asp:LinkButton ID="btnRechercher" runat="server"
                                CssClass="btn btn-primary w-100"
                                OnClick="btnRechercher_Click">
<i class="fa fa-search"></i> Rechercher
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnReset" runat="server"
                                CssClass="btn btn-secondary w-100"
                                OnClick="btnReset_Click">
<i class="fa fa-rotate-left"></i> Réinitialiser
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnEffectuerTous" runat="server"
                                CssClass="btn btn-success w-100"
                                OnClick="btnEffectuerTous_Click"
                                OnClientClick="return confirm('Confirmer l’exécution du payroll pour tous ?');">
<i class="fa fa-check"></i> Effectuer payroll pour tous
                            </asp:LinkButton>
                        </div>

                    </div>

                    <asp:Label ID="Lb2" runat="server" CssClass="text-danger fw-bold"></asp:Label>

                    <!-- ================= GRID ================= -->

                    <div class="table-responsive mt-3">

                        <asp:GridView ID="GridView1"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover"
                            DataKeyNames="typePersonne,idPersonne"
                            OnRowCommand="GridView1_RowCommand">

                            <Columns>

                                <asp:BoundField DataField="code" HeaderText="Code" />
                                <asp:BoundField DataField="nom" HeaderText="Nom" />
                                <asp:BoundField DataField="prenom" HeaderText="Prénom" />
                                <asp:BoundField DataField="roleLibelle" HeaderText="Rôle" />
                                <asp:BoundField DataField="salaire" HeaderText="Salaire"
                                    DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="datePayroll" HeaderText="Date payroll" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="datePaiement" HeaderText="Date paiement" DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnValider"
                                            runat="server"
                                            CssClass="btn btn-sm btn-primary"
                                            CommandName="OpenModal"
                                            CommandArgument="<%# Container.DataItemIndex %>">
Valider paiement
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                    </div>

                    <!-- ================= HIDDEN ================= -->

                    <asp:HiddenField ID="hfType" runat="server" />
                    <asp:HiddenField ID="hfIdPersonne" runat="server" />
                    <asp:HiddenField ID="hfSelectedPayrollIds" runat="server" />

                </div>
            </div>
        </div>
    </div>

    <!-- ================= MODAL PAIEMENT ================= -->

    <div class="modal fade" id="payModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Validation paiement</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <p><strong>Nom :</strong>
                        <asp:Label ID="lblNomModal" runat="server" /></p>
                    <p><strong>Rôle :</strong>
                        <asp:Label ID="lblRoleModal" runat="server" /></p>

                    <div class="d-flex gap-2 mb-2">
                        <button type="button" class="btn btn-sm btn-outline-primary"
                            onclick="selectAllPayroll(true)">
                            Select all</button>

                        <button type="button" class="btn btn-sm btn-outline-secondary"
                            onclick="selectAllPayroll(false)">
                            Unselect all</button>
                    </div>

                    <div class="border rounded p-2" style="max-height: 260px; overflow: auto;">

                        <asp:Repeater ID="rptPayrollMois" runat="server">
                            <ItemTemplate>

                                <div class="form-check d-flex justify-content-between align-items-center py-1">

                                    <div>
                                        <input class="form-check-input pay-cb"
                                            type="checkbox"
                                            value="<%# Eval("idPayroll") %>"
                                            data-amount="<%# Eval("montant") %>"
                                            <%# (Eval("statut").ToString().ToUpper()=="PAYE") 
? "disabled checked" : "" %>
                                            onchange="updatePayrollTotal()" />

                                        <label class="form-check-label ms-1">
                                            <%# Eval("libMois") %>
                                            <%# (Eval("statut").ToString().ToUpper()=="PAYE")
? "<span class='badge bg-success ms-2'>PAYE</span>"
: "<span class='badge bg-warning text-dark ms-2'>EN_ATTENTE</span>" %>
                                        </label>
                                    </div>

                                    <div class="fw-bold">
                                        <%# string.Format("{0:N0} HTG", Eval("montant")) %>
                                    </div>

                                </div>

                            </ItemTemplate>
                        </asp:Repeater>

                    </div>

                    <div class="d-flex justify-content-between mt-3">
                        <div class="fw-bold">Total sélectionné :</div>
                        <div class="fw-bold text-primary">
                            <span id="totalAmount">0</span> HTG
                        </div>
                    </div>

                    <asp:Label ID="lblModalErr" runat="server"
                        CssClass="text-danger fw-bold mt-2 d-block"></asp:Label>

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary"
                        data-bs-dismiss="modal">
                        Fermer</button>

                    <asp:LinkButton ID="btnConfirmerModal"
                        runat="server"
                        CssClass="btn btn-primary"
                        OnClick="btnConfirmerModal_Click">
Confirmer paiement
                    </asp:LinkButton>

                </div>

            </div>
        </div>
    </div>

    <!-- ================= MODAL MESSAGE ================= -->

    <div class="modal fade" id="msgModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content p-4 text-center">

                <h5 class="mb-3">Message</h5>
                <p id="msgText" class="fw-bold"></p>

                <div class="mt-3">
                    <button type="button" class="btn btn-primary"
                        data-bs-dismiss="modal">
                        OK</button>
                </div>

            </div>
        </div>
    </div>

    <!-- ================= SCRIPT ================= -->

    <script>

        function updatePayrollTotal() {

            let total = 0;
            let ids = [];

            document.querySelectorAll(".pay-cb").forEach(cb => {
                if (cb.checked && !cb.disabled) {
                    total += parseFloat(cb.dataset.amount);
                    ids.push(cb.value);
                }
            });

            document.getElementById("totalAmount").innerText =
            total.toLocaleString();

            document.getElementById('<%= hfSelectedPayrollIds.ClientID %>').value =
ids.join(",");

}

function selectAllPayroll(state) {
    document.querySelectorAll(".pay-cb").forEach(cb=> {
        if (!cb.disabled) {
            cb.checked = state;
        }
    });
    updatePayrollTotal();
}

function openPayModal() {
    new bootstrap.Modal(document.getElementById('payModal')).show();
}

    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>
