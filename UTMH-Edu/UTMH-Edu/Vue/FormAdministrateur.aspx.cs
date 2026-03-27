using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using System.Net.Mail;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using UTMH_Edu.Model;
using System.Security.Cryptography;
using System.Text;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Vue
{
    public partial class FormAdministrateur : System.Web.UI.Page
    {
        Log log = new Log();
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurAdministrateur Adm = new ControlleurAdministrateur();
        ControlleurEtudiant Etu = new ControlleurEtudiant();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime hier = DateTime.Today.AddDays(-1);
                txtDateNaissance.Attributes["max"] = hier.ToString("yyyy-MM-dd");
                txtDateEmbauche.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtStatut.Text = "Actif";
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormConnexion.aspx");
                    return;
                }
            }
           


        

        }


        public void codeEtudiant()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                return;
            }

            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();

            string codeNom = nom.Length >= 2 ? nom.Substring(0, 2).ToUpper() : nom.ToUpper();
            string codePrenom = prenom.Length >= 2 ? prenom.Substring(0, 2).ToUpper() : prenom.ToUpper();

            Random rnd = new Random();
            int randomNumber = rnd.Next(10, 99);

            txtCode.Text = codeNom + "-" + codePrenom + "-" + randomNumber;
        }

        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }


        public bool EmailValide(string email)
        {
            try
            {
                var adr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }

        private string GenererMotDePasse(int longueur = 8)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, longueur)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        void videChamps()
        {
            txtCode.Text = "";
            txtDateNaissance.Text = "";
            txtPrenom.Text = "";
            txtNom.Text = "";
            txtCin.Text = "";
            txtEmail.Text = "";
            txtTelephone.Text = "";
            ddlRole.SelectedIndex = -1;
            ddlSexe.SelectedIndex = -1;
        }
        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            this.codeEtudiant();

            if (!EmailValide(txtEmail.Text))
            {
                ShowAlert("Email non valide.");
                return;
            }

            if (Adm.emailExisteDansSysteme(txtEmail.Text))
            {
                ShowAlert("l'email existe deja dans le systeme.");
                return;
            }
            if (Adm.cinExisteDansSysteme(txtCin.Text))
            {
                ShowAlert("le cin existe deja dans le systeme.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
            string.IsNullOrWhiteSpace(txtNom.Text))
            {
                ShowAlert("Certains champs sont vides !");
                return;
            }
            else
            {
                // -----------------------------
                // Enregistrement en base
                // -----------------------------
                string nouveauMotDePasse = GenererMotDePasse();
                string hashedPassword = HashPassword(nouveauMotDePasse);
                Adm.enregistrerAdministrateur(txtCode.Text, txtNom.Text, txtPrenom.Text, txtEmail.Text, txtDateNaissance.Text,  ddlSexe.SelectedValue, txtTelephone.Text, hashedPassword, txtStatut.Text,txtCin.Text, ddlRole.SelectedValue, txtDateEmbauche.Text);

                ShowAlert("Inscription effectuée avec succès.");
                ServiceTechnique.SendEmail(
             txtEmail.Text.Trim(),
             "Envoie mot de passe - UTMH",
             $"Salut {txtPrenom.Text.Trim()} {txtNom.Text.Trim()},\n\n" +
             "Votre mot de passe a été envoyé.\n\n" +
             $"Mot de passe : {nouveauMotDePasse}\n\n" +
             "Veuillez le modifier après connexion.\n\n" +
             "Administration UTMH"
         );
                this.videChamps();
                log.AjouterLog(
                   Session["code"].ToString(),
                   "FormAdministrateur.aspx",
                   "Enregistrement Administrateur",
                   Session["role"].ToString()
               );
            }

        }
    }
}