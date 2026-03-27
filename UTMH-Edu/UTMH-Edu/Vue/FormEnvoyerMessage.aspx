<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormEnvoyerMessage.aspx.cs" Inherits="UTMH_Edu.Vue.FormEnvoyerMessage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">
                <div class="card-header pb-0">
                    <h6 class="mb-0">Envoyer un message</h6>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <asp:Label ID="Lb2" runat="server" CssClass="fw-bold"></asp:Label>

                    <div class="row g-3 mt-2">

                        <div class="col-md-4">
                            <label class="form-label fw-bold">Type de destinataire</label>
                            <asp:DropDownList ID="ddlTypeDestinataire" runat="server" CssClass="form-select">
                                <asp:ListItem Value="TOUS">Tout le monde</asp:ListItem>
                                <asp:ListItem Value="ADM">Administration</asp:ListItem>
                                <asp:ListItem Value="PROF">Professeur</asp:ListItem>
                                <asp:ListItem Value="ETUD">Étudiant</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-8">
                            <label class="form-label fw-bold">Objet</label>
                            <asp:TextBox ID="txtObjet" runat="server" CssClass="form-control" MaxLength="150"
                                placeholder="Ex: Réunion, Note importante, Information..." />
                        </div>

                        <div class="col-12">
                            <label class="form-label fw-bold">Message</label>
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine"
                                Rows="7" placeholder="Écrivez votre message ici..." />
                        </div>

                        <div class="col-12 d-flex gap-2 mt-2">
                            <asp:LinkButton ID="btnEnvoyer" runat="server"
                                CssClass="btn btn-primary"
                                OnClick="btnEnvoyer_Click"
                                OnClientClick="return confirm('Confirmer l’envoi du message ?');">
                                <i class="fa fa-paper-plane"></i> Envoyer
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnAnnuler" runat="server"
                                CssClass="btn btn-secondary"
                                OnClick="btnAnnuler_Click">
                                <i class="fa fa-times"></i> Annuler
                            </asp:LinkButton>
                        </div>

                        <div class="col-12 mt-2">
                            <small class="text-muted">
                                Remarque : le système enverra l’email à tous les destinataires correspondant au type sélectionné.
                            </small>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>