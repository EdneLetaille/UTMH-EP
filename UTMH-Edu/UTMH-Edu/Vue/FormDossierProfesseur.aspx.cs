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

namespace UTMH_Edu.Vue
{
    public partial class FormDossierProfesseur : System.Web.UI.Page
    {
        Log log = new Log();
        private static readonly string strCon =
           ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurProfesseur Prof = new ControlleurProfesseur();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                ChargerCours();
                this.ChargerDepartements();
                this.ChargerVille();

                if (Request.QueryString["code"] == null)
                {
                    Response.Redirect("FormListeProfesseur.aspx");
                    return;
                }

                ChargerDossierProfesseur(Request.QueryString["code"]);


            }
        }
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }

        private void ChargerCours()
        {
            string query = @"SELECT c.idCours, 
                            c.nom AS nomCours, 
                            p.nom AS nomProfesseur
                     FROM cours c
                     INNER JOIN professeur p 
                     ON c.idProf = p.idProf";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlCours.DataSource = dr;
                ddlCours.DataTextField = "nomCours";
                ddlCours.DataValueField = "idCours";
                ddlCours.DataBind();
            }

            ddlCours.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Sélectionner un cours --", ""));
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
        // CHARGER DOSSIER PROFESSEUR
        // =========================
        private void ChargerDossierProfesseur(string codeProfesseur)
        {
            // On ajoute 'c.idCours' dans le SELECT et un LEFT JOIN sur la table cours
            string query = @"
        SELECT p.idProf, p.code, p.nom, p.prenom, p.idVille, p.departement, 
               p.email, p.statut, p.dateEmbauche, p.salaire, p.cin, p.role,
               c.idCours -- On récupère l'ID du cours lié
        FROM professeur p
        LEFT JOIN cours c ON p.idProf = c.idProf 
        WHERE p.code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@code", codeProfesseur);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();


                if (dr.Read())
                {
                    string idVille = dr["idVille"].ToString();
                    if (ddlVille.Items.FindByValue(idVille) != null)
                    {
                        ddlVille.SelectedValue = idVille;
                    }

                    txtCode.Text = dr["code"].ToString();
                    txtNom.Text = dr["nom"].ToString();
                    txtPrenom.Text = dr["prenom"].ToString();
                    ddlDepartement.SelectedValue = dr["departement"].ToString();
                    txtEmail.Text = dr["email"].ToString();
                    ddlStatut.SelectedValue = dr["statut"].ToString();
                    txtDateEmbauche.Text = dr["dateEmbauche"].ToString();
                    txtSalaire.Text = dr["salaire"].ToString();
                    txtCin.Text = dr["cin"].ToString();
                    txtRole.Text = dr["role"].ToString();
                    // Affectation de la DropDownList du Cours
                    string idCours = dr["idCours"].ToString();
                    if (!string.IsNullOrEmpty(idCours) && ddlCours.Items.FindByValue(idCours) != null)
                    {
                        ddlCours.SelectedValue = idCours;
                    }

                    Session["codeProfesseur"] = dr["code"].ToString();
                }
            }
        }


        //private void ChargerDossierProfesseur(string codeProfesseur)
        //{
        //    string query = @"
        //        SELECT idProf,
        //            code,
        //            nom,
        //            prenom,               
        //            idVille,
        //            departement,
        //            email,
        //            statut,
        //            dateEmbauche,
        //            salaire,
        //             cin,
        //            role
        //        FROM professeur
        //        WHERE code = @code";

        //    using (SqlConnection con = new SqlConnection(strCon))
        //    using (SqlCommand cmd = new SqlCommand(query, con))
        //    {
        //        cmd.Parameters.AddWithValue("@code", codeProfesseur);

        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();

        //        if (dr.Read())
        //        {
        //            string idVille = dr["idVille"].ToString();
        //            if (ddlVille.Items.FindByValue(idVille) != null)
        //            {
        //                ddlVille.SelectedValue = idVille;
        //            }
        //            string idCours = dr["idCours"].ToString();
        //            if (ddlCours.Items.FindByValue(idCours) != null)
        //            {
        //                ddlCours.SelectedValue = idCours;
        //            }
        //            txtCode.Text = dr["code"].ToString();
        //            txtNom.Text = dr["nom"].ToString();
        //            txtPrenom.Text = dr["prenom"].ToString();
        //            ddlDepartement.SelectedValue = dr["departement"].ToString();
        //            txtEmail.Text = dr["email"].ToString();
        //            ddlStatut.SelectedValue = dr["statut"].ToString();
        //            txtDateEmbauche.Text = dr["dateEmbauche"].ToString();
        //            txtSalaire.Text = dr["salaire"].ToString();
        //            txtCin.Text = dr["cin"].ToString();
        //            txtRole.Text = dr["role"].ToString();

        //            // Session
        //            Session["codeProfesseur"] = dr["code"].ToString();
        //        }
        //    }
        //}
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



        protected void btnModifier_Click(object sender, EventArgs e)
        {
            if (Session["codeProfesseur"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string nouveauMotDePasse = GenererMotDePasse();



            string query = @"
        UPDATE professeur SET
                    code = @code,
                    nom = @nom,
                    prenom = @prenom,
                    idVille = @idVille,
                    departement = @departement,
                    email = @email,
                    statut = @statut,
                    dateEmbauche = @dateEmbauche,
                    salaire = @salaire,
                    cin = @cin,
                    role = @role
        WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@nom", txtNom.Text);
                cmd.Parameters.AddWithValue("@prenom", txtPrenom.Text);
                cmd.Parameters.AddWithValue("@idVille", ddlVille.SelectedValue);
                cmd.Parameters.AddWithValue("@departement", ddlDepartement.SelectedValue);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@statut", ddlStatut.SelectedValue);
                cmd.Parameters.AddWithValue("@dateEmbauche", txtDateEmbauche.Text);
                cmd.Parameters.AddWithValue("@salaire", txtSalaire.Text);
                cmd.Parameters.AddWithValue("@cin", txtCin.Text);
                cmd.Parameters.AddWithValue("@role", txtRole.Text);
                cmd.Parameters.AddWithValue("@code", Session["codeProfesseur"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            log.AjouterLog(
                 Session["code"].ToString(),
                 "FormDossierProfesseur.aspx",
                 "Modification Professeur",
                 Session["role"].ToString()
             );
            ShowAlert("Professeur Modifier avec succes");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirectAfterModal",
                "setTimeout(function(){ window.location='FormListeProfesseur.aspx'; }, 2000);", true);
        }

        protected void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (Session["codeProfesseur"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string codeASupprimer = txtCode.Text.Trim();

            if (!string.IsNullOrEmpty(codeASupprimer))
            {
                // Appel à la méthode du modèle pour supprimer
                string resultat = Prof.supprimerProfesseur(codeASupprimer);

                // Supprimer la session
                Session.Remove("codeProfesseur");

                // Redirection vers la liste
                Response.Redirect("FormListeProfesseur.aspx?msg=" + resultat);
                log.AjouterLog(
                 Session["code"].ToString(),
                 "FormDossierProfesseur.aspx",
                 "Supression Professeur",
                 Session["role"].ToString()
             );
            }
            else
            {
                ShowAlert("Il n'y a pas de code Professeur sélectionné");
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormListeProfesseur.aspx");
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (Session["codeProfesseur"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            string nouveauMotDePasse = GenererMotDePasse();
            // Hasher le mot de passe avant insertion
            string hashedPassword = HashPassword(nouveauMotDePasse);

            string query = "UPDATE professeur SET motPasse = @mdp WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@mdp", hashedPassword);
                cmd.Parameters.AddWithValue("@code", Session["codeProfesseur"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            log.AjouterLog(
                 Session["code"].ToString(),
                 "FormDossierProfesseur.aspx",
                 "reinitialisation mode de passe professeur",
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

            ShowAlert("Mot de passe envoyé au professeur.");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            // Nom du fichier
            string filename = $"{txtCode.Text}_Professeur.pdf";

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
            Paragraph title = new Paragraph("\nDOSSIER DU PROFESSEUR\n\n", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);

            // ===== TABLE DES INFORMATIONS =====
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 30, 70 });

            AddRowToTable(table, "Code", txtCode.Text, labelFont, valueFont);
            AddRowToTable(table, "Prénom", txtPrenom.Text, labelFont, valueFont);
            AddRowToTable(table, "Nom", txtNom.Text, labelFont, valueFont);
            AddRowToTable(table, "Département", ddlDepartement.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Ville", ddlVille.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Email", txtEmail.Text, labelFont, valueFont);
            AddRowToTable(table, "Salaire", txtSalaire.Text, labelFont, valueFont);
            AddRowToTable(table, "Cours", ddlCours.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Statut", ddlStatut.SelectedItem.Text, labelFont, valueFont);
            AddRowToTable(table, "Date Embauche", txtDateEmbauche.Text, labelFont, valueFont);

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