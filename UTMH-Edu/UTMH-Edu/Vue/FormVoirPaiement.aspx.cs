using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace UTMH_Edu.Vue
{
    public partial class FormVoirPaiement : Page
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("FormDeconnexion.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ChargerPaiements();
            }
        }

        private string CodeEtudiant() => Session["code"].ToString();

        private void ChargerPaiements()
        {
            lbMsg.Text = "";

            try
            {
                int idEtudiant = GetIdEtudiantFromCode(CodeEtudiant());
             
                string sql = @"
                    SELECT
                        ISNULL(CAST(p.versement AS varchar(50)), '') AS versement,
                        ISNULL(CAST(p.motif AS varchar(100)), '') AS motif,
                        p.datePaiement,
                        ISNULL(p.montantPaye, 0) AS montantPaye,
                        ISNULL(p.balance, p.balance) AS balance
                    FROM dbo.paiement p
                    WHERE p.idEtudiant = @idEtudiant
                    ORDER BY p.datePaiement DESC
                ";

                DataTable dt = new DataTable();
                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(sql, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                    con.Open();
                    da.Fill(dt);
                }

                gvPaiements.DataSource = dt;
                gvPaiements.DataBind();
            }
            catch (Exception ex)
            {
                lbMsg.Text = "Erreur : " + ex.Message;
            }
        }

        private int GetIdEtudiantFromCode(string code)
        {
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"SELECT TOP 1 idEtudiant FROM dbo.etudiant WHERE code = @code", con))
            {
                cmd.Parameters.AddWithValue("@code", code);
                con.Open();

                object o = cmd.ExecuteScalar();
                if (o == null || o == DBNull.Value)
                    throw new Exception("Étudiant introuvable pour le code : " + code);

                return Convert.ToInt32(o);
            }
        }
    }
}