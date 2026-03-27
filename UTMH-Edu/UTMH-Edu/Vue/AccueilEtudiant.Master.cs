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
    public partial class AccueilEtudiant : System.Web.UI.MasterPage
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null || Session["role"].ToString() != "Etudiant")
            {
                Response.Redirect("FormDeconnexion.aspx");
                return;
            }
            string role = Session["role"].ToString();

            //if (role == "Responsable")
            //{
            //    //menuAdministration.Visible = false;
            //}
            if (!IsPostBack) {

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetNoServerCaching();
                this.AfficherHeure();
                lbNombreCours.Text = nombreCours().ToString();
                lbSolde.Text = soldeRestant().ToString();
                lbNombreNotes.Text = nombreNote().ToString();
            }
           
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

        private void AfficherHeure()
        {
            TimeZoneInfo haitiZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime heureHaiti = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, haitiZone);

            lbHeure.Text = heureHaiti.ToString("HH:mm:ss");
        }

        public int nombreCours()
        {
            if (Session["userId"] == null)
            {
                return 0;
            }

            int total = 0;


            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            SELECT COUNT(DISTINCT occ.idCours)
            FROM OptionChoisieCours occ
            INNER JOIN etudiant e ON occ.idOption = e.idOption
            WHERE e.idEtudiant = @idEtudiant";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idEtudiant", Session["userId"]);

                con.Open();
                total = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return total;
        }


        public int nombreNote()
        {
            if (Session["userId"] == null)
            {
                return 0;
            }

            int total = 0;


            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            SELECT COUNT(*)
            FROM note
            WHERE idEtudiant = @idEtudiant";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idEtudiant", Session["userId"]);

                con.Open();
                total = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return total;
        }


        public decimal soldeRestant()
        {
            if (Session["userId"] == null)
                return 0;

            decimal solde = 0;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            SELECT 
                ISNULL(
                    (SELECT TOP 1 balance 
                     FROM paiement 
                     WHERE idEtudiant = @idEtudiant
                     ORDER BY datePaiement DESC),
                    20000
                ) AS Solde";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add("@idEtudiant", System.Data.SqlDbType.Int)
                    .Value = Convert.ToInt32(Session["userId"]);

                con.Open();
                solde = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            return solde;
        }
    }
}