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

namespace UTMH_Edu.Vue
{
    public partial class FormProfileProfesseur : System.Web.UI.Page
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

                // Charger les informations professeur
                chargerInformationProfesseur();

            }
        }

        void chargerInformationProfesseur()
        {
            if (Session["userId"] != null)
            {
                string userId = Session["userId"].ToString();
                string nom = Session["nom"]?.ToString();
                string email = Session["email"]?.ToString();
                string prenom = Session["prenom"]?.ToString();
                string code = Session["code"]?.ToString();
                string date = Session["dateEmbauche"]?.ToString();
                string salaire = Session["salaire"]?.ToString();


              

                // Affichage des informations
                txtNom.Text = nom;
                txtEmail.Text = email;
                txtPrenom.Text = prenom;
                txtCode.Text = code;
                txtDateEmbauche.Text = date;
                txtSalaire.Text = salaire;


                // Photo depuis DB
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT photo FROM professeur WHERE idProf = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        var photo = cmd.ExecuteScalar()?.ToString();

                        imgProf.ImageUrl = !string.IsNullOrEmpty(photo)
                            ? "~/uploads/" + photo
                            : "~/uploads/default-user.png";
                    }
                }

            }
        }
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
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
                using (SqlCommand cmd = new SqlCommand("SELECT photo FROM professeur WHERE idProf = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    oldPhoto = cmd.ExecuteScalar()?.ToString();
                }
            }

            // Enregistrer nouvelle photo
            string photoFileName = "prof_" + userId + "_" + DateTime.Now.Ticks + extension;
            string folderPath = Server.MapPath("~/uploads/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, photoFileName);
            fuPhotoProfil.SaveAs(filePath);

            // Update DB
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE professeur SET photo = @photo WHERE idProf = @id", con))
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
            imgProf.ImageUrl = "~/uploads/" + photoFileName;
            Session["photo"] = imgProf.ImageUrl;

            log.AjouterLog(
                Session["code"]?.ToString() ?? "-",
                "FormProfileProfesseur.aspx",
                "Upload automatique photo profil",
                Session["role"]?.ToString() ?? "-"
            );

            ShowAlert("Photo mise à jour automatiquement ✅");
        }
        protected void btnRetour_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormAccueilProfesseur.aspx");
        }
    }
}