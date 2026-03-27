using UTMH_Edu.Model;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System;
using UTMH_Edu.Controlleur;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace UTMH_Edu.Vue
{
    public partial class FormProfileEtudiant : System.Web.UI.Page
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
                listeNomOption();
                chargerInformationEtudiant();
            }
        }

        private void chargerInformationEtudiant()
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("FormConnexion.aspx");
                return;
            }

            // Session values
            string userId = Session["userId"].ToString();
            string nom = Session["nom"]?.ToString() ?? "";
            string prenom = Session["prenom"]?.ToString() ?? "";
            string email = Session["email"]?.ToString() ?? "";
            string code = Session["code"]?.ToString() ?? "";
            string sexe = Session["sexe"]?.ToString() ?? "";
            string telephone = Session["telephone"]?.ToString() ?? "";
            string option = Session["idOption"]?.ToString() ?? "";
            string date = Session["dateInscription"]?.ToString() ?? "";
            string annee = Session["anneeAcademique"]?.ToString() ?? "";         

            //// Header labels
            //lblNomComplet.Text = (prenom + " " + nom).Trim();
          

            // Champs
            txtNom.Text = nom;
            txtPrenom.Text = prenom;
            txtEmail.Text = email;
            txtCode.Text = code;
            txtTelephone.Text = telephone;
        

            // Sexe (lecture seule)
            ddlSexe.Items.Clear();
            if (!string.IsNullOrEmpty(sexe))
            {
                ddlSexe.Items.Add(new ListItem(sexe, sexe));
                ddlSexe.SelectedValue = sexe;
            }
            else
            {
                ddlSexe.Items.Add(new ListItem("--", ""));
            }

            // Option
            if (!string.IsNullOrEmpty(option) && ddlOption.Items.FindByValue(option) != null)
                ddlOption.SelectedValue = option;

            // Année académique (lecture seule)
            ddlAnneeAcademique.Items.Clear();
            if (!string.IsNullOrEmpty(annee))
            {
                ddlAnneeAcademique.Items.Add(new ListItem(annee, annee));
                ddlAnneeAcademique.SelectedValue = annee;
            }
            else
            {
                ddlAnneeAcademique.Items.Add(new ListItem("-", ""));
            }

          

            // Photo depuis DB
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT photo FROM etudiant WHERE idEtudiant = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    var photo = cmd.ExecuteScalar()?.ToString();

                    imgEtudiant.ImageUrl = !string.IsNullOrEmpty(photo)
                        ? "~/uploads/" + photo
                        : "~/uploads/default-user.png";
                }
            }
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
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    con.Open();
                    da.Fill(ds);
                }
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

        protected void btnAutoUpload_Click(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("FormConnexion.aspx");
                return;
            }

            string userId = Session["userId"].ToString();

            if (!fuPhotoProfil.HasFile)
            {
                ShowAlert("Veuillez sélectionner une image !");
                return;
            }

            // Taille max 2MB
            int maxBytes = 2 * 1024 * 1024;
            if (fuPhotoProfil.PostedFile.ContentLength > maxBytes)
            {
                ShowAlert("Image trop grande (max 2MB).");
                return;
            }

            string extension = Path.GetExtension(fuPhotoProfil.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                ShowAlert("Format image invalide (jpg, jpeg, png uniquement).");
                return;
            }

            string contentType = fuPhotoProfil.PostedFile.ContentType.ToLower();
            if (!(contentType.Contains("jpeg") || contentType.Contains("jpg") || contentType.Contains("png")))
            {
                ShowAlert("Type de fichier non supporté.");
                return;
            }

            // Ancienne photo (pour supprimer après)
            string oldPhoto = null;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT photo FROM etudiant WHERE idEtudiant = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    oldPhoto = cmd.ExecuteScalar()?.ToString();
                }
            }

            // Enregistrer nouvelle photo
            string photoFileName = "etu_" + userId + "_" + DateTime.Now.Ticks + extension;
            string folderPath = Server.MapPath("~/uploads/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, photoFileName);
            fuPhotoProfil.SaveAs(filePath);

            // Update DB
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE etudiant SET photo = @photo WHERE idEtudiant = @id", con))
                {
                    cmd.Parameters.AddWithValue("@photo", photoFileName);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Supprimer ancienne photo (si pas default)
            if (!string.IsNullOrWhiteSpace(oldPhoto) && !oldPhoto.ToLower().Contains("default"))
            {
                string oldPath = Server.MapPath("~/uploads/" + oldPhoto);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            // Rafraîchir image + session
            imgEtudiant.ImageUrl = "~/uploads/" + photoFileName;
            Session["photo"] = imgEtudiant.ImageUrl;

            log.AjouterLog(
                Session["code"]?.ToString() ?? "-",
                "FormProfileEtudiant.aspx",
                "Upload automatique photo profil",
                Session["role"]?.ToString() ?? "-"
            );

            ShowAlert("Photo mise à jour automatiquement ✅");
        }


        protected void btnRetour_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormAccueilEtudiant.aspx");
        }
    }
}
