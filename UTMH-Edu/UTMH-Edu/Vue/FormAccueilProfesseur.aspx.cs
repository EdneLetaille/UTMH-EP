using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormAccueilProfesseur : System.Web.UI.Page
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        Log log = new Log();
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (Session["userId"] == null)
            {
                Response.Redirect("FormConnexion.aspx");
                return;
            }

            if (!IsPostBack)
            {              
                this.ChargerInfosProfesseur();
                this.ChargerNotesProfesseur();
                this.ChargerGraphNotesParMois();
            }

            if (Session["nom"] != null)
            {
                string nom = Session["nom"].ToString();
                string prenom = Session["prenom"].ToString();

                // Affichage du nom dans les labels avec l'interpolation de chaîne
                //lbUser.Text = $"{nom+" "+prenom}";
                log.AjouterLog(
                   Session["code"].ToString(),
                  "FormAccueilProfesseur.aspx",
                   "Navigation sur la page Professeur",
                   Session["role"].ToString()
               );
            }
            else
            {

                Response.Redirect("FormConnexion.aspx");
            }
        }

        private void ChargerGraphNotesParMois()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT 
            MONTH(n.dateNote) AS Mois,
            DATENAME(MONTH, n.dateNote) AS NomMois,
            COUNT(*) AS NombreNotes
        FROM note n
        INNER JOIN cours c ON n.idCours = c.idCours
        WHERE c.idProf = @idProf
          AND YEAR(n.dateNote) = YEAR(GETDATE())
        GROUP BY MONTH(n.dateNote), DATENAME(MONTH, n.dateNote)
        ORDER BY MONTH(n.dateNote);
    ", con))
            {
                cmd.Parameters.AddWithValue("@idProf", Session["userId"]); // ou Session["idProf"]

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                StringBuilder labels = new StringBuilder();
                StringBuilder values = new StringBuilder();

                while (dr.Read())
                {
                    labels.Append("'" + dr["NomMois"].ToString() + "',");
                    values.Append(dr["NombreNotes"].ToString() + ",");
                }

                hiddenLabels.Value = labels.ToString().TrimEnd(',');
                hiddenValues.Value = values.ToString().TrimEnd(',');
            }
        }


        private void ChargerNotesProfesseur()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            SELECT TOP 5
                e.code,
                e.nom,
                e.prenom,
                o.nom AS nomOption,
                c.nom AS nomCours,
                n.noteObtenue,
                n.typeNote,
                n.dateNote
            FROM note n
            INNER JOIN cours c ON n.idCours = c.idCours
            INNER JOIN etudiant e ON n.idEtudiant = e.idEtudiant
            INNER JOIN optionChoisie o ON e.idOption = o.idOption
            WHERE c.idProf = @IdProf
            ORDER BY n.dateNote DESC, n.idNote DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdProf", Session["userId"]);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        GridView2.DataSource = dt;
                        GridView2.DataBind();
                    }
                }
            }
        }
        private void ChargerInfosProfesseur()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
                    SELECT
                    dbo.professeur.cin, dbo.cours.nom
                    FROM  dbo.professeur INNER JOIN
                     dbo.cours ON dbo.professeur.idProf = dbo.cours.idProf
                     WHERE dbo.professeur.idProf = @IdProf";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdProf", Session["userId"]);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        lbCin.Text = dr["cin"].ToString();
                        lbNomCoursEnseigner.Text = dr["nom"].ToString();                    
                    }
                    else
                    {
                        lbCin.Text = "...";
                        lbNomCoursEnseigner.Text = "...";
                    }
                }
            }
        }



    }
}