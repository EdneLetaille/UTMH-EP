using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.ServiceTech;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormDossierAdministrateur : System.Web.UI.Page
    {
        Log log = new Log();
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurAdministrateur Adm = new ControlleurAdministrateur();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime hier = DateTime.Today.AddDays(-1);
                txtDateNaissance.Attributes["max"] = hier.ToString("yyyy-MM-dd");
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }

                if (Request.QueryString["code"] == null)
                {
                    Response.Redirect("FormListeAdministrateur.aspx");
                    return;
                }

                ChargerDossierAdministrateur(Request.QueryString["code"]);



            }
        }
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }

        // CHARGER DOSSIER ADMINISTRATEUR
        // =========================
        private void ChargerDossierAdministrateur(string codeAdm)
        {
            string query = @"
                SELECT 
                    code,
                    nom,
                    prenom,
                    email,
                    dateNaissance,
                    telephone,
                    cin,
                    statut,                  
                    role,
                    dateEmbauche
                FROM administrateur
                WHERE code = @code";
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@code", codeAdm);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {


                    txtCode.Text = dr["code"].ToString();
                    txtNom.Text = dr["nom"].ToString();
                    txtPrenom.Text = dr["prenom"].ToString();
                    txtEmail.Text = dr["email"].ToString();
                    txtDateNaissance.Text = (dr["dateNaissance"]).ToString();
                    txtTelephone.Text = dr["telephone"].ToString();
                    ddlStatut.SelectedValue = dr["statut"].ToString();
                    txtCin.Text = dr["cin"].ToString();
                    ddlRole.SelectedValue = dr["role"].ToString();
                    txtDateEmbauche.Text = dr["dateEmbauche"].ToString();


                    // Session
                    Session["codeAdm"] = dr["code"].ToString();
                }
            }
        }
        private string GenererMotDePasse(int longueur = 8)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, longueur)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        private string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // =========================
        // MODIFIER DOSSIER
        // =========================

        protected void btnModifier_Click(object sender, EventArgs e)
        {
            if (Session["codeAdm"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string nouveauMotDePasse = GenererMotDePasse();



            string query = @"
        UPDATE administrateur SET
                    nom = @nom,
                    prenom = @prenom,
                    email = @email,
                    dateNaissance = @dateNaissance,
                    telephone = @telephone,
                    statut = @statut,
                    cin=@cin,
                    role = @role                  
        WHERE code = @code";
            using (SqlConnection con = new SqlConnection(strCon))

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@nom", txtNom.Text);
                cmd.Parameters.AddWithValue("@prenom", txtPrenom.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@dateNaissance", txtDateNaissance.Text);
                cmd.Parameters.AddWithValue("@telephone", txtTelephone.Text);
                cmd.Parameters.AddWithValue("@statut", ddlStatut.SelectedValue);
                cmd.Parameters.AddWithValue("@role", ddlRole.SelectedValue);
                cmd.Parameters.AddWithValue("@cin", txtCin.Text);
                cmd.Parameters.AddWithValue("@code", Session["codeAdm"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            ShowAlert("administrateur modifié avec succes");
            log.AjouterLog(
                   Session["code"].ToString(),
                   "FormDossierAdministrateur.aspx",
                   "Modification Administrateur",
                   Session["role"].ToString()
               );
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirectAfterModal",
                "setTimeout(function(){ window.location='FormListeAdministrateur.aspx'; }, 3000);", true);
        }
        // =========================
        // SUPPRIMER ADMINISTRATEUR
        // =========================
        protected void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (Session["codeAdm"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string codeASupprimer = txtCode.Text.Trim();

            if (!string.IsNullOrEmpty(codeASupprimer))
            {
                // Appel à la méthode du modèle pour supprimer
                string resultat = Adm.supprimerAdministrateur(codeASupprimer);

                // Supprimer la session
                Session.Remove("codeAdm");

                // Redirection vers la liste
                Response.Redirect("FormListeAdministrateur.aspx?msg=" + resultat);
                log.AjouterLog(
                   Session["code"].ToString(),
                   "FormDossierAdministrateur.aspx",
                   "Supression Administrateur",
                   Session["role"].ToString()
               );
            }
            else
            {
                ShowAlert("Il n'y a pas de code Administrateur sélectionné");
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormListeAdministrateur.aspx");
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (Session["codeAdm"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string nouveauMotDePasse = GenererMotDePasse();
            // Hasher le mot de passe avant insertion
            string hashedPassword = HashPassword(nouveauMotDePasse);

            string query = "UPDATE administrateur SET motPasse = @mdp WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@mdp", hashedPassword);
                cmd.Parameters.AddWithValue("@code", Session["codeAdm"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            log.AjouterLog(
                   Session["code"].ToString(),
                   "FormDossierAdministrateur.aspx",
                   "Reinitialiser mot de passe",
                   Session["role"].ToString()
               );
            // Envoi email
            ServiceTechnique.SendEmail(
                txtEmail.Text.Trim(),
                "Envoie mot de passe - UTMH",
                $"Salut {txtPrenom.Text.Trim()} {txtNom.Text.Trim()},\n\n" +
                "Votre mot de passe a été envoyé.\n\n" +
                $"Mot de passe : {nouveauMotDePasse}\n\n" +
                "Veuillez le modifier après connexion.\n\n" +
                "Voici le lien de la connexion: https://unwaved-jennefer-frazzledly.ngrok-free.dev/Vue/FormConnexion.aspx \n\n" +
                "Administration UTMH"
            );

            ShowAlert("Mot de passe envoyé à l'administrateur.");
        }
    }
}