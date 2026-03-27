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
    public partial class FormSalaire : Page
    {
        private readonly string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        private int SelectedIdPersonne
        {
            get { return ViewState["idPersonne"] == null ? 0 : (int)ViewState["idPersonne"]; }
            set { ViewState["idPersonne"] = value; }
        }

        private string SelectedType
        {
            get { return ViewState["type"] == null ? "" : ViewState["type"].ToString(); }
            set { ViewState["type"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "Veuillez sélectionner un type pour afficher la liste.";
                ddlActif.SelectedValue = "1";
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Lb2.Text = "";
            SelectedIdPersonne = 0;
            SelectedType = "";
            txtSalaire.Text = "";
            ddlActif.SelectedValue = "1";

            ChargerListe();
        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            ddlType.SelectedIndex = 0;
            txtSalaire.Text = "";
            ddlActif.SelectedValue = "1";

            SelectedIdPersonne = 0;
            SelectedType = "";

            GridView1.DataSource = null;
            GridView1.DataBind();

            Lb2.Text = "Veuillez sélectionner un type pour afficher la liste.";
        }

        private void ChargerListe()
        {
            if (ddlType.SelectedValue == "0")
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "Veuillez sélectionner un type pour afficher la liste.";
                return;
            }

            string sql;

            if (ddlType.SelectedValue == "PROF")
            {
                sql = @"
SELECT 
    'PROF' AS typePersonne,
    p.idProf AS idPersonne,
    p.code,
    p.nom,
    p.prenom,
    ISNULL(p.role,'') AS role,
    ISNULL(s.montantMensuel,0) AS montantMensuel,
    ISNULL(s.actif,'1') AS actif
FROM dbo.professeur p
LEFT JOIN dbo.salaire s 
    ON s.typePersonne='PROF' AND s.idProf=p.idProf
ORDER BY p.nom, p.prenom;";
            }
            else // ADM
            {
                sql = @"
SELECT 
    'ADM' AS typePersonne,
    a.idAdm AS idPersonne,
    a.code,
    a.nom,
    a.prenom,
    ISNULL(a.role,'') AS role,
    ISNULL(s.montantMensuel,0) AS montantMensuel,
    ISNULL(s.actif,'1') AS actif
FROM dbo.administrateur a
LEFT JOIN dbo.salaire s 
    ON s.typePersonne='ADM' AND s.idAdm=a.idAdm
ORDER BY a.nom, a.prenom;";
            }

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                Lb2.Text = (dt.Rows.Count == 0) ? "Aucun enregistrement trouvé." : "";
            }
        }

        protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditSalaire")
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            SelectedType = GridView1.DataKeys[index].Values["typePersonne"].ToString();
            SelectedIdPersonne = Convert.ToInt32(GridView1.DataKeys[index].Values["idPersonne"]);

            string salaireText = GridView1.Rows[index].Cells[4].Text
                .Replace("&nbsp;", "")
                .Replace(",", "")
                .Trim();

            if (string.IsNullOrWhiteSpace(salaireText))
                salaireText = "0";

            txtSalaire.Text = salaireText;

            string actifText = GridView1.Rows[index].Cells[5].Text.Replace("&nbsp;", "").Trim();
            ddlActif.SelectedValue = (actifText == "0") ? "0" : "1";

            Lb2.Text = "Vous pouvez modifier le salaire puis cliquer sur Enregistrer.";
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            Lb2.Text = "";

            if (ddlType.SelectedValue == "0")
            {
                Lb2.Text = "Veuillez sélectionner le type de personne.";
                return;
            }

            if (SelectedIdPersonne <= 0 || string.IsNullOrWhiteSpace(SelectedType))
            {
                Lb2.Text = "Veuillez cliquer sur 'Modifier' sur la personne dans la liste.";
                return;
            }

            decimal salaire;
            if (!decimal.TryParse(txtSalaire.Text.Trim(), out salaire) || salaire <= 0)
            {
                Lb2.Text = "Salaire invalide.";
                return;
            }

            bool actifBool = (ddlActif.SelectedValue == "1");

            try
            {
                // Utilise ton model déjà mis à jour
                UTMH_Edu.Model.Salaire.UpsertSalaire(
                    SelectedType,
                    SelectedIdPersonne,
                    salaire,
                    actifBool ? "1" : "0"
                );

                ChargerListe();
                Lb2.Text = "Salaire enregistré avec succès.";
            }
            catch (SqlException ex)
            {
                Lb2.Text = "Erreur SQL : " + ex.Message;
            }
            catch (Exception ex)
            {
                Lb2.Text = "Erreur : " + ex.Message;
            }
        }
    }
}