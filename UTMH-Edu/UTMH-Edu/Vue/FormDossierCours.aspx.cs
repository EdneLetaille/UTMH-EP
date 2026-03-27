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
    public partial class FormDossierCours : System.Web.UI.Page
    {
        Log log = new Log();
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
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
                this.ChargerOptions();
                this.ChargerNomProfesseur();

                if (Request.QueryString["code"] == null)
                {
                    Response.Redirect("FormListeCours.aspx");
                    return;
                }

                ChargerDossierCours(Request.QueryString["code"]);



            }

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
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT idOption, nom FROM optionChoisie", con))
            {
                con.Open();
                chkOptions.DataSource = cmd.ExecuteReader();
                chkOptions.DataTextField = "nom";
                chkOptions.DataValueField = "idOption";
                chkOptions.DataBind();
            }
        }


        // =========================
        // CHARGER PROFESSEUR
        // =========================
        private void ChargerNomProfesseur()
        {
            string query =" SELECT idProf, nom + ' ' + prenom AS NomComplet FROM professeur";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlNomProfesseur.DataSource = dr;
                ddlNomProfesseur.DataTextField = "NomComplet";   // affiché
                ddlNomProfesseur.DataValueField = "idProf";   // stocké
                ddlNomProfesseur.DataBind();
            }

            ddlNomProfesseur.Items.Insert(0, new ListItem("-- Sélectionner un professeur --", ""));
        }

        // =========================
        // CHARGER DOSSIER COURS
        // =========================
        private void ChargerDossierCours(string codeCours)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT idCours, idProf, code, nom, description,
               heureDebut, heureFin, duree, statut, coefficient
        FROM cours
        WHERE code = @code", con))
            {
                cmd.Parameters.AddWithValue("@code", codeCours);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int idCours = Convert.ToInt32(dr["idCours"]);
                    Session["idCours"] = idCours;

                    ddlNomProfesseur.SelectedValue = dr["idProf"].ToString();
                    txtCode.Text = dr["code"].ToString();
                    txtNom.Text = dr["nom"].ToString();
                    txtDescription.Text = dr["description"].ToString();
                    txtHeureDebut.Text = dr["heureDebut"].ToString();
                    txtHeureFin.Text = dr["heureFin"].ToString();
                    txtDuree.Text = dr["duree"].ToString();
                    ddlStatut.SelectedValue = dr["statut"].ToString();
                    txtCoefficient.Text = dr["coefficient"].ToString();
                }
            }

            ChargerOptionsSelectionnees();
        }
        private void ChargerOptionsSelectionnees()
        {
            int idCours = (int)Session["idCours"];

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT idOption
        FROM OptionChoisieCours
        WHERE idCours = @idCours", con))
            {
                cmd.Parameters.AddWithValue("@idCours", idCours);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem item = chkOptions.Items
                        .FindByValue(dr["idOption"].ToString());

                    if (item != null)
                        item.Selected = true;
                }
            }
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

        protected void btnModifier_Click(object sender, EventArgs e)
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
       
            // 🔴 Validation champs obligatoires
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                ShowAlert("Certains champs sont vides !");
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
            int idCours = (int)Session["idCours"];

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                SqlTransaction tx = con.BeginTransaction();

                try
                {
                    // 1️⃣ Update cours
                    SqlCommand cmd = new SqlCommand(@"
                UPDATE cours SET
                    idProf = @idProf,
                    nom = @nom,
                    description = @description,
                    heureDebut = @heureDebut,
                    heureFin = @heureFin,
                    duree = @duree,
                    statut = @statut,
                    coefficient = @coefficient
                WHERE idCours = @idCours", con, tx);

                    cmd.Parameters.AddWithValue("@idCours", idCours);
                    cmd.Parameters.AddWithValue("@idProf", ddlNomProfesseur.SelectedValue);
                    cmd.Parameters.AddWithValue("@nom", txtNom.Text);
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text);
                    cmd.Parameters.AddWithValue("@heureDebut", txtHeureDebut.Text);
                    cmd.Parameters.AddWithValue("@heureFin", txtHeureFin.Text);
                    cmd.Parameters.AddWithValue("@duree", txtDuree.Text);
                    cmd.Parameters.AddWithValue("@statut", ddlStatut.SelectedValue);
                    cmd.Parameters.AddWithValue("@coefficient", txtCoefficient.Text);

                    cmd.ExecuteNonQuery();

                    // 2️⃣ Supprimer anciennes options
                    new SqlCommand(
                        "DELETE FROM OptionChoisieCours WHERE idCours = @idCours",
                        con, tx)
                    {
                        Parameters = { new SqlParameter("@idCours", idCours) }
                    }.ExecuteNonQuery();

                    // 3️⃣ Réinsérer options
                    foreach (ListItem item in chkOptions.Items)
                    {
                        if (item.Selected)
                        {
                            SqlCommand insert = new SqlCommand(@"
                        INSERT INTO OptionChoisieCours (idCours, idOption)
                        VALUES (@idCours, @idOption)", con, tx);

                            insert.Parameters.AddWithValue("@idCours", idCours);
                            insert.Parameters.AddWithValue("@idOption", item.Value);
                            insert.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                    log.AjouterLog(
                   Session["code"].ToString(),
                   "FormDossierCours.aspx",
                   "Modification Cours",
                   Session["role"].ToString()
               );
                    ShowAlert("Cours modifié avec succès !");
                }
                catch
                {
                    tx.Rollback();
                    ShowAlert("Erreur lors de la modification");
                }
            }
        }

        protected void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (Session["idCours"] == null)
            {
                ShowAlert("Session expirée.");
                return;
            }

            int idCours = (int)Session["idCours"];

            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    SqlTransaction tx = con.BeginTransaction();

                    // 1️⃣ Supprimer les liens cours-options
                    SqlCommand cmd1 = new SqlCommand(
                        "DELETE FROM OptionChoisieCours WHERE idCours = @idCours",
                        con, tx);
                    cmd1.Parameters.AddWithValue("@idCours", idCours);
                    cmd1.ExecuteNonQuery();

                    // 2️⃣ Supprimer le cours
                    SqlCommand cmd2 = new SqlCommand(
                        "DELETE FROM cours WHERE idCours = @idCours",
                        con, tx);
                    cmd2.Parameters.AddWithValue("@idCours", idCours);
                    cmd2.ExecuteNonQuery();

                    tx.Commit();
                }
                log.AjouterLog(
                  Session["code"].ToString(),
                  "FormDossierCours.aspx",
                  "Supression Cours",
                  Session["role"].ToString()
              );
                ShowAlert("Cours supprimé avec succès !");
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(),
                    "redirect",
                    "setTimeout(function(){ window.location='FormListeCours.aspx'; }, 2000);",
                    true);
            }
            catch (Exception ex)
            {
                ShowAlert("Erreur lors de la suppression : " + ex.Message);
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormListeCours.aspx");
        }
    }
}