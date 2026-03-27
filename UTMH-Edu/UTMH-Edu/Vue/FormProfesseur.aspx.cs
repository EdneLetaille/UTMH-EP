using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.Model;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Vue
{
    public partial class FormProfesseur : System.Web.UI.Page
    {
        Log log = new Log();
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurProfesseur Prof = new ControlleurProfesseur();
        ControlleurEtudiant Etu = new ControlleurEtudiant();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }

                this.ChargerDepartements();
                txtDateEmbauche.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
 

        }

        public void codeProfesseur()
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

        private void ChargerDepartements()
        {
            ddlDepartement.Items.Clear();
            ddlDepartement.Items.Add(new ListItem("-- Sélectionner département --", ""));


            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT idDepartement, nom FROM departement", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlDepartement.Items.Add(
                        new ListItem(dr["nom"].ToString(), dr["idDepartement"].ToString()));
                }
            }
        }

        protected void ddlDepartement_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVille.Items.Clear();
            ddlVille.Items.Add(new ListItem("-- Sélectionner ville --", ""));

            if (string.IsNullOrEmpty(ddlDepartement.SelectedValue))
                return;



            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT idVille, nom
            FROM ville
            WHERE idDepartement = @idDepartement
        ", con);

                cmd.Parameters.AddWithValue("@idDepartement", ddlDepartement.SelectedValue);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlVille.Items.Add(
                        new ListItem(dr["nom"].ToString(), dr["idVille"].ToString()));
                }
            }
        }

        public bool EmailValide(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var adr = new MailAddress(email.Trim());
                string domaine = adr.Host;

                return domaine.Contains("."); // ex: gmail.com obligatoire
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
            txtNom.Text = "";
            txtPrenom.Text = "";
            ddlVille.SelectedIndex = -1;
            ddlDepartement.SelectedIndex = -1;
            txtEmail.Text = "";
            txtSalaire.Text = "";
            txtCin.Text = "";
        }

        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            this.codeProfesseur();

            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                    string.IsNullOrWhiteSpace(txtNom.Text))
            {
                ShowAlert("Certains champs sont vides !");
                return;
            }

            if (!EmailValide(txtEmail.Text))
            {
                ShowAlert("Email non valide.");
                return;
            }

            if (Etu.emailExisteDansSysteme(txtEmail.Text))
            {
                ShowAlert("l'email existe deja dans le systeme.");
                return;
            }


          
            // -----------------------------
            // GESTION CV
            // -----------------------------
            string cv = null;

            if (!fuCv.HasFile)
            {
                ShowAlert("Veuillez ajouter votre CV !");
                return;
            }

            string extFiche = Path.GetExtension(fuCv.FileName).ToLower();
            if (extFiche != ".pdf")
            {
                ShowAlert("Format de fichier non autorisé pour le CV.");
                return;
            }

            string dossierFiche = Server.MapPath("~/Uploads/CV/");
            if (!Directory.Exists(dossierFiche))
                Directory.CreateDirectory(dossierFiche);

            string nomFiche = txtCode.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extFiche;
            string cheminFiche = Path.Combine(dossierFiche, nomFiche);

            fuCv.SaveAs(cheminFiche);
            cv = "~/Uploads/CV/" + nomFiche;


            // -----------------------------
            // GESTION PHOTO PROFIL
            // -----------------------------
            string photoPath = null;
          

            string extProfil = Path.GetExtension(fuPhotoProfil.FileName).ToLower();
            if (extProfil != ".jpg" && extProfil != ".jpeg" && extProfil != ".png")
            {
               
            }

            string dossierProfil = Server.MapPath("~/Uploads/PhotoProfilProf/");
            if (!Directory.Exists(dossierProfil))
                Directory.CreateDirectory(dossierProfil);

            string nomProfil = txtCode.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extProfil;
            string cheminProfil = Path.Combine(dossierProfil, nomProfil);

            fuPhotoProfil.SaveAs(cheminProfil);
            photoPath = "~/Uploads/PhotoProfilProf/" + nomProfil;

           
            // -----------------------------
            // Enregistrement en base
            // -----------------------------
            string statut = "Actif";
            string nouveauMotDePasse = GenererMotDePasse();
            // Hasher le mot de passe avant insertion

            string hashedPassword = HashPassword(nouveauMotDePasse);
            txtRole.Text = "Professeur";
            int idVille = int.Parse(ddlVille.SelectedValue);
            Prof.enregistrerProfesseur(
                txtCode.Text,
                txtNom.Text,
                txtPrenom.Text,
                  idVille,
                ddlDepartement.Text,        
                txtEmail.Text,
                hashedPassword,
                cv, statut,
 txtDateEmbauche.Text, txtSalaire.Text,txtCin.Text, txtRole.Text);
            log.AjouterLog(
                 Session["code"].ToString(),
                 "FormProfesseur.aspx",
                 "Enregistrement Professeur",
                 Session["role"].ToString()
             );
          
            ServiceTechnique.SendEmail(
              txtEmail.Text.Trim(),
              "Envoie mot de passe - UTMH",
              $"Salut {txtPrenom.Text.Trim()} {txtNom.Text.Trim()},\n\n" +
              "Votre mot de passe a été envoyé.\n\n" +
              $"Mot de passe : {nouveauMotDePasse}\n\n" +
              "Veuillez le modifier après connexion.\n\n" +
              "Administration UTMH"
          );
            ShowAlert("Enregistrement effectuée avec succès.");
            this.videChamps();



        }
    }
}