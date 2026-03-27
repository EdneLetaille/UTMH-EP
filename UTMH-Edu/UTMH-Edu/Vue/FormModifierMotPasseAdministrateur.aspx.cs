using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormModifierMotPasseAdministrateur : System.Web.UI.Page
    {
        Log log = new Log();
        ControlleurAdministrateur Adm = new ControlleurAdministrateur();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("FormDeconnexion.aspx");
                return;
            }
        }

        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }

        protected void btnModifierMotPasse_Click(object sender, EventArgs e)
        {
            // Vérifier si la session existe
            if (Session["userId"] == null)
            {
                ShowAlert("Session expirée. Connectez-vous à nouveau.");
                return;
            }

            int idAdm = Convert.ToInt32(Session["userId"]);

            string ancienMdp = txtAncienPasse.Text.Trim();
            string nouveauMdp = txtNouveauPasse.Text.Trim();
            string confirmerMdp = txtConfPasse.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(ancienMdp))
            {
                ShowAlert("L'ancien mot de passe ne doit pas être vide.");
                return;
            }
            if (string.IsNullOrEmpty(nouveauMdp))
            {
                ShowAlert("Le nouveau mot de passe ne doit pas être vide.");
                return;
            }
            if (nouveauMdp != confirmerMdp)
            {
                ShowAlert("Les mots de passe ne correspondent pas.");
                return;
            }
            if (ancienMdp == nouveauMdp)
            {
                ShowAlert("Le nouveau mot de passe doit être différent de l'ancien.");
                return;
            }

            try
            {
                // VÉRIFIER L'ANCIEN MOT DE PASSE
                if (!Adm.verifierMotPasseAdministrateur(idAdm, ancienMdp))
                {
                    ShowAlert("L'ancien mot de passe est incorrect.");
                    return;
                }

                Adm.modifierMotPasseAdministrateur(idAdm, nouveauMdp);
                ShowAlert("Mot de passe modifié avec succès.");
                log.AjouterLog(
                 Session["code"].ToString(),
                 "FormModifierMotPasseAdministrateur.aspx",
                 "Modification mot de passe Administrateur",
                 Session["role"].ToString()
             );

                // Vider les champs
                txtAncienPasse.Text = "";
                txtNouveauPasse.Text = "";
                txtConfPasse.Text = "";
            }
            catch (Exception)
            {
                ShowAlert("Impossible de faire la modification dans la base");
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormProfileAdministrateur.aspx");
        }
    }
}