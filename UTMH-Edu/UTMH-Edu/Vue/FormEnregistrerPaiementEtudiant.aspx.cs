using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UTMH_Edu.Vue
{
    public partial class FormEnregistrerPaiementEtudiant : System.Web.UI.Page
    {
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
                ChargerOptions();
                ChargerEtudiants();
            }
        }

        private void ChargerAnnees()
        {
            ddlAnneAcademique.Items.Clear();
            ddlAnneAcademique.Items.Add(new ListItem("Toutes les années", "0"));

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT anneeAcademique FROM etudiant ORDER BY anneeAcademique DESC", con);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddlAnneAcademique.Items.Add(
                        new ListItem(dr["anneeAcademique"].ToString(), dr["anneeAcademique"].ToString()));
                }
            }
        }

        private void ChargerOptions()
        {
            ddlOption.Items.Clear();
            ddlOption.Items.Add(new ListItem("Toutes les options", "0"));

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT idOption, nom FROM optionChoisie ORDER BY nom", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    ddlOption.Items.Add(
                        new ListItem(row["nom"].ToString(), row["idOption"].ToString()));
                }
            }
        }

        private void ChargerEtudiants()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
   SELECT
    e.code,
    e.nom,
    e.prenom,
    o.nom AS NomOption,
    e.anneeAcademique,
    20000 AS montantAPayer
FROM etudiant e
INNER JOIN optionChoisie o
    ON e.idOption = o.idOption
LEFT JOIN paiement p
    ON e.idEtudiant = p.idEtudiant
WHERE 1 = 1";




                SqlCommand cmd = new SqlCommand(query, con);

                // Filtre année académique
                if (ddlAnneAcademique.SelectedValue != "0")
                {
                    query += " AND e.anneeAcademique = @annee";
                    cmd.Parameters.AddWithValue("@annee", ddlAnneAcademique.SelectedValue);
                }

                // Filtre option
                if (ddlOption.SelectedValue != "0")
                {
                    query += " AND e.idOption = @idOption";
                    cmd.Parameters.AddWithValue("@idOption", ddlOption.SelectedValue);
                }

                // Recherche
                if (!string.IsNullOrWhiteSpace(txtRecherche.Text))
                {
                    query += @" AND (
                e.nom LIKE @recherche OR 
                e.prenom LIKE @recherche OR 
                e.code LIKE @recherche
            )";
                    cmd.Parameters.AddWithValue("@recherche", "%" + txtRecherche.Text + "%");
                }

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }


        protected void Filtre_Changed(object sender, EventArgs e)
        {
            ChargerEtudiants();
        }

        protected void btnRecherche_Click(object sender, EventArgs e)
        {
            ChargerEtudiants();
        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            ddlAnneAcademique.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;
            txtRecherche.Text = "";
            ChargerEtudiants();
        }
    }
}