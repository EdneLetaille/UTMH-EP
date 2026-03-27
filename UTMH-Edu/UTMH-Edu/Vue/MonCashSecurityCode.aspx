<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonCashSecurityCode.aspx.cs" Inherits="UTMH_Edu.Vue.MonCashSecurityCode" %>

<!DOCTYPE html>
<html lang="fr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Secure Payment - MonCash</title>

    <!-- Bootstrap 5 (CDN) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background: #dfe6ef;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            font-family: Arial, sans-serif;
        }

        .mc-card {
            width: 340px;
            border-radius: 4px;
            overflow: hidden;
            box-shadow: 0 6px 18px rgba(0,0,0,.15);
            background: #fff;
        }

        .mc-header {
            background: linear-gradient(#ff4b2b, #d62d20);
            padding: 10px 14px;
            color: #fff;
            font-weight: 700;
            font-size: 14px;
        }

        .mc-body {
            padding: 16px;
        }

        .mc-logo {
            height: 34px;
            width: auto;
        }

        .mc-row {
            display:flex;
            align-items:center;
            justify-content:space-between;
            margin-bottom: 12px;
        }

        .mc-label {
            font-size: 12px;
            color: #6b7280;
            margin-bottom: 6px;
        }

        .mc-input {
            border: 1px solid #d1d5db;
            border-radius: 4px;
            height: 40px;
            padding: 8px 10px;
            width: 100%;
        }

        .mc-resend {
            font-size: 12px;
            text-decoration: underline;
            color: #0d6efd;
            cursor: pointer;
        }

        .mc-btn {
            width: 100%;
            height: 42px;
            border: 0;
            border-radius: 4px;
            background: #d50000;
            color: #fff;
            font-weight: 700;
        }

        .mc-btn:hover {
            background: #b80000;
        }

        .mc-msg {
            font-size: 12px;
            margin-top: 10px;
        }

        .mc-divider {
            height: 1px;
            background: #e5e7eb;
            margin: 10px 0 14px 0;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="mc-card">
            <div class="mc-header">Secure Payment</div>

            <div class="mc-body">

                <div class="mc-row">
                    <div></div>
                    <!-- Tu peux remplacer l'image si tu as un logo local -->
                  <img alt="MonCash" src="../assets/img/moncash.png"
                             style="width:52px;height:auto;opacity:.95" />
                </div>

                <div class="mc-divider"></div>

                <div class="mb-2">
                    <div class="mc-label">Security Code</div>
                    <asp:TextBox
                        ID="txtOtp"
                        runat="server"
                        CssClass="mc-input"
                        MaxLength="6"
                        TextMode="SingleLine"
                        placeholder="Entrez le code à 6 chiffres" />
                </div>

                <div class="d-flex justify-content-start mb-3">
                    <asp:LinkButton ID="lnkResend" runat="server" CssClass="mc-resend" OnClick="lnkResend_Click" CausesValidation="false">
                        Resend code
                    </asp:LinkButton>
                </div>

                <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="mc-btn" OnClick="btnSend_Click" />

                <asp:Label ID="lblMsg" runat="server" CssClass="mc-msg d-block"></asp:Label>

                <!-- (Dev only) Afficher OTP si tu veux tester vite. Mets Visible=false en prod -->
                <asp:Label ID="lblDevOtp" runat="server" CssClass="mc-msg text-muted d-block mt-2" Visible="false"></asp:Label>

            </div>
        </div>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
