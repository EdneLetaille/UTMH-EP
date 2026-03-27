<%@ Page Title="" Language="C#" MasterPageFile="~/Vue/Accueil.Master" AutoEventWireup="true" CodeBehind="FormMessagesSupport.aspx.cs" Inherits="UTMH_Edu.Vue.FormMessagesSupport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
         <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl " id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">

                    <h6 class="font-weight-bolder text-white mb-0">Liste des messages des utilisateurs</h6>
                </nav>

            </div>
        </nav>
   <div class="row">
        <div class="col-12">
            <div class="card mb-4 ms-3">

                <!-- TITRE -->
                <div class="card-header pb-0">
                    <h6>Messages</h6>
                </div>

                <div class="card-body px-3 pt-3 pb-2">

                   


                    <!-- TABLE -->
                    <div class="table-responsive">
                        <asp:GridView
                            ID="GridView1"
                            runat="server"
                            AutoGenerateColumns="False"
                            CssClass="table table-bordered table-hover"
                             DataSourceID="SqlDataSource1">

                            <Columns>
                                <asp:BoundField DataField="nomComplet" HeaderText="nomComplet" SortExpression="nomComplet" />
                                <asp:BoundField DataField="telephone" HeaderText="telephone" SortExpression="telephone" />
                                <asp:BoundField DataField="email" HeaderText="email" SortExpression="email" />
                                <asp:BoundField DataField="message" HeaderText="message" SortExpression="message" />                            

                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbConnect %>" SelectCommand="SELECT [nomComplet], [telephone], [email], [message] FROM [contact]"></asp:SqlDataSource>
                    </div>

                </div>
            </div>
        </div>
    </div>
    </main>
</asp:Content>
