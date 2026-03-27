using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UTMH_Edu.Vue
{
    public partial class FormPresence : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                ChargerAnnees();
                ChargerCours();
                ChargerOptions();
                txtDatePresence.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        // =======================
        // CHARGEMENT DES LISTES
        // =======================

        private void ChargerAnnees()
        {
            ddlAnneAcademique.Items.Clear();
            ddlAnneAcademique.Items.Add(new ListItem("Année académique", "0"));

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT anneeAcademique FROM etudiant ORDER BY anneeAcademique DESC",
                    con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlAnneAcademique.Items.Add(dr["anneeAcademique"].ToString());
                }
            }
        }

        private void ChargerCours()
        {
            ddlCours.Items.Clear();
            ddlCours.Items.Add(new ListItem("-- Tous les cours --", "0"));

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT idCours, nom FROM cours ORDER BY nom",
                    con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlCours.Items.Add(new ListItem(
                        dr["nom"].ToString(),
                        dr["idCours"].ToString()));
                }
            }
        }

        private void ChargerOptions()
        {
            ddlOption.Items.Clear();
            ddlOption.Items.Add(new ListItem("Options", "0"));

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT idOption, nom FROM optionChoisie ORDER BY nom",
                    con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlOption.Items.Add(new ListItem(
                        dr["nom"].ToString(),
                        dr["idOption"].ToString()));
                }
            }
        }

        // =======================
        // EVENEMENTS
        // =======================
        
        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerPresences();
        }
        protected void ddlAnneAcademique_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerPresences();
        }
        protected void ddlCours_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerPresences();
        }

        protected void btnRecherche_Click(object sender, EventArgs e)
        {
            ChargerPresences();
        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            ddlCours.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;
            ddlAnneAcademique.SelectedIndex = 0;
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        // =======================
        // CHARGEMENT PRESENCES
        // =======================

        private void ChargerPresences()
        {
            // ✅ Sécurité si aucune date choisie
            DateTime datePresence = string.IsNullOrEmpty(txtDatePresence.Text)
                ? DateTime.Today
                : DateTime.Parse(txtDatePresence.Text);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT
                e.idEtudiant,
                c.idCours,
                c.idProf,
                c.nom AS NomCours,
                pr.nom + ' ' + pr.prenom AS NomProfesseur,
                e.nom + ' ' + e.prenom AS NomEtudiant,

                ISNULL(p.date, @datePresence) AS datePresence,
                ISNULL(p.heure, CONVERT(VARCHAR(5), GETDATE(), 108)) AS heurePresence,
                ISNULL(p.status, 'Présent') AS status,

                CASE 
                    WHEN p.idPresence IS NOT NULL THEN 1
                    ELSE 0
                END AS DejaValide
            FROM etudiantCours ec
            INNER JOIN etudiant e ON ec.idEtudiant = e.idEtudiant
            INNER JOIN cours c ON ec.idCours = c.idCours
            INNER JOIN professeur pr ON c.idProf = pr.idProf
            LEFT JOIN presence p
                ON p.idEtudiant = e.idEtudiant
               AND p.idCours = c.idCours
               AND CAST(p.date AS DATE) = CAST(@datePresence AS DATE)

            WHERE
                (@idCours = 0 OR c.idCours = @idCours)
                AND (@idOption = 0 OR e.idOption = @idOption)
                AND (@annee = '0' OR e.anneeAcademique = @annee)
            ORDER BY e.nom
        ", con);

                cmd.Parameters.AddWithValue("@idCours", ddlCours.SelectedValue);
                cmd.Parameters.AddWithValue("@idOption", ddlOption.SelectedValue);
                cmd.Parameters.AddWithValue("@annee", ddlAnneAcademique.SelectedValue);
                cmd.Parameters.AddWithValue("@datePresence", datePresence);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }



        // =======================
        // ENREGISTREMENT PRESENCE
        // =======================

        private void EnregistrerPresence(
      int idEtudiant,
      int idCours,
      int idProf,
      string status,
      DateTime datePresence)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
        IF EXISTS (
            SELECT 1 FROM presence
            WHERE idEtudiant = @idEtudiant
              AND idCours = @idCours
              AND date = @datePresence
        )
        BEGIN
            UPDATE presence
            SET status = @status,
                heure = @heure,
                idProf = @idProf
            WHERE idEtudiant = @idEtudiant
              AND idCours = @idCours
              AND date = @datePresence
        END
        ELSE
        BEGIN
            INSERT INTO presence
                (idEtudiant, idCours, idProf, date, heure, status)
            VALUES
                (@idEtudiant, @idCours, @idProf,
                 @datePresence,
                 @heure,
                 @status)
        END
        ", con);

                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                cmd.Parameters.AddWithValue("@idCours", idCours);
                cmd.Parameters.AddWithValue("@idProf", idProf);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@datePresence", datePresence.Date);
                cmd.Parameters.AddWithValue("@heure", DateTime.Now.ToString("HH:mm"));

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ValiderPresence")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[index];

                int idEtudiant = Convert.ToInt32(
                    GridView1.DataKeys[index]["idEtudiant"]);

                int idCours = Convert.ToInt32(
                    GridView1.DataKeys[index]["idCours"]);

                int idProf = Convert.ToInt32(
                    GridView1.DataKeys[index]["idProf"]);

                DropDownList ddlStatut =
                    (DropDownList)row.FindControl("ddlStatut");
                // ✅ DATE CHOISIE PAR L’UTILISATEUR
                DateTime datePresence = DateTime.Parse(txtDatePresence.Text);
                EnregistrerPresence(
                    idEtudiant,
                    idCours,
                    idProf,
                    ddlStatut.SelectedValue,
                    datePresence
                );

                // 🔥 IMPORTANT : recharger les données
                ChargerPresences();

                Lb2.Text = "Présence validée.";
                Lb2.CssClass = "text-success";
            }
        }




    }
}
