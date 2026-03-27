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
using System.Linq;

namespace UTMH_Edu.Vue
{
    public partial class FormCours : System.Web.UI.Page
    {
        Log log = new Log();
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurCours Cour = new ControlleurCours();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                this.listeNomOption();
                this.listeNomProfesseur();
                Session["idCours"] = Request.QueryString["idCours"];
            }
        }

        public void codeCours()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                return;
            }

            string nom = txtNom.Text.Trim();

            string codeNom = nom.Length >= 2 ? nom.Substring(0, 2).ToUpper() : nom.ToUpper();
            Random rnd = new Random();
            int randomNumber = rnd.Next(10, 99);

            txtCode.Text = codeNom + "-" + randomNumber;
        }

       

        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }


        public void listeNomOption()
        {
            string query = "SELECT idOption, nom FROM optionChoisie";

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                chkOptions.Items.Clear();

                while (dr.Read())
                {
                    chkOptions.Items.Add(
                        new ListItem(
                            dr["nom"].ToString(),
                            dr["idOption"].ToString()
                        )
                    );
                }
            }
        }


        public void listeNomProfesseur()
        {
            string query = "SELECT * FROM professeur";
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
            }

            ddlNomProfesseur.Items.Clear();
            ddlNomProfesseur.Items.Add(new ListItem("-- Sélectionnez --", "0"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlNomProfesseur.Items.Add(new ListItem(
                     row["nom"].ToString() + " " + row["prenom"].ToString(),
                    row["idProf"].ToString()
                ));
            }
        }

        void videchamps()
        {
            txtCode.Text = "";
            txtNom.Text = "";
            txtDescription.Text = "";
            txtHeureDebut.Text = "";
            txtHeureFin.Text = "";
            txtDuree.Text = "";
            ddlStatut.Text = "";
            txtCoefficient.Text = "";
            chkOptions.SelectedIndex = 0;
            ddlNomProfesseur.SelectedIndex = 0;

        }
        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            // 🔴 Vérifier professeur d'abord
            if (ddlNomProfesseur.SelectedValue == "0")
            {
                ShowAlert("Veuillez sélectionner un professeur !");
                return;
            }

            int idProf = int.Parse(ddlNomProfesseur.SelectedValue);

            // 🔴 Vérifier durée valide (1 à 5 heures)
            int duree;
            if (!int.TryParse(txtDuree.Text, out duree) || duree < 1 || duree > 5)
            {
                ShowAlert("La durée doit être comprise entre 1 et 5 heures !");
                return;
            }

            // 🔴 Vérifier heure début
            DateTime heureDebut;
            if (!DateTime.TryParse(txtHeureDebut.Text, out heureDebut))
            {
                ShowAlert("Veuillez entrer une heure de début valide !");
                return;
            }

            // ✅ Calcul automatique heure fin
            DateTime heureFin = heureDebut.AddHours(duree);

            // IMPORTANT : format correct
            txtHeureFin.Text = heureFin.ToString("HH:mm");

            this.codeCours();

            // 🔴 Vérifier si le professeur a déjà un cours
            if (Cour.professeurDejaAssigne(idProf))
            {
                ShowAlert("Ce professeur est déjà assigné à un cours !");
                return;
            }

            // 🔴 Validation champs obligatoires
            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                string.IsNullOrWhiteSpace(txtNom.Text))
            {
                ShowAlert("Certains champs sont vides !");
                return;
            }

            // 🔴 Vérifier doublon
            if (Cour.nomCoursExisteDansSysteme(txtNom.Text))
            {
                ShowAlert("Le nom existe déjà dans le système.");
                return;
            }

            // 🔴 Vérifier option
            bool optionSelectionnee = chkOptions.Items
                .Cast<ListItem>()
                .Any(i => i.Selected);

            if (!optionSelectionnee)
            {
                ShowAlert("Veuillez sélectionner au moins une option !");
                return;
            }

            // 1️⃣ Création du cours
            ddlStatut.Text = "Actif";
            int idCours = Cour.creerCours(
                idProf,
                txtCode.Text.Trim(),
                txtNom.Text.Trim(),
                txtDescription.Text.Trim(),
                heureDebut.ToString("HH:mm"),   // ⚡ utiliser variable
                heureFin.ToString("HH:mm"),     // ⚡ utiliser variable
                duree.ToString(),
                ddlStatut.Text,
                txtCoefficient.Text
            );

            foreach (ListItem item in chkOptions.Items)
            {
                if (item.Selected)
                {
                    Cour.lierCoursOption(idCours, int.Parse(item.Value));
                }
            }

            ShowAlert("Inscription effectuée avec succès.");
            this.videchamps();
        }

        protected void CalculerHeureFin(object sender, EventArgs e)
        {
            DateTime heureDebut;
            int duree;

            if (DateTime.TryParse(txtHeureDebut.Text, out heureDebut) &&
                int.TryParse(txtDuree.Text, out duree))
            {
                DateTime heureFin = heureDebut.AddHours(duree);
                txtHeureFin.Text = heureFin.ToString("HH:mm");
            }
            else
            {
                txtHeureFin.Text = "";
            }
        }
        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.videchamps();
        }
    }
}