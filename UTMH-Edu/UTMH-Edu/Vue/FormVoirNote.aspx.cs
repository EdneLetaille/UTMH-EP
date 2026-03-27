using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace UTMH_Edu.Vue
{
    public partial class FormVoirNote : Page
    {
        private readonly string cs =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

           

            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                ChargerNotes();
            }
        }

        private void ChargerNotes()
        {
            try
            {
                int idEtudiant = GetIdEtudiantFromCode(Session["code"].ToString());

                string sql = @"
                    SELECT
                        c.nom AS Cours,
                        CAST(n.noteObtenue AS decimal(18,2)) AS NoteObtenue,
                        ISNULL(n.libelleNote,'') AS libelleNote,
                        ISNULL(n.typeNote,'') AS TypeNote
                    FROM dbo.note n
                    INNER JOIN dbo.cours c ON c.idCours = n.idCours
                    WHERE n.idEtudiant = @idEtudiant
                    ORDER BY n.dateNote DESC
                ";

                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);

                    DataTable dt = new DataTable();
                    con.Open();
                    da.Fill(dt);

                    gvNotes.DataSource = dt;
                    gvNotes.DataBind();
                }
            }
            catch (Exception ex)
            {
                lbMsg.Text = "Erreur : " + ex.Message;
            }
        }

        private int GetIdEtudiantFromCode(string code)
        {
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 idEtudiant FROM dbo.etudiant WHERE code = @code", con))
            {
                cmd.Parameters.AddWithValue("@code", code);
                con.Open();

                object result = cmd.ExecuteScalar();

                if (result == null)
                    throw new Exception("Étudiant introuvable.");

                return Convert.ToInt32(result);
            }
        }
    }
}