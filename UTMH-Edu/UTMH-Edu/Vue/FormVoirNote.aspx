<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/AccueilEtudiant.Master" AutoEventWireup="true" CodeBehind="FormVoirNote.aspx.cs" Inherits="UTMH_Edu.Vue.FormVoirNote" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <h6 class="font-weight-bolder text-white mb-0">Note</h6>
                </nav>
            </div>
        </nav>
    
    <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <div class="card-header pb-0">
                    <h6 class="mb-0">Toutes mes notes</h6>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                    <asp:Label ID="lbMsg" runat="server" CssClass="text-danger"></asp:Label>
                    
                    <div class="table-responsive">
                        <asp:GridView
                            ID="gvNotes"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover text-center align-middle"
                            GridLines="None"
                            EmptyDataText="Aucune note enregistrée.">

                            <Columns>

                                <asp:BoundField DataField="Cours"
                                    HeaderText="Nom du cours"
                                    DataFormatString="{0}" />

                                <asp:BoundField DataField="NoteObtenue"
                                    HeaderText="Note obtenue"
                                    DataFormatString="{0:0.00}" />

                                <asp:BoundField DataField="libelleNote"
                                    HeaderText="Libellé" />

                                <asp:BoundField DataField="TypeNote"
                                    HeaderText="Type" />

                            </Columns>

                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>


</asp:Content>