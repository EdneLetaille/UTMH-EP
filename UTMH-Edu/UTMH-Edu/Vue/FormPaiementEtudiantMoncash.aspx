<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormPaiementEtudiantMoncash.aspx.cs" Inherits="UTMH_Edu.Vue.FormPaiementEtudiantMoncash" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>MonCash - Button</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap (optional) -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />

    <style>
        body{
            background:#e9eef5;
            min-height:100vh;
            display:flex;
            align-items:center;
            justify-content:center;
            font-family:system-ui, -apple-system, Segoe UI, Roboto, Arial;
        }
        .mc-card{
            width: 360px;
            background:#fff;
            border-radius:6px;
            overflow:hidden;
            box-shadow:0 8px 30px rgba(0,0,0,.12);
            border:1px solid rgba(0,0,0,.08);
        }
        .mc-header{
            background: linear-gradient(#e24633, #c42519);
            color:#fff;
            padding:10px 12px;
            font-weight:700;
            font-size:14px;
        }
        .mc-body{
            padding:14px 14px 16px 14px;
        }
        .mc-merchant{
            font-size:13px;
            font-weight:600;
            color:#444;
            margin-bottom:2px;
        }
        .mc-total{
            color:#18a957;
            font-weight:800;
            font-size:14px;
            margin-bottom:10px;
        }
        .mc-input{
            border:1px solid #d9dee7;
            border-radius:4px;
            overflow:hidden;
            display:flex;
            background:#fff;
            margin-bottom:10px;
        }
        .mc-prefix{
            background:#f3f5f9;
            padding:10px 10px;
            border-right:1px solid #d9dee7;
            font-weight:700;
            color:#667085;
            min-width:46px;
            text-align:center;
        }
        .mc-input input{
            border:none;
            outline:none;
            padding:10px 10px;
            flex:1;
            font-size:14px;
        }
        .mc-hint{
            font-size:12px;
            color:#667085;
            margin-top:-4px;
            margin-bottom:12px;
        }
        .mc-pay{
            width:100%;
            border:none;
            border-radius:4px;
            padding:10px 12px;
            color:#fff;
            font-weight:800;
            background: linear-gradient(#e24633, #c42519);
            box-shadow:0 4px 14px rgba(196,37,25,.25);
        }
        .mc-pay:hover{ filter:brightness(.98); }
        .mc-error{
            color:#b42318;
            font-size:13px;
            margin-bottom:10px;
        }
        .mc-footer-note{
            font-size:12px;
            color:#667085;
            margin-top:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="mc-card">

            <div class="mc-header">
                Secure Payment
            </div>

            <div class="mc-body">
                <div class="d-flex justify-content-between align-items-start">
                    <div>
                        <div class="mc-merchant">
                            <asp:Label ID="lblMerchant" runat="server"></asp:Label>
                        </div>
                        <div class="mc-total">
                            Total: <asp:Label ID="lblTotal" runat="server"></asp:Label> HTG
                        </div>
                    </div>
                    <div>
                        <img alt="MonCash" src="../assets/img/moncash.png"
                             style="width:52px;height:auto;opacity:.95" />
                    </div>
                </div>

                <asp:Label ID="lblErreur" runat="server" CssClass="mc-error" />

                <!-- Nimewo MonCash -->
                <div class="mc-input">
                    <div class="mc-prefix">509</div>
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="8" placeholder="votre numero de telephone"
                        inputmode="numeric" CssClass="" />
                </div>
                 <div class="mc-input">
                    <div class="mc-prefix">🔑</div>
                    <asp:TextBox ID="txtPin" runat="server" MaxLength="4" placeholder="votre code secret"
                       
                        TextMode="Password" CssClass="" />
                </div>
               <div class="mc-hint">
    Entrez les 8 chiffres (sans l’indicatif 509). Exemple : 34xxxxxx
</div>

                <!-- Pa mande PIN sou sit ou -->
               

                <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="mc-pay mt-2" OnClick="btnPay_Click" />            
            </div>

        </div>
    </form>

    <script>
        // ti netwayaj: pa kite moun tape lèt nan nimewo
        document.addEventListener("input", function (e) {
            if (e.target && e.target.id && e.target.id.indexOf("txtPhone") >= 0) {
                e.target.value = e.target.value.replace(/\D/g, "").slice(0, 8);
            }
        });
    </script>
</body>
</html>
