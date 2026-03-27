using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormNote1 : System.Web.UI.Page
    {
        Log log = new Log();
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurNote Note = new ControlleurNote();
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
                listeNomOption();

                ddlCours.Items.Clear();
                ddlCours.Items.Add(new ListItem("-- Sélectionner un cours --", "0"));
            }
        }

        private void ChargerAnnees()
        {
            ddlAnneAcademique.Items.Clear();
            ddlAnneAcademique.Items.Add(new ListItem("Année académique", "0"));

            using (SqlConnection con = new SqlConnection(strCon))
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

        protected void ddlAnneAcademique_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();

            ddlCours.Items.Clear();
            ddlCours.Items.Add(new ListItem("-- Sélectionner un cours --", "0"));
        }


        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCours.Items.Clear();
            ddlCours.Items.Add(new ListItem("-- Sélectionner un cours --", "0"));

            if (ddlOption.SelectedIndex == 0)
                return;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT c.idCours, c.nom
            FROM cours c
            INNER JOIN OptionChoisieCours occ ON c.idCours = occ.idCours
            WHERE occ.idOption = @idOption
        ", con);

                cmd.Parameters.AddWithValue("@idOption", ddlOption.SelectedValue);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlCours.Items.Add(new ListItem(
                        dr["nom"].ToString(),
                        dr["idCours"].ToString()
                    ));
                }
            }

            GridView1.DataSource = null;
            GridView1.DataBind();
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
            ddlOption.Items.Add(new ListItem("-- Sélectionnez une option", "0"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlOption.Items.Add(new ListItem(
                    row["nom"].ToString(),
                    row["idOption"].ToString()
                ));
            }
        }



        private void ChargerNotes()
        {
            // Tant que tout n'est pas sélectionné → rien afficher
            if (ddlAnneAcademique.SelectedIndex == 0 ||
                ddlOption.SelectedIndex == 0 ||
                ddlCours.SelectedIndex == 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "";
                return;
            }

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
           SELECT
    e.idEtudiant,
    c.idCours,
    c.nom AS NomCours,
    e.nom + ' ' + e.prenom AS NomEtudiant
FROM etudiant e
INNER JOIN optionChoisie o ON o.idOption = e.idOption
INNER JOIN OptionChoisieCours oc ON oc.idOption = o.idOption
INNER JOIN cours c ON c.idCours = oc.idCours
WHERE e.anneeAcademique = @annee
  AND o.idOption = @idOption
  AND c.idCours = @idCours
ORDER BY e.nom", con);

                cmd.Parameters.AddWithValue("@annee", ddlAnneAcademique.SelectedValue);
                cmd.Parameters.AddWithValue("@idOption", ddlOption.SelectedValue);
                cmd.Parameters.AddWithValue("@idCours", ddlCours.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();

             
            }
        }


        protected void ddlCours_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si l'utilisateur choisit "-- Sélectionner un cours --"
            if (ddlCours.SelectedIndex == 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "";
                return;
            }

            ChargerNotes();

            // Vérifier APRÈS le chargement
            if (GridView1.Rows.Count == 0)
            {
                Lb2.Text = "Aucun étudiant trouvé pour ce cours.";
            }
            else
            {
                Lb2.Text = "";
            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EnregistrerNote")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[index];

                int idEtudiant = Convert.ToInt32(GridView1.DataKeys[index].Values["idEtudiant"]);
                int idCours = Convert.ToInt32(GridView1.DataKeys[index].Values["idCours"]);

                TextBox txtNote = (TextBox)row.FindControl("txtNote");
                TextBox txtLibelleNote = (TextBox)row.FindControl("txtLibelleNote");
                DropDownList ddlTypeNote = (DropDownList)row.FindControl("ddlTypeNote");

                if (string.IsNullOrWhiteSpace(txtNote.Text) ||
                    string.IsNullOrWhiteSpace(txtLibelleNote.Text))
                {
                    Lb2.Text = "Veuillez saisir la note et le libellé.";
                    return;
                }

                using (SqlConnection con = new SqlConnection(strCon))
                {
                    SqlCommand cmd = new SqlCommand(@"
            IF NOT EXISTS (
                SELECT 1 FROM note
                WHERE idEtudiant = @idEtudiant
                  AND idCours = @idCours
                  AND libelleNote = @libelleNote
            )
            BEGIN
                INSERT INTO note (idCours, idEtudiant, noteObtenue, typeNote, libelleNote, dateNote)
                VALUES (@idCours, @idEtudiant, @note, @typeNote, @libelleNote, GETDATE())
            END
        ", con);

                    cmd.Parameters.AddWithValue("@idCours", idCours);
                    cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                    cmd.Parameters.AddWithValue("@note", txtNote.Text);
                    cmd.Parameters.AddWithValue("@typeNote", ddlTypeNote.SelectedValue);
                    cmd.Parameters.AddWithValue("@libelleNote", txtLibelleNote.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                log.AjouterLog(
                 Session["code"].ToString(),
                 "FormNote.aspx",
                 "Enregistrement note",
                 Session["role"].ToString()
             );

                Lb2.Text = "Note ajoutée ✔";
            }

        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            ddlAnneAcademique.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;

            ddlCours.Items.Clear();
            ddlCours.Items.Add(new ListItem("-- Sélectionner un cours --", "0"));

            GridView1.DataSource = null;
            GridView1.DataBind();

            Lb2.Text = "";
        }



    }
}