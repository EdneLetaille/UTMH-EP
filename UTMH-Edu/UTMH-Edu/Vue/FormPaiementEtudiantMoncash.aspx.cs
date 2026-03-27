using System;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Vue
{
    public partial class FormPaiementEtudiantMoncash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {           

                lblMerchant.Text = "UTMH";

          
                decimal amount = 1500;
              

                lblTotal.Text = amount.ToString("0");
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                lblErreur.Text = "";

                string numero = (txtPhone.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(numero))
                {
                    lblErreur.Text = "Numéro de téléphone obligatoire";
                    return;
                }

                if (numero.Length != 8)
                {
                    lblErreur.Text = "Le numéro doit contenir 8 chiffres";
                    return;
                }

                // ✅ Générer OTP
                string otp = new Random().Next(100000, 999999).ToString();

                // ✅ Stocker session
                Session["MC_NUM"] = numero;
                Session["MC_OTP"] = otp;
                Session["MC_OTP_EXPIRES"] = DateTime.Now.AddMinutes(5);

                // ✅ Envoyer OTP par email
                string email = Session["INS_email"]?.ToString();
                if (string.IsNullOrWhiteSpace(email))
                {
                    lblErreur.Text = "Email introuvable. Recommencez l'inscription.";
                    return;
                }

                ServiceTechnique.SendEmail(
                    email,
                    "Code de vérification (OTP) - UTMH",
                    $"Bonjour {Session["INS_prenom"]} {Session["INS_nom"]},\n\n" +
                    $"Voici votre code de vérification : {otp}\n\n" +
                    "Ce code est valable 5 minutes.\n\n" +
                    "Cordialement,\nUTMH"
                );

                // ✅ Rediriger
                Response.Redirect("MonCashSecurityCode.aspx");
            }
            catch (Exception ex)
            {
                lblErreur.Text = ex.Message;
            }
        }


    }
}
