using System;
using UTMH_Edu.Model;
using UTMH_Edu.Controlleur;
using System.Text;

namespace UTMH_Edu.Vue
{
    public partial class FormReinitialiserMotPasse : System.Web.UI.Page
    {
        ControlleurReinitialiserMotPasse ctrl = new ControlleurReinitialiserMotPasse();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                if (string.IsNullOrEmpty(token) || !ctrl.VerifierToken(token))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Token invalide ou expiré.";
                }
            }
        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"];
            string motPasse = txtMotPasse.Text;
            string confirmation = txtConfirmerMotPasse.Text;

            if (motPasse != confirmation)
            {
                lblMessage.Text = "Les mots de passe ne correspondent pas.";
                return;
            }

            bool success =
                ctrl.ReinitialiserMotDePasse(token, motPasse);

            if (success)
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Mot de passe réinitialisé avec succès.";
                Response.Redirect("FormConnexion.aspx");
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Erreur lors de la réinitialisation.";
            }
        }

        

    }
}
