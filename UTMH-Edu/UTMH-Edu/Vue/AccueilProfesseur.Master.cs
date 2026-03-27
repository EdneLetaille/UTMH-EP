using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class AccueilProfesseur : System.Web.UI.MasterPage
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null || Session["role"].ToString() != "Professeur")
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetNoServerCaching();
                this.AfficherHeure();

                int idProf = Convert.ToInt32(Session["userId"]);

                lbNombreEtudiants.Text = GetTotalEtudiantsProf(idProf).ToString();
                lbSolde.Text = GetMoisEnAttenteProf(idProf).ToString();
                lbNombreNotes.Text = GetNombreNotesProf(idProf).ToString();
            }
        }

        private void AfficherHeure()
        {
            TimeZoneInfo haitiZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime heureHaiti = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, haitiZone);

            lbHeure.Text = heureHaiti.ToString("HH:mm:ss");
        }
        protected void btnSeDeconnecter_Click(object sender, EventArgs e)
        {
            if (Session["code"] != null)
            {
                Log log = new Log();
                log.MettreAJourDeconnexion(Session["code"].ToString());
            }
            Session.Abandon();
            Session.Clear();
            Session["userId"] = null;
            Session["email"] = null;
            Session["role"] = null;
            Session["nom"] = null;
            Session.RemoveAll();
            Response.Redirect("FormConnexion.aspx");
        }


        private int GetTotalEtudiantsProf(int idProf)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT COUNT(DISTINCT e.idEtudiant)
        FROM cours c
        INNER JOIN OptionChoisieCours occ ON occ.idCours = c.idCours
        INNER JOIN etudiant e ON e.idOption = occ.idOption
        WHERE c.idProf = @idProf
          AND (e.statut IS NULL OR e.statut = 'Actif');
    ", con))
            {
                cmd.Parameters.AddWithValue("@idProf", idProf);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int GetMoisEnAttenteProf(int idProf)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT ISNULL(COUNT(DISTINCT CONCAT([annee], '-', [mois])),0)
        FROM dbo.payroll
        WHERE typePersonne='PROF'
        AND idProf=@idProf
        AND statut='EN_ATTENTE'
    ", con))
            {
                cmd.Parameters.AddWithValue("@idProf", idProf);
                con.Open();

                object result = cmd.ExecuteScalar();
                return (result == DBNull.Value) ? 0 : Convert.ToInt32(result);
            }
        }


        private int GetNombreNotesProf(int idProf)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT ISNULL(COUNT(*), 0)
        FROM dbo.note n
        INNER JOIN dbo.cours c ON c.idCours = n.idCours
        WHERE c.idProf = @idProf;
    ", con))
            {
                cmd.Parameters.AddWithValue("@idProf", idProf);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}