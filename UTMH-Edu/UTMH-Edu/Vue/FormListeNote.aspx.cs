using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormListeNote : System.Web.UI.Page
    {
        Log log = new Log();
        string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

  
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
                log.AjouterLog(
                 Session["code"].ToString(),
                 "FormListeNote.aspx",
                 "Navigation sur la liste note",
                 Session["role"].ToString()
             );
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


        protected void ddlCours_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si l'utilisateur choisit "-- Sélectionner un cours --"
            if (ddlCours.SelectedIndex == 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "";
                btnAjouterNote.Visible = false;
                return;
            }
            btnAjouterNote.Visible = true;
            ChargerListeNotes();

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


        private void ChargerListeNotes()
        {
            if (ddlAnneAcademique.SelectedIndex == 0 &&
                ddlOption.SelectedIndex == 0 &&
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
    SELECT *
    FROM listeNotes
    WHERE (@annee = '0' OR anneeAcademique = @annee)
      AND (@idOption = '0' OR idOption = @idOption)
      AND (@idCours = '0' OR idCours = @idCours)
      AND (
            @recherche = '' 
            OR nom LIKE '%' + @recherche + '%'
            OR prenom LIKE '%' + @recherche + '%'
          )
    ORDER BY nom, prenom
", con);


                cmd.Parameters.AddWithValue("@annee", ddlAnneAcademique.SelectedValue);
                cmd.Parameters.AddWithValue("@idOption", ddlOption.SelectedValue);
                cmd.Parameters.AddWithValue("@idCours", ddlCours.SelectedValue);
                cmd.Parameters.AddWithValue("@recherche", txtRecherche.Text.Trim());


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                Lb2.Text = dt.Rows.Count == 0
                    ? "Aucune note trouvée."
                    : "";
            }
        }
        protected void txtRecherche_TextChanged(object sender, EventArgs e)
        {
            ChargerListeNotes();
        }
     

        protected void btnAjouterNote_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormNote.aspx");
        }




        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlType");
                if (ddlType != null)
                {
                    string typeNote = DataBinder.Eval(e.Row.DataItem, "typeNote").ToString();
                    ddlType.SelectedValue = typeNote;
                }
            }
        }


        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            ChargerListeNotes(); // recharge la liste avec la ligne en édition
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            ChargerListeNotes();
            Lb2.Text = "";
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idNote = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            GridViewRow row = GridView1.Rows[e.RowIndex];

            TextBox txtLibelle = (TextBox)row.FindControl("txtLibelle");
            TextBox txtNote = (TextBox)row.FindControl("txtNote");
            DropDownList ddlType = (DropDownList)row.FindControl("ddlType");

            if (txtLibelle == null || txtNote == null || ddlType == null)
            {
                Lb2.Text = "❌ Erreur lors de la modification.";
                return;
            }

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
            UPDATE note
            SET libelleNote = @libelle,
                noteObtenue = @note,
                typeNote = @type
            WHERE idNote = @idNote", con);

                cmd.Parameters.AddWithValue("@libelle", txtLibelle.Text);
                cmd.Parameters.AddWithValue("@note", txtNote.Text);
                cmd.Parameters.AddWithValue("@type", ddlType.SelectedValue);
                cmd.Parameters.AddWithValue("@idNote", idNote);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            ChargerListeNotes();

            Lb2.Text = "✅ Note modifiée avec succès";
        }


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int idNote = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM note WHERE idNote = @idNote", con);

                cmd.Parameters.AddWithValue("@idNote", idNote);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ChargerListeNotes();
            Lb2.Text = "🗑️ Note supprimée avec succès";
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