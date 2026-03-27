using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UTMH_Edu.Vue
{
    public partial class FormPayroll : System.Web.UI.Page
    {
        private readonly string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                RemplirMois();
                RemplirAnnees();
                ddlType.SelectedValue = "0";
                ChargerListe();            
            }
          
        }

        // ✅ AJOUT UNIQUEMENT : ShowAlert
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                "document.getElementById('msgText').innerText = '"
                + msg.Replace("'", "\\'") + "';" +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();",
                true);
        }



        private void RemplirMois()
        {
            ddlMois.Items.Clear();
            ddlMois.Items.Add(new ListItem("-- Mois --", "0"));

            string[] mois = { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" };
            for (int i = 0; i < mois.Length; i++)
                ddlMois.Items.Add(new ListItem(mois[i], (i + 1).ToString()));

            ddlMois.SelectedValue = DateTime.Now.Month.ToString();
        }

        private void RemplirAnnees()
        {
            ddlAnnee.Items.Clear();
            ddlAnnee.Items.Add(new ListItem("-- Année --", "0"));

            int an = DateTime.Now.Year;
            for (int y = an - 3; y <= an + 3; y++)
                ddlAnnee.Items.Add(new ListItem(y.ToString(), y.ToString()));

            ddlAnnee.SelectedValue = an.ToString();
        }

        protected void btnRechercher_Click(object sender, EventArgs e) => ChargerListe();

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlType.SelectedValue = "0";
            ddlMois.SelectedValue = DateTime.Now.Month.ToString();
            ddlAnnee.SelectedValue = DateTime.Now.Year.ToString();
            Lb2.Text = "";
            ChargerListe();
        }

        private void ChargerListe()
        {
            int mois = int.Parse(ddlMois.SelectedValue);
            int annee = int.Parse(ddlAnnee.SelectedValue);
            string type = ddlType.SelectedValue;

            if (mois == 0 || annee == 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "Veuillez sélectionner le mois et l'année.";
                return;
            }

            string sql = @"
;WITH Base AS
(
    SELECT 'PROF' AS typePersonne, pr.idProf AS idPersonne, pr.code, pr.nom, pr.prenom,
           CAST('Professeur' AS varchar(50)) AS roleLibelle
    FROM dbo.professeur pr

    UNION ALL

    SELECT 'ADM' AS typePersonne, ad.idAdm AS idPersonne, ad.code, ad.nom, ad.prenom,
           ISNULL(ad.role,'Administration') AS roleLibelle
    FROM dbo.administrateur ad
)
SELECT
    b.typePersonne,
    b.idPersonne,
    b.code,
    b.nom,
    b.prenom,
    b.roleLibelle,
    ISNULL(s.montantMensuel, 0) AS salaire,

    p.idPayroll,
    ISNULL(p.statut,'') AS statut,
    p.datePayroll AS datePayroll,
    p.datePaiement

FROM Base b
LEFT JOIN dbo.salaire s
    ON s.typePersonne = b.typePersonne
   AND (
        s.idPersonne = b.idPersonne
        OR (b.typePersonne='ADM'  AND s.idAdm  = b.idPersonne)
        OR (b.typePersonne='PROF' AND s.idProf = b.idPersonne)
   )
   AND (ISNULL(s.actif,'1')='1' OR UPPER(ISNULL(s.actif,'')) IN ('OUI','TRUE'))


LEFT JOIN dbo.payroll p
    ON p.typePersonne = b.typePersonne
   AND p.mois = @mois
   AND p.annee = @annee
   AND (
        (b.typePersonne='ADM'  AND p.idAdm = b.idPersonne)
     OR (b.typePersonne='PROF' AND p.idProf= b.idPersonne)
   )

WHERE (@type='0' OR b.typePersonne=@type)
ORDER BY b.nom, b.prenom;";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@mois", SqlDbType.Int).Value = mois;
                cmd.Parameters.Add("@annee", SqlDbType.Int).Value = annee;
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = type;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();

                Lb2.Text = dt.Rows.Count == 0 ? "Aucun résultat." : "";
            }
            bool disablePayroll = false;

            // 1️⃣ Si mwa futur
            if (EstMoisFutur(mois, annee))
            {
                disablePayroll = true;
                Lb2.Text = "⚠️ Vous ne pouvez pas effectuer le payroll pour un mois futur.";

            }
            else
            {
                // 2️⃣ Si payroll deja fèt
                if (PayrollExistePourMois(mois, annee))
                {
                    disablePayroll = true;
                    Lb2.Text = "ℹ️ Le payroll a déjà été effectué pour ce mois.";

                }
            }

            // Applique disabled sou bouton
            btnEffectuerTous.Enabled = !disablePayroll;

            // Applique disabled sou bouton
            if (disablePayroll)
            {
                btnEffectuerTous.Enabled = false;

                btnEffectuerTous.CssClass =
                    "btn btn-secondary w-100 disabled";

                btnEffectuerTous.OnClientClick = "return false;";
            }
            else
            {
                btnEffectuerTous.Enabled = true;

                btnEffectuerTous.CssClass =
                    "btn btn-success w-100";

                btnEffectuerTous.OnClientClick =
                    "return confirm('Confirmer l’exécution du payroll pour tous ?');";
            }


        }

        protected void btnEffectuerTous_Click(object sender, EventArgs e)
        {
            int mois = int.Parse(ddlMois.SelectedValue);
            int annee = int.Parse(ddlAnnee.SelectedValue);
            string type = ddlType.SelectedValue;

            if (mois == 0 || annee == 0)
            {
                Lb2.Text = "Veuillez sélectionner le mois et l'année.";
                return;
            }

            try
            {
                int total = EffectuerPayrollPourToutLeMonde(mois, annee, type);
                Lb2.Text = $"Payroll effectué avec succès. Total créé : {total}.";
                ChargerListe();
            }
            catch (Exception ex)
            {
                Lb2.Text = "Erreur : " + ex.Message;
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (index < 0 || index >= GridView1.Rows.Count) return;

            string type = GridView1.DataKeys[index].Values["typePersonne"].ToString();
            int idPersonne = Convert.ToInt32(GridView1.DataKeys[index].Values["idPersonne"]);

            int mois = int.Parse(ddlMois.SelectedValue);
            int annee = int.Parse(ddlAnnee.SelectedValue);

            try
            {
                if (e.CommandName == "EffectuerPayroll")
                {
                   
                    Lb2.Text = "Payroll généré avec succès (EN ATTENTE).";
                    ChargerListe();
                    return;
                }

                if (e.CommandName == "OpenModal")
                {
                    string nom = GridView1.Rows[index].Cells[1].Text;
                    string prenom = GridView1.Rows[index].Cells[2].Text;
                    string roleLib = GridView1.Rows[index].Cells[3].Text;

                    DataTable dt = GetPayrollHistorique(type, idPersonne, mois, annee);

                    if (dt.Rows.Count == 0)
                    {
                        ShowAlert("⚠️ Aucun payroll disponible pour cette personne.");
                        return;
                    }

                    // 🔥 verifye si gen EN_ATTENTE
                    bool genImpayes = false;

                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["statut"].ToString().ToUpper() != "PAYE")
                        {
                            genImpayes = true;
                            break;
                        }
                    }

                    if (!genImpayes)
                    {
                        ShowAlert("✅ Tous les mois ont déjà été payés. Aucun paiement en attente.");
                        return;
                    }

                    // Si gen impayés → ouvri modal
                    lblNomModal.Text = nom + " " + prenom;
                    lblRoleModal.Text = roleLib;

                    hfType.Value = type;
                    hfIdPersonne.Value = idPersonne.ToString();

                    rptPayrollMois.DataSource = dt;
                    rptPayrollMois.DataBind();

                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "openModal", "openPayModal();", true);

                    return;
                }



            }
            catch (Exception ex)
            {
                Lb2.Text = "Erreur : " + ex.Message;
            }
        }
        private bool EstMoisFutur(int mois, int annee)
        {
            DateTime maintenant = DateTime.Now;
            DateTime selected = new DateTime(annee, mois, 1);

            DateTime current = new DateTime(maintenant.Year, maintenant.Month, 1);

            return selected > current;
        }


        private bool PayrollExistePourMois(int mois, int annee)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT COUNT(1)
FROM dbo.payroll
WHERE mois=@mois AND annee=@annee;", con))
            {
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@annee", annee);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }




        protected void btnConfirmerModal_Click(object sender, EventArgs e)
        {
            string ids = hfSelectedPayrollIds.Value;

            if (string.IsNullOrEmpty(ids))
            {
                lblModalErr.Text = "Veuillez sélectionner au minimum un mois à valider.";
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "reopen", "openPayModal();", true);
                return;
            }

            try
            {
                int updated = ValiderPlusieursPayroll(ids);

                ShowAlert($"Paiement validé avec succès pour {updated} mois.");

                ChargerListe();
            }
            catch (Exception ex)
            {
                lblModalErr.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "reopen", "openPayModal();", true);
            }
        }

        private int ValiderPlusieursPayroll(string ids)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
UPDATE dbo.payroll
SET statut='PAYE',
    datePaiement=GETDATE()
WHERE idPayroll IN (" + ids + @")
AND ISNULL(statut,'') <> 'PAYE';

SELECT @@ROWCOUNT;", con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private DataTable GetPayrollHistorique(string type, int idPersonne, int mois, int annee)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SET LANGUAGE French;
SELECT idPayroll,
       mois,
       annee,
       montant,
       statut,
       DATENAME(MONTH, DATEFROMPARTS(annee, mois, 1)) 
       + ' ' + CAST(annee AS varchar) AS libMois
FROM dbo.payroll
WHERE typePersonne=@type
  AND (
        (@type='ADM' AND idAdm=@id)
     OR (@type='PROF' AND idProf=@id)
      )
  AND (annee < @annee OR (annee=@annee AND mois <= @mois))
ORDER BY annee ASC, mois ASC;", con))
            {
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", idPersonne);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@annee", annee);

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);

                return dt;
            }
        }

        private decimal GetSalaire(string type, int idPersonne)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1 montantMensuel
FROM dbo.salaire
WHERE typePersonne=@type
  AND (
        (idPersonne=@id)
     OR (@type='ADM'  AND idAdm=@id)
     OR (@type='PROF' AND idProf=@id)
  )
  AND (ISNULL(actif,'1')='1' OR UPPER(ISNULL(actif,'')) IN ('OUI','TRUE'));
", con))
            {
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", idPersonne);
                con.Open();

                object r = cmd.ExecuteScalar();
                if (r == null || r == DBNull.Value) throw new Exception("Salaire non défini.");
                return Convert.ToDecimal(r);
            }
        }


       


        private decimal LireSalaireActif(SqlConnection con, SqlTransaction tr, string type, int idPersonne)
        {
            using (SqlCommand cmdSal = new SqlCommand(@"
SELECT TOP 1 montantMensuel
FROM dbo.salaire
WHERE typePersonne=@type AND idPersonne=@id
  AND (ISNULL(actif,'1')='1' OR UPPER(ISNULL(actif,'')) IN ('OUI','TRUE'));", con, tr))
            {
                cmdSal.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = type;
                cmdSal.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;

                object r = cmdSal.ExecuteScalar();
                if (r == null || r == DBNull.Value)
                    throw new Exception("Salaire non défini ou inactif pour cette personne.");

                decimal salaire = Convert.ToDecimal(r);
                if (salaire <= 0) throw new Exception("Salaire invalide.");
                return salaire;
            }
        }

        private void ValiderPaiement(int idPayroll)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
UPDATE dbo.payroll
SET statut='PAYE',
    datePaiement = GETDATE()
WHERE idPayroll=@idPayroll
  AND ISNULL(statut,'') <> 'PAYE';", con))
            {
                cmd.Parameters.Add("@idPayroll", SqlDbType.Int).Value = idPayroll;

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    throw new Exception("Ce payroll est déjà payé ou introuvable.");
            }
        }

        private int EffectuerPayrollPourToutLeMonde(int mois, int annee, string typeFiltre)
        {
            int created = 0;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlTransaction tr = con.BeginTransaction())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        using (SqlCommand cmd = new SqlCommand(@"
;WITH Base AS
(
    SELECT 'PROF' AS typePersonne, pr.idProf AS idPersonne
    FROM dbo.professeur pr
    UNION ALL
    SELECT 'ADM' AS typePersonne, ad.idAdm AS idPersonne
    FROM dbo.administrateur ad
)
SELECT b.typePersonne, b.idPersonne, s.montantMensuel
FROM Base b
INNER JOIN dbo.salaire s
    ON s.typePersonne=b.typePersonne
   AND (
        s.idPersonne = b.idPersonne
        OR (b.typePersonne='ADM'  AND s.idAdm  = b.idPersonne)
        OR (b.typePersonne='PROF' AND s.idProf = b.idPersonne)
   )
   AND (ISNULL(s.actif,'1')='1' OR UPPER(ISNULL(s.actif,'')) IN ('OUI','TRUE'))
WHERE (@type='0' OR b.typePersonne=@type);", con, tr))
                        {
                            cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = typeFiltre;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                da.Fill(dt);
                        }


                        foreach (DataRow r in dt.Rows)
                        {
                            string type = r["typePersonne"].ToString();
                            int idPersonne = Convert.ToInt32(r["idPersonne"]);
                            decimal sal = Convert.ToDecimal(r["montantMensuel"]);

                            if (!PayrollExiste(con, tr, type, idPersonne, mois, annee))
                            {
                                InsererPayrollEnAttente(con, tr, type, idPersonne, mois, annee, sal);
                                created++;
                            }
                        }

                        tr.Commit();
                        return created;
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        private bool PayrollExiste(SqlConnection con, SqlTransaction tr, string type, int idPersonne, int mois, int annee)
        {
            using (SqlCommand cmd = new SqlCommand(@"
SELECT COUNT(1)
FROM dbo.payroll
WHERE typePersonne=@type
  AND mois=@mois AND annee=@annee
  AND (
        (@type='ADM'  AND idAdm=@id)
     OR (@type='PROF' AND idProf=@id)
  );", con, tr))
            {
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = type;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;
                cmd.Parameters.Add("@mois", SqlDbType.Int).Value = mois;
                cmd.Parameters.Add("@annee", SqlDbType.Int).Value = annee;

                int c = Convert.ToInt32(cmd.ExecuteScalar());
                return c > 0;
            }
        }


        private bool PayrollExisteJusqua(string type, int idPersonne, int mois, int annee)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT COUNT(1)
FROM dbo.payroll
WHERE typePersonne=@type
  AND (
        (@type='ADM'  AND idAdm=@id)
     OR (@type='PROF' AND idProf=@id)
  )
  AND (annee < @annee OR (annee=@annee AND mois <= @mois));", con))
            {
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", idPersonne);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@annee", annee);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private bool ADesMoisImpayes(string type, int idPersonne, int mois, int annee)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT COUNT(1)
FROM dbo.payroll
WHERE typePersonne=@type
  AND (
        (@type='ADM'  AND idAdm=@id)
     OR (@type='PROF' AND idProf=@id)
  )
  AND ISNULL(statut,'') <> 'PAYE'
  AND (annee < @annee OR (annee=@annee AND mois <= @mois));", con))
            {
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@id", idPersonne);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@annee", annee);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }


        private void InsererPayrollEnAttente(SqlConnection con, SqlTransaction tr, string type, int idPersonne, int mois, int annee, decimal montant)
        {
            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.payroll(typePersonne,idAdm,idProf,mois,annee,montant,statut,dateCreation,datePayroll)
VALUES(
 @type,
 CASE WHEN @type='ADM' THEN @id ELSE NULL END,
 CASE WHEN @type='PROF' THEN @id ELSE NULL END,
 @mois,@annee,@montant,'EN_ATTENTE',GETDATE(),GETDATE());", con, tr))
            {
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = type;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;
                cmd.Parameters.Add("@mois", SqlDbType.Int).Value = mois;
                cmd.Parameters.Add("@annee", SqlDbType.Int).Value = annee;

                cmd.Parameters.Add("@montant", SqlDbType.Decimal).Value = montant;
                cmd.Parameters["@montant"].Precision = 18;
                cmd.Parameters["@montant"].Scale = 2;

                cmd.ExecuteNonQuery();
            }
        }
    }
}