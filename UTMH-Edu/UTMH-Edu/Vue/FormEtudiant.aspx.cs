using UTMH_Edu.Model;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System;
using UTMH_Edu.Controlleur;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Net.Mail;
using UTMH_Edu.ServiceTech;
using System.Linq;
using System.Security.Cryptography;

namespace UTMH_Edu.Vue
{
    public partial class FormEtudiant : System.Web.UI.Page
    {
        Log log = new Log();
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurEtudiant Etu = new ControlleurEtudiant();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null )
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                DateTime hier = DateTime.Today.AddDays(-1);
                txtDateNaissance.Attributes["max"] = hier.ToString("yyyy-MM-dd");
                this.RemplirAnneeAcademique();
                this.ChargerDepartements();
                this.listeNomOption();
                txtDateInscription.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
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

        void RemplirAnneeAcademique()
        {
            ddlAnneeAcademique.Items.Clear();

            int annee = DateTime.Now.Year;
            int mois = DateTime.Now.Month;

            int debut;
            int fin;

            // Si on est avant septembre
            if (mois < 9)
            {
                debut = annee - 1;
                fin = annee;
            }
            else
            {
                debut = annee;
                fin = annee + 1;
            }

            string anneeAcademique = debut + "/" + fin;

            ddlAnneeAcademique.Items.Add(
                new ListItem(anneeAcademique, anneeAcademique)
            );

            ddlAnneeAcademique.SelectedIndex = 0;
        }

        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }


        public void listeNomOption()
        {
            string query = "SELECT * FROM optionChoisie";
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
            }

            ddlOption.Items.Clear();
            ddlOption.Items.Add(new ListItem("-- Sélectionnez --", "0"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlOption.Items.Add(new ListItem(
                    row["nom"].ToString(),
                    row["idOption"].ToString()
                ));
            }
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

        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            this.codeEtudiant();

            if (!EmailValide(txtEmail.Text))
            {
                ShowAlert("Email non valide.");
                return;
            }
            string prenom = txtPrenom.Text;
            if (!Regex.IsMatch(prenom, @"^[A-Za-zÀ-ÿ]+([ _-][A-Za-zÀ-ÿ]+)*$"))
            {
                ShowAlert("Format de prénom invalide.");
                return;
            }
            string nom = txtNom.Text;
            if (!Regex.IsMatch(nom, @"^[A-Za-zÀ-ÿ]+([ _-][A-Za-zÀ-ÿ]+)*$"))
            {
                ShowAlert("Format de nom invalide.");
                return;
            }

            string nomPersonnel = txtPerResponsable.Text;
            if (!Regex.IsMatch(nomPersonnel, @"^[A-Za-zÀ-ÿ]+([ _-][A-Za-zÀ-ÿ]+)*$"))
            {
                ShowAlert("Format de nom du personnel invalide.");
                return;
            }

            DateTime dateNaissance;

            if (!DateTime.TryParse(txtDateNaissance.Text, out dateNaissance))
            {
                ShowAlert("Date de naissance invalide.");
                return;
            }

            int age = DateTime.Today.Year - dateNaissance.Year;
            if (dateNaissance > DateTime.Today.AddYears(-age)) age--;

            if (age < 12)
            {
                ShowAlert("L'étudiant doit avoir au moins 12 ans.");
                return;
            }
            if (Etu.emailExisteDansSysteme(txtEmail.Text))
            {
                ShowAlert("l'email existe deja dans le systeme.");
                return;
            }

            //if (Etu.matriculeExisteDansSysteme(txtMatricule.Text))
            //{
            //    ShowAlert("la matricule existe deja dans le systeme.");
            //    return;
            //}
            //if (!Regex.IsMatch(txtCin.Text, @"^\d{3}-\d{3}-\d{3}-\d{1}$"))
            //{
            //    ShowAlert("Cin invalide");
            //    return;
            //}
     
            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                string.IsNullOrWhiteSpace(txtNom.Text))
            {
                ShowAlert("Certains champs sont vides !");
                return;
            }

            if (ddlOption.SelectedValue == "0")
            {
                ShowAlert("Veuillez sélectionner une option valide !");
                return;
            }

            int idOption = int.Parse(ddlOption.SelectedValue);
            int idVille = int.Parse(ddlVille.SelectedValue);






            // Enregistrer la photo
            string path = SaveEtudiantPhoto();

            string motDePasse = GenererMotDePasse();
            // Hasher le mot de passe avant insertion
            string hashedPassword = HashPassword(motDePasse);

            // -----------------------------
            // Enregistrement en base
            // -----------------------------
            txtRole.Text = "Etudiant";
            ddlStatut.Text = "Actif";

            Etu.inscrireEtudiant(
                idOption,
                txtCode.Text,
                txtNom.Text,
                txtPrenom.Text,
                txtDateNaissance.Text,
                ddlSexe.Text,
                idVille,
                ddlDepartement.Text,
                txtAdresse.Text,
                txtTelephone.Text,
                txtEmail.Text,
                ddlStatut.Text,
                txtPerResponsable.Text,
                txtTelephoneRes.Text,
                txtDateInscription.Text,
                path,
                txtCin.Text,
                ddlStatutPaiement.Text,
                ddlAnneeAcademique.Text,               
                ddlModePaiement.Text,
                hashedPassword,
                txtRole.Text
            );

            // Envoi email
            ServiceTechnique.SendEmail(
                txtEmail.Text.Trim(),
                "Envoie mot de passe - UTMH",
                $"Salut {txtPrenom.Text.Trim()} {txtNom.Text.Trim()},\n\n" +
                "Votre mot de passe a été envoyé.\n\n" +
                $"Mot de passe : {motDePasse}\n\n" +
                "Veuillez le modifier après connexion.\n\n" +
                "Administration UTMH"
            );
            log.AjouterLog(
                 Session["code"].ToString(),
                 "FormEtudiant.aspx",
                 "Enregistrement Etudiant",
                 Session["role"].ToString()
             );
            this.viderChamps();
            ShowAlert("Inscription effectuée avec succès.");
        }

        // ============================================
        // ✅ Enregistrer photo (Upload ou Caméra)
        // ============================================
        private string SaveEtudiantPhoto()
        {
            try
            {
                string folder = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                // 1) Photo caméra (base64)
                string b64 = (hfPhotoBase64.Value ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(b64) && b64.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                {
                    string[] parts = b64.Split(',');
                    if (parts.Length == 2)
                    {
                        byte[] bytes = Convert.FromBase64String(parts[1]);
                        string fileName = "etu_" + Guid.NewGuid().ToString("N") + ".jpg";
                        string path = Path.Combine(folder, fileName);

                        File.WriteAllBytes(path, bytes);

                        if (File.Exists(path))
                            return "~/Uploads/" + fileName;
                    }
                }

                // 2) Photo upload (PC)
                if (fuPhoto != null && fuPhoto.HasFile)
                {
                    string ext = Path.GetExtension(fuPhoto.FileName).ToLowerInvariant();
                    if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                        return "";

                    string fileName = "etu_" + Guid.NewGuid().ToString("N") + ext;
                    string path = Path.Combine(folder, fileName);

                    fuPhoto.SaveAs(path);

                    if (File.Exists(path))
                        return "~/Uploads/" + fileName;
                }
            }
            catch
            {
                // On retourne vide si erreur, pour ne pas bloquer l'enregistrement
                return "";
            }

            return "";
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
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }
        void viderChamps()
        {
            txtCode.Text = "";
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtDateNaissance.Text = "";
            txtDateInscription.Text = "";
            ddlDepartement.Text = "";
            txtAdresse.Text = "";
            ddlVille.Text = "";
            txtTelephone.Text = "";
            txtTelephoneRes.Text = "";
            txtPerResponsable.Text = "";
            txtCin.Text = "";
            txtEmail.Text = "";
            ddlStatutPaiement.Text = "";
            ddlModePaiement.Text = "";
            ddlSexe.Text = "";
            ddlOption.SelectedIndex = -1;

        }



        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.viderChamps();
        }
    }
}
