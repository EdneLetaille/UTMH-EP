using iTextSharp.text;
using iTextSharp.text.pdf;
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
using System.Security.Cryptography;
using System.Net.Mail;
using System.IO;

namespace UTMH_Edu.Vue
{
    public partial class FormDossierEtudiant : System.Web.UI.Page
    {
        Log log = new Log();
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
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
                this.ChargerOptions();
              this.ChargerDepartements();
                this.RemplirAnneeAcademique();
                this.ChargerVille();
                string role = Session["role"] as string;
                if (string.IsNullOrEmpty(role) || role != "Responsable")
                {
                    btnSupprimer.Visible = false;
                }


                if (Request.QueryString["code"] == null)
                {
                    Response.Redirect("FormListeEtudiant.aspx");
                    return;
                }

                ChargerDossierEtudiant(Request.QueryString["code"]);


                DateTime hier = DateTime.Today.AddDays(-1);
                txtDateNaissance.Attributes["max"] = hier.ToString("yyyy-MM-dd");

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

        private void ChargerDepartements()
        {
            ddlDepartement.Items.Clear();
            ddlDepartement.Items.Add(new System.Web.UI.WebControls.ListItem("-- Sélectionner département --", ""));


            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT idDepartement, nom FROM departement", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlDepartement.Items.Add(
                        new System.Web.UI.WebControls.ListItem(dr["nom"].ToString(), dr["idDepartement"].ToString()));
                }
            }
        }

        protected void ddlDepartement_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVille.Items.Clear();
            ddlVille.Items.Add(new System.Web.UI.WebControls.ListItem("-- Sélectionner ville --", ""));

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
                        new System.Web.UI.WebControls.ListItem(dr["nom"].ToString(), dr["idVille"].ToString()));
                }
            }
        }

        // =========================
        // CHARGER LES villes
        // =========================
        private void ChargerVille()
        {
            string query = "SELECT idVille, nom FROM ville";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlVille.DataSource = dr;
                ddlVille.DataTextField = "nom";   // affiché
                ddlVille.DataValueField = "idVille";   // stocké
                ddlVille.DataBind();
            }
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
                new System.Web.UI.WebControls.ListItem(anneeAcademique, anneeAcademique)
            );

            ddlAnneeAcademique.SelectedIndex = 0;
        }
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }
        // =========================
        // CHARGER LES OPTIONS
        // =========================
        private void ChargerOptions()
        {
            string query = "SELECT idOption, nom FROM optionChoisie";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlOption.DataSource = dr;
                ddlOption.DataTextField = "nom";   // affiché
                ddlOption.DataValueField = "idOption";   // stocké
                ddlOption.DataBind();
            }

            ddlOption.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Sélectionner une option --", ""));
        }

      

    // =========================
    // CHARGER DOSSIER ETUDIANT
    // =========================
    private void ChargerDossierEtudiant(string codeEtudiant)
        {

            string query = @"
                SELECT 
                    idOption,
                    code,
                    nom,
                    prenom,
                    dateNaissance,
                    sexe,
                    idVille,
                    departement,
                    adresse,
                    telephone,
                    email,
                    statut,
                    personneRes,
                    telephoneRes,
                    dateInscription,
                    cin,
                    statutPaiement,
                    anneeAcademique,
                    photo,
                    role
                FROM etudiant
                WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@code", codeEtudiant);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

               
                if (dr.Read())
                {
                    // ================= IMAGE =================
                    if (dr["photo"] != DBNull.Value)
                    {
                        string base64 = dr["photo"].ToString();

                        string imageUrl = "data:image/jpeg;base64," + base64;

                        imgPreview.ImageUrl = imageUrl;

                        hfPhotoBase64.Value = imageUrl;
                    }
                    string idOption = dr["idOption"].ToString();
                    if (ddlOption.Items.FindByValue(idOption) != null)
                    {
                        ddlOption.SelectedValue = idOption;
                    }
                    string idVille = dr["idVille"].ToString();
                    if (ddlVille.Items.FindByValue(idVille) != null)
                    {
                        ddlVille.SelectedValue = idVille;
                    }

                    txtCode.Text = dr["code"].ToString();
                    txtNom.Text = dr["nom"].ToString();
                    txtPrenom.Text = dr["prenom"].ToString();
                    txtDateNaissance.Text = (dr["dateNaissance"]).ToString();

                    ddlSexe.SelectedValue = dr["sexe"].ToString();
                   
                    ddlDepartement.SelectedValue = dr["departement"].ToString();
                    txtAdresse.Text = dr["adresse"].ToString();
                    txtTelephone.Text = dr["telephone"].ToString();
                    txtEmail.Text = dr["email"].ToString();

                    ddlStatut.SelectedValue = dr["statut"].ToString();
                    txtPerResponsable.Text = dr["personneRes"].ToString();
                    txtTelephoneRes.Text = dr["telephoneRes"].ToString();

                    txtDateInscription.Text = dr["dateInscription"].ToString();

                    txtCin.Text = dr["cin"].ToString();
                    ddlStatutPaiement.SelectedValue = dr["statutPaiement"].ToString();
                    string annee = dr["anneeAcademique"].ToString();
                    if (ddlAnneeAcademique.Items.FindByValue(annee) != null)
                    {
                        ddlAnneeAcademique.SelectedValue = annee;
                    }
           
                    txtRole.Text = dr["role"].ToString();

                    // Session
                    Session["codeEtudiant"] = dr["code"].ToString();
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
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }
        // =========================
        // MODIFIER DOSSIER
        // =========================
        protected void btnModifier_Click(object sender, EventArgs e)
        {
            if (Session["codeEtudiant"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            if (!EmailValide(txtEmail.Text))
            {
                ShowAlert("Email non valide.");
                return;
            }

            string photoPath = null;

            // 📂 Upload fichier
            if (fuPhoto.HasFile)
            {
                string folder = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = "etu_" + Session["codeEtudiant"] + Path.GetExtension(fuPhoto.FileName);
                string fullPath = Path.Combine(folder, fileName);

                fuPhoto.SaveAs(fullPath);
                photoPath = "Uploads/" + fileName;
            }

            // 📸 Caméra (uniquement si vraie image base64)
            else if (!string.IsNullOrEmpty(hfPhotoBase64.Value) && hfPhotoBase64.Value.StartsWith("data:image"))
            {
                string base64 = hfPhotoBase64.Value.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64);

                string folder = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = "etu_" + Session["codeEtudiant"] + ".jpg";
                string fullPath = Path.Combine(folder, fileName);

                File.WriteAllBytes(fullPath, bytes);
                photoPath = "Uploads/" + fileName;
            
            }


            string nouveauMotDePasse = GenererMotDePasse();



            string query = @"
        UPDATE etudiant SET
            idOption = @idOption,
            nom = @nom,
            prenom = @prenom,
            dateNaissance = @dateNaissance,
            sexe = @sexe,
            idVille = @idVille,
            departement = @departement,
            adresse = @adresse,
            telephone = @telephone,
            email = @email,
            statut = @statut,
            cin = @cin,
            statutPaiement = @statutPaiement,
            anneeAcademique = @anneeAcademique,
            photo = ISNULL(@photo, photo) ,
            role = @role
        WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idOption", ddlOption.SelectedValue);
                cmd.Parameters.AddWithValue("@nom", txtNom.Text);
                cmd.Parameters.AddWithValue("@prenom", txtPrenom.Text);
                cmd.Parameters.AddWithValue("@dateNaissance", txtDateNaissance.Text);
                cmd.Parameters.AddWithValue("@sexe", ddlSexe.SelectedValue);
                cmd.Parameters.AddWithValue("@idVille", ddlVille.SelectedValue);
                cmd.Parameters.AddWithValue("@departement", ddlDepartement.SelectedValue);
                cmd.Parameters.AddWithValue("@adresse", txtAdresse.Text);
                cmd.Parameters.AddWithValue("@telephone", txtTelephone.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@cin", txtCin.Text);
                cmd.Parameters.AddWithValue("@statut", ddlStatut.SelectedValue);
                cmd.Parameters.AddWithValue("@statutPaiement", ddlStatutPaiement.SelectedValue);
                cmd.Parameters.AddWithValue("@anneeAcademique", ddlAnneeAcademique.SelectedValue);
                cmd.Parameters.AddWithValue("@photo", (object)photoPath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@role", txtRole.Text);
                cmd.Parameters.AddWithValue("@code", Session["codeEtudiant"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            log.AjouterLog(
                  Session["code"].ToString(),
                  "FormDossierEtudiant.aspx",
                  "Modification Etudiant",
                  Session["role"].ToString()
              );
            ShowAlert("Etudiant Modifier avec succes");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirectAfterModal",
                "setTimeout(function(){ window.location='FormListeEtudiant.aspx'; }, 2000);", true);
        }




        // =========================
        // SUPPRIMER ETUDIANT
        // =========================
        protected void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (Session["codeEtudiant"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string codeASupprimer = txtCode.Text.Trim();
            if (!string.IsNullOrEmpty(codeASupprimer))
            {
                string resultat = Etu.supprimerEtudiant(codeASupprimer);

                // Supprimer la session
                Session.Remove("codeEtudiant");

                // Redirection vers la liste
                Response.Redirect("FormListeEtudiant.aspx?msg=" + resultat);
            }
            else
            {
                ShowAlert("Il n'y a pas de code Etudiant sélectionné");
            }
        }

        // =========================
        // ANNULER
        // =========================
        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormListeEtudiant.aspx");
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (Session["codeEtudiant"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string nouveauMotDePasse = GenererMotDePasse();
            // Hasher le mot de passe avant insertion
            string hashedPassword = HashPassword(nouveauMotDePasse);

            string query = "UPDATE etudiant SET motPasse = @mdp WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@mdp", hashedPassword);
                cmd.Parameters.AddWithValue("@code", Session["codeEtudiant"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            log.AjouterLog(
                  Session["code"].ToString(),
                  "FormDossierEtudiant.aspx",
                  "Modification mot de passe Etudiant",
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
                "Administration UTMH"
            );

            ShowAlert("Mot de passe envoyé à l'étudiant.");
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            // Nom du fichier
            string filename = $"{txtCode.Text}etudiant.pdf";

            // Préparer la réponse HTTP
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename={filename}");
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            // Création du document avec marge supérieure plus grande pour l'en-tête
            Document doc = new Document(PageSize.A4, 40, 40, 100, 40);
            PdfWriter.GetInstance(doc, Response.OutputStream);
            doc.Open();

            // ===== FONTS =====
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
            Font valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);

            // ===== ENTÊTE (CENTRÉ) =====
            PdfPTable headerTable = new PdfPTable(1); // une seule colonne
            headerTable.WidthPercentage = 100;

            // Logo
            string logoPath = Server.MapPath("../SiteUtilisateur/img/logo1.png");
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
            logo.ScaleToFit(80f, 80f);
            logo.Alignment = Element.ALIGN_CENTER;

            PdfPCell logoCell = new PdfPCell(logo);
            logoCell.Border = PdfPCell.NO_BORDER;
            logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.AddCell(logoCell);

            // Nom de l'école
            PdfPCell nameCell = new PdfPCell(new Phrase(
                "UNION DES TECHNICIENS MODERNES D’HAÏTI  UTMH  ÉCOLE PROFESSIONNELLE",
                titleFont
            ));
            nameCell.Border = PdfPCell.NO_BORDER;
            nameCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(nameCell);

            // Adresse
            PdfPCell addressCell = new PdfPCell(new Phrase(
                "#40, Delmas 95, Jacquet Tybull Rue Pomeyrac Prolongée impasse Louis Jeanty #40 au local de l’INSTITUTION MIXTE LOUIS JEANTY",
                valueFont
            ));
            addressCell.Border = PdfPCell.NO_BORDER;
            addressCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(addressCell);

            // Téléphone
            PdfPCell phoneCell = new PdfPCell(new Phrase(
                "Téléphone : +509 4473-9494",
                valueFont
            ));
            phoneCell.Border = PdfPCell.NO_BORDER;
            phoneCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerTable.AddCell(phoneCell);

            doc.Add(headerTable);

            // ===== TITRE =====
            Paragraph title = new Paragraph("\nDOSSIER DE L'ÉLÈVE\n\n", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);

            // ===== TABLE DES INFORMATIONS =====
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 30, 70 });

            AddRowToTable(table, "Code", txtCode.Text, labelFont, valueFont);
            AddRowToTable(table, "Prénom", txtPrenom.Text, labelFont, valueFont);
            AddRowToTable(table, "Nom", txtNom.Text, labelFont, valueFont);
            AddRowToTable(table, "Date de naissance", txtDateNaissance.Text, labelFont, valueFont);
            AddRowToTable(table, "Sexe", ddlSexe.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Département", ddlDepartement.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Ville", ddlVille.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "adresse", txtAdresse.Text, labelFont, valueFont);
            AddRowToTable(table, "Email", txtEmail.Text, labelFont, valueFont);
            AddRowToTable(table, "Téléphone", txtTelephone.Text, labelFont, valueFont);
            AddRowToTable(table, "Cin", txtCin.Text, labelFont, valueFont);
            AddRowToTable(table, "Option Choisie", ddlOption.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Année Academique", ddlAnneeAcademique.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Statut", ddlStatut.SelectedItem.Text, labelFont, valueFont);

            doc.Add(table);

            // ===== FIN =====
            doc.Close();
            Response.End();
        }
        // ===== MÉTHODE HELPER =====
        private void AddRowToTable(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
        {
            PdfPCell cell1 = new PdfPCell(new Phrase(label, labelFont));
            cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell1.Padding = 6;

            PdfPCell cell2 = new PdfPCell(new Phrase(value, valueFont));
            cell2.Padding = 6;

            table.AddCell(cell1);
            table.AddCell(cell2);
        }







    }
}
