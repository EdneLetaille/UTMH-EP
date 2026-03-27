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
    public partial class FormListeAdministrateur : System.Web.UI.Page
    {
        Log log = new Log();
        private static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                this.chargerAdministrateur();
                log.AjouterLog(
                 Session["code"].ToString(),
                 "FormListeAdministrateur.aspx",
                 "Navigation sur la liste administrateur",
                 Session["role"].ToString()
             );
            }
        }

       protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            txtRecherche.Text = "";
            Lb2.Text = "";
            ddlStatut.SelectedIndex = 0; // "Tous"
            chargerAdministrateur();
        }

        protected void btnRecherche_Click(object sender, EventArgs e)
        {
            string recherche = txtRecherche.Text.Trim();
            string statut = ddlStatut.SelectedValue;
            rechercherAdministrateur(recherche, statut);
        }
        

        void chargerAdministrateur()
        {
            SqlConnection con = new SqlConnection(strCon);
            string chReq = "SELECT * from administrateur order by idAdm DESC";
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                cmd = new SqlCommand(chReq, con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    Lb2.Text += "<br/><span style='color:red;'>Il n'y a pas d'administrateur.</span>";
                }
            }
            catch (Exception ex)
            {
                Response.Write("Erreur : " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            // code etidyan an soti nan DataKey
            string code = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();

            // URL dossier la
            string url = "FormDossierAdministrateur.aspx?code=" + code;

            // Mete curseur & hover
            e.Row.Style["cursor"] = "pointer";
            e.Row.Attributes["title"] = "Cliquer pour voir le dossier";

            // Fè row la klike (si w klike nenpòt kote sou row la)
            e.Row.Attributes["onclick"] = "window.location='" + url + "';";
        }
        void rechercherAdministrateur(string recherche = "", string statut = "")
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                // Requête SQL avec filtre sur statut et recherche
                string chReq = "SELECT code, nom, prenom, email, telephone, statut " +
                               "FROM administrateur " +
                               "WHERE (@rech = '' OR nom LIKE '%' + @rech + '%' " +
                               "OR prenom LIKE '%' + @rech + '%' OR code LIKE '%' + @rech + '%') " +
                               "AND (@statut = '' OR statut = @statut) " +
                               "ORDER BY code DESC";

                using (SqlCommand cmd = new SqlCommand(chReq, con))
                {
                    cmd.Parameters.AddWithValue("@rech", recherche);
                    cmd.Parameters.AddWithValue("@statut", statut);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Lb2.Text = dt.Rows.Count > 0 ? "" : "<span style='color:red;'>Aucun administrateur trouvé.</span>";
                }
            }
        }


    }
}