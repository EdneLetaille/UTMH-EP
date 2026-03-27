using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Vue
{
    public partial class FormMotDePasseOublie : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void btnEnvoyer_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblMessage.Text = "Veuillez entrer votre adresse e-mail.";
                return;
            }

            // 🔎 Trouver l'utilisateur (étudiant / prof / admin)
            int idUtilisateur;
            string typeUtilisateur;

            bool existe =
                UtilisateurService.TrouverParEmail(
                    email,
                    out idUtilisateur,
                    out typeUtilisateur);

            if (!existe)
            {
                lblMessage.Text =
                    "Si l'adresse existe, un e-mail sera envoyé.";
                return;
            }

            // 🔐 Générer le token
            ControlleurReinitialiserMotPasse ctrl =
                new ControlleurReinitialiserMotPasse();

            string token =
                ctrl.CreerToken(idUtilisateur, typeUtilisateur);

            // 📧 Construire le lien
            string lien =
                Request.Url.GetLeftPart(UriPartial.Authority)
                + "/Vue/FormReinitialiserMotPasse.aspx?token=" + token;
            string body = $@"
Cliquez sur le lien suivant pour réinitialiser votre mot de passe : 
<a href='{lien}'>Réinitialiser le mot de passe</a>";

            // 📤 Envoyer l'e-mail
            ServiceTechnique.SendEmail(
                email,
                "Réinitialisation du mot de passe",
                body);

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text =
                "Si l'adresse existe, un e-mail a été envoyé.";
        }
    }
}