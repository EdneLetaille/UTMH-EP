using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace UTMH_Edu.Vue
{
    public partial class FormVoirPaie : Page
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
                ChargerAnnees();
                ChargerPaie();
            }
        }

        private string CodeProf() => Session["code"].ToString();

        // ✅ Chaje dropdown ane
        private void ChargerAnnees()
        {
            ddlAnnee.Items.Clear();

            int anneeActuelle = DateTime.Now.Year;
            for (int y = anneeActuelle; y >= anneeActuelle - 5; y--)
            {
                ddlAnnee.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }

            ddlAnnee.SelectedValue = anneeActuelle.ToString();
        }

        protected void ddlAnnee_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerPaie();
        }

        private void ChargerPaie()
        {
            lbMsg.Text = "";
            lbTotalPaye.Text = "0";
            lbMoisPayes.Text = "0";

            try
            {
                int idProf = GetIdProfFromCode(CodeProf());
                int annee = int.Parse(ddlAnnee.SelectedValue);

                // ✅ 12 mwa yo + LEFT JOIN sou peman pwofesè a
                // ⚠️ Si table/kolòn yo diferan: modifye "dbo.payroll", idProf, mois, annee, montant, datePaiement
                string sql = @"
;WITH Mois AS (
    SELECT 1 AS MoisNum, N'Janvier' AS MoisNom UNION ALL
    SELECT 2, N'Février' UNION ALL
    SELECT 3, N'Mars' UNION ALL
    SELECT 4, N'Avril' UNION ALL
    SELECT 5, N'Mai' UNION ALL
    SELECT 6, N'Juin' UNION ALL
    SELECT 7, N'Juillet' UNION ALL
    SELECT 8, N'Août' UNION ALL
    SELECT 9, N'Septembre' UNION ALL
    SELECT 10, N'Octobre' UNION ALL
    SELECT 11, N'Novembre' UNION ALL
    SELECT 12, N'Décembre'
)
SELECT
    m.MoisNom AS Mois,
    @annee AS Annee,
    ISNULL(p.montant, 0) AS Montant,
    p.datePaiement AS DatePaiement,
    CASE WHEN p.idPayroll IS NULL THEN 'NON PAYÉ' ELSE 'PAYÉ' END AS Statut,
    m.MoisNum
FROM Mois m
LEFT JOIN dbo.payroll p
    ON p.idProf = @idProf
   AND p.annee = @annee
   AND p.mois = m.MoisNum
ORDER BY m.MoisNum;
";

                DataTable dt = new DataTable();
                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(sql, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@idProf", idProf);
                    cmd.Parameters.AddWithValue("@annee", annee);

                    con.Open();
                    da.Fill(dt);
                }

                gvPaie.DataSource = dt;
                gvPaie.DataBind();

                // ✅ stats
                decimal totalPaye = 0;
                int moisPayes = 0;

                foreach (DataRow r in dt.Rows)
                {
                    if (r["Statut"].ToString() == "PAYÉ")
                    {
                        moisPayes++;
                        totalPaye += Convert.ToDecimal(r["Montant"]);
                    }
                }

                lbTotalPaye.Text = totalPaye.ToString("N0", CultureInfo.InvariantCulture);
                lbMoisPayes.Text = moisPayes.ToString();
            }
            catch (Exception ex)
            {
                lbMsg.Text = "Erreur : " + ex.Message;
            }
        }

        private int GetIdProfFromCode(string code)
        {
            // ⚠️ Adapte table professeur si li diferan
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"SELECT TOP 1 idProf FROM dbo.professeur WHERE code = @code", con))
            {
                cmd.Parameters.AddWithValue("@code", code);
                con.Open();

                object o = cmd.ExecuteScalar();
                if (o == null || o == DBNull.Value)
                    throw new Exception("Professeur introuvable pour le code : " + code);

                return Convert.ToInt32(o);
            }
        }

        // ✅ Badge color pou statut
        protected void gvPaie_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string statut = DataBinder.Eval(e.Row.DataItem, "Statut")?.ToString() ?? "";
            var sp = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("spStatut");
            if (sp == null) return;

            if (statut == "PAYÉ")
                sp.Attributes["class"] = "badge bg-success";
            else
                sp.Attributes["class"] = "badge bg-danger";
        }
    }
}