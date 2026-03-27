using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormAccueilEtudiant : System.Web.UI.Page
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
                ChargerNotesEtudiant();
                ChargerInfosEtudiant();
            }

            if (Session["nom"] != null)
            {
                string nom = Session["nom"].ToString();
                string prenom = Session["prenom"].ToString();

                // Affichage du nom dans les labels avec l'interpolation de chaîne
                //lbUser.Text = $"{nom+" "+prenom}";
                log.AjouterLog(
                   Session["code"].ToString(),
                   "FormAccueilEtudiant.aspx",
                   "Navigation sur la page Etudiant",
                   Session["role"].ToString()
               );
            }
            else
            {

                Response.Redirect("FormConnexion.aspx");
            }


        }

        [WebMethod(EnableSession = true)]
        public static string GetProgressionData()
        {
            var labels = new List<string>();
            var cours = new List<decimal>();
            var examens = new List<decimal>();



            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
                    SELECT 
                        DATENAME(MONTH, dateNote) + ' ' + CAST(YEAR(dateNote) AS VARCHAR(4)) AS Mois,
                        YEAR(dateNote) AS Annee,
                        MONTH(dateNote) AS NumMois,
                        AVG(CASE 
                                WHEN typeNote IN ('Cours', 'Devoir') 
                                THEN CAST(noteObtenue AS DECIMAL(10,2)) 
                            END) AS MoyCours,
                        AVG(CASE 
                                WHEN typeNote = 'Examen' 
                                THEN CAST(noteObtenue AS DECIMAL(10,2)) 
                            END) AS MoyExamens
                    FROM note
                    WHERE idEtudiant = @IdEtudiant
                    GROUP BY YEAR(dateNote), MONTH(dateNote), DATENAME(MONTH, dateNote)
                    ORDER BY Annee, NumMois;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    object userId = HttpContext.Current.Session["userId"];
                    if (userId == null)
                    {
                        return new JavaScriptSerializer().Serialize(new
                        {
                            labels = labels,
                            cours = cours,
                            examens = examens
                        });
                    }

                    cmd.Parameters.AddWithValue("@IdEtudiant", userId);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labels.Add(reader["Mois"].ToString());

                            cours.Add(
                                reader["MoyCours"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["MoyCours"])
                            );

                            examens.Add(
                                reader["MoyExamens"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(reader["MoyExamens"])
                            );
                        }
                    }
                }
            }

            var result = new
            {
                labels = labels,
                cours = cours,
                examens = examens
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        private void ChargerNotesEtudiant()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
                   SELECT TOP 5
    c.code,
    o.nom AS nomOption,
    c.nom AS nomCours,
    n.noteObtenue,
    n.typeNote,
    n.dateNote
FROM note n
INNER JOIN cours c ON n.idCours = c.idCours
INNER JOIN etudiant e ON n.idEtudiant = e.idEtudiant
INNER JOIN optionChoisie o ON e.idOption = o.idOption
WHERE n.idEtudiant = @IdEtudiant
ORDER BY n.dateNote DESC, n.idNote DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdEtudiant", Session["userId"]);

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


        private void ChargerInfosEtudiant()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
                    SELECT 
                        e.cin,
                        e.anneeAcademique,
                        o.nom AS nomOption
                    FROM dbo.etudiant e
                    INNER JOIN dbo.optionChoisie o ON e.idOption = o.idOption
                    WHERE e.idEtudiant = @IdEtudiant";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdEtudiant", Session["userId"]);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        lbMatricule.Text = dr["cin"].ToString(); // tu peux renommer ce label en lbCin
                        lbOption.Text = dr["nomOption"].ToString();
                        lbSessionActive.Text = dr["anneeAcademique"].ToString(); // tu peux renommer en lbAnneeAcademique
                    }
                    else
                    {
                        lbMatricule.Text = "...";
                        lbOption.Text = "...";
                        lbSessionActive.Text = "...";
                    }
                }
            }
        }


    }
}