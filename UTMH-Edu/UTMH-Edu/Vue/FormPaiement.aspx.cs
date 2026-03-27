using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Vue
{
    public partial class FormPaiement : System.Web.UI.Page
    {
        private static readonly string strCon =
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
                RemplirAnneeAcademique();

                if (Request.QueryString["code"] == null)
                {
                    Response.Redirect("FormListeEtudiant.aspx");
                    return;
                }

                ChargerEtudiant(Request.QueryString["code"]);

                txtDatePaiement.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtBalance.ReadOnly = true;
                txtMontantAPayer.ReadOnly = true;

                // Mete montant a peye selon motif si deja chwazi
                ddlMotif_SelectedIndexChanged(null, null);

                // Montre resume actuel
                ChargerResumeActuel();
            }
        }

        // ==========================
        // MODAL ALERT
        // ==========================
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}';" +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();",
                true);
        }

        // ==========================
        // LOAD OPTIONS
        // ==========================


        void RemplirAnneeAcademique()
        {
            ddlAnneeAcademique.Items.Clear();

            int annee = DateTime.Now.Year;
            int mois = DateTime.Now.Month;

            int debut;
            int fin;

            // Si on est avant septembre
            if (mois < 9)
            {
                debut = annee - 1;
                fin = annee;
            }
            else
            {
                debut = annee;
                fin = annee + 1;
            }

            string anneeAcademique = debut + "/" + fin;

            ddlAnneeAcademique.Items.Add(
                new System.Web.UI.WebControls.ListItem(anneeAcademique, anneeAcademique)
            );

            ddlAnneeAcademique.SelectedIndex = 0;
        }

        // ==========================
        // LOAD ETUDIANT
        // ==========================
        private void ChargerEtudiant(string codeEtudiant)
        {
            string query = @"
                SELECT idEtudiant, idOption, code, nom, prenom, anneeAcademique
                FROM dbo.etudiant
                WHERE code = @code";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@code", codeEtudiant);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        ShowAlert("Étudiant introuvable.");
                        Response.Redirect("FormListeEtudiant.aspx");
                        return;
                    }

                    ViewState["idEtudiant"] = Convert.ToInt32(dr["idEtudiant"]);

               
                    txtNom.Text = dr["nom"].ToString();
                    txtPrenom.Text = dr["prenom"].ToString();          

                    string annee = dr["anneeAcademique"].ToString();
                    if (ddlAnneeAcademique.Items.FindByValue(annee) != null)
                        ddlAnneeAcademique.SelectedValue = annee;
                }
            }
        }

        private int GetIdEtudiant()
        {
            if (ViewState["idEtudiant"] == null)
                throw new Exception("IdEtudiant manquant.");
            return Convert.ToInt32(ViewState["idEtudiant"]);
        }

        // ==========================
        // RULES: Motif -> Montant dû
        // ==========================
        private decimal GetMontantDu(string motif)
        {
            string m = (motif ?? "").Trim().ToUpperInvariant();

            if (m == "ANNEE_ACADEMIQUE" || m == "ANNEE ACADEMIQUE" || m == "ANNÉE ACADÉMIQUE") return 20000m;
            if (m == "GRADUATION") return 25000m;

            // Ajoute pri fiks si ou vle:
            // if (m == "STAGE") return 15000m;

            return 0m;
        }

        // ==========================
        // DB: Total payé cumulé (par étudiant + motif)
        // ==========================
        private decimal GetTotalPaye(int idEtudiant, string motif)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(SUM(montantPaye),0)
                FROM dbo.Paiement
                WHERE idEtudiant=@id AND motif=@motif;", con))
            {
                cmd.Parameters.AddWithValue("@id", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);
                con.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        // ==========================
        // DB: Next versement (MAX+1)
        // ==========================
        private int GetNextVersement(int idEtudiant, string motif)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(MAX(versement),0) + 1
                FROM dbo.Paiement
                WHERE idEtudiant=@id AND motif=@motif;", con))
            {
                cmd.Parameters.AddWithValue("@id", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ==========================
        // EVENT: Motif change
        // ==========================
        protected void ddlMotif_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal montantDu = GetMontantDu(ddlMotif.SelectedValue);

            txtMontantAPayer.Text = montantDu > 0 ? montantDu.ToString("0") : "";
            txtMontantDu_TextChanged(null, null);
            ChargerResumeActuel();
        }

       
        // ==========================
        // UI PREVIEW: Montant payé change
        // ✅ règle: montant payé ne peut pas dépasser montant à payer
        // ✅ balance = montant à payer - (total déjà payé + montant payé)
        // ==========================
        protected void txtMontantDu_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMotif.SelectedValue))
            {
                txtBalance.Text = "0";
                return;
            }

            decimal montantDu = GetMontantDu(ddlMotif.SelectedValue);
            if (montantDu <= 0)
            {
                txtBalance.Text = "0";
                return;
            }

            decimal montantPaye;
            if (!decimal.TryParse(txtMontantDu.Text.Trim(), out montantPaye))
            {
                txtBalance.Text = "0";
                return;
            }

            if (montantPaye <= 0)
            {
                txtBalance.Text = "0";
                return;
            }

            int idEtudiant = GetIdEtudiant();
            decimal totalDeja = GetTotalPaye(idEtudiant, ddlMotif.SelectedValue);

            // ✅ règle: si montant payé (nouveau) depase reste a payer => erreur
            decimal resteAvant = montantDu - totalDeja;
            if (montantPaye > resteAvant)
            {
                txtBalance.Text = "";
                txtMontantDu.Text = "";
                ShowAlert($"❌ Montant payé trop élevé. Reste à payer : {resteAvant:0}.");
                return;
            }

            decimal nouveauTotal = totalDeja + montantPaye;

            // ✅ balance = montantDu - nouveauTotal (toujours >=0)
            decimal balance = montantDu - nouveauTotal;
            txtBalance.Text = balance.ToString("0");
        }

        // ==========================
        // LOAD RESUME ACTUEL
        // balance = montantDu - totalDeja
        // ==========================
        private void ChargerResumeActuel()
        {
            if (string.IsNullOrWhiteSpace(ddlMotif.SelectedValue))
            {
                txtBalance.Text = "0";
                return;
            }

            decimal montantDu = GetMontantDu(ddlMotif.SelectedValue);
            if (montantDu <= 0)
            {
                txtBalance.Text = "0";
                return;
            }

            int idEtudiant = GetIdEtudiant();
            decimal totalDeja = GetTotalPaye(idEtudiant, ddlMotif.SelectedValue);

            decimal balance = montantDu - totalDeja;
            if (balance < 0) balance = 0; // sekirite
            txtBalance.Text = balance.ToString("0");
        }

        // ==========================
        // SAVE (INSERT dbo.Paiement)
        // ✅ règle: montant payé ne peut pas dépasser reste à payer
        // ==========================
        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMotif.SelectedValue))
            {
                ShowAlert("Veuillez sélectionner un motif.");
                return;
            }

            string motif = ddlMotif.SelectedValue;

            decimal montantDu = GetMontantDu(motif);
            if (montantDu <= 0)
            {
                ShowAlert("Montant dû non défini pour ce motif.");
                return;
            }

            // 🔴 Vérifier durée valide (1 à 5 heures)
            int montantAPaye;
            if (!int.TryParse(txtMontantAPayer.Text, out montantAPaye) || montantAPaye < 1)
            {
                ShowAlert("Le montant ne doit pas être plus petit que un (1)  !");
                return;
            }
            decimal montantPaye;
            if (!decimal.TryParse(txtMontantDu.Text.Trim(), out montantPaye) || montantPaye <= 0)
            {
                ShowAlert("Le montant payé est invalide.");
                return;
            }

            DateTime datePaiement;
            if (!DateTime.TryParse(txtDatePaiement.Text.Trim(), out datePaiement))
                datePaiement = DateTime.Now;

            int idEtudiant = GetIdEtudiant();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlTransaction tr = con.BeginTransaction())
                {
                    try
                    {
                        // Total deja peye
                        decimal totalDeja = 0;
                        using (SqlCommand cmd = new SqlCommand(@"
                            SELECT ISNULL(SUM(montantPaye),0)
                            FROM dbo.Paiement
                            WHERE idEtudiant=@id AND motif=@motif;", con, tr))
                        {
                            cmd.Parameters.AddWithValue("@id", idEtudiant);
                            cmd.Parameters.AddWithValue("@motif", motif);
                            totalDeja = Convert.ToDecimal(cmd.ExecuteScalar());
                        }

                        // deja solde
                        if (totalDeja >= montantDu)
                        {
                            tr.Rollback();
                            ShowAlert("Paiement déjà soldé.");
                            return;
                        }

                        // Reste avant paiement
                        decimal resteAvant = montantDu - totalDeja;

                        // ✅ règle: si montantPaye > resteAvant => erreur
                        if (montantPaye > resteAvant)
                        {
                            tr.Rollback();
                            ShowAlert($"❌ Montant payé trop élevé. Reste à payer : {resteAvant:0}.");
                            return;
                        }

                        // Next versement
                        int nextVersement = 1;
                        using (SqlCommand cmd = new SqlCommand(@"
                            SELECT ISNULL(MAX(versement),0) + 1
                            FROM dbo.Paiement
                            WHERE idEtudiant=@id AND motif=@motif;", con, tr))
                        {
                            cmd.Parameters.AddWithValue("@id", idEtudiant);
                            cmd.Parameters.AddWithValue("@motif", motif);
                            nextVersement = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        decimal nouveauTotal = totalDeja + montantPaye;

                        // ✅ balance final (toujours >=0)
                        decimal balance = montantDu - nouveauTotal;
                        if (balance < 0) balance = 0;

                        // INSERT
                        using (SqlCommand cmd = new SqlCommand(@"
                            INSERT INTO dbo.Paiement
                            (idEtudiant, datePaiement, moyenPaiement, motif, versement, montantDu, balance, montantPaye)
                            VALUES
                            (@id, @date, @moyen, @motif, @versement, @montantDu, @balance, @montantPaye);", con, tr))
                        {
                            cmd.Parameters.AddWithValue("@id", idEtudiant);
                            cmd.Parameters.AddWithValue("@date", datePaiement);
                            cmd.Parameters.AddWithValue("@moyen", DBNull.Value); // mete ddl si ou gen moyen
                            cmd.Parameters.AddWithValue("@motif", motif);
                            cmd.Parameters.AddWithValue("@versement", nextVersement);
                            cmd.Parameters.AddWithValue("@montantDu", montantDu);
                            cmd.Parameters.AddWithValue("@balance", balance);
                            cmd.Parameters.AddWithValue("@montantPaye", montantPaye);

                            cmd.ExecuteNonQuery();
                        }

                        tr.Commit();

                        // Update UI
                        txtMontantAPayer.Text = montantDu.ToString("0");
                        txtBalance.Text = balance.ToString("0");
                        txtMontantDu.Text = "";

                        if (balance == 0)
                            ShowAlert("Paiement enregistré ✅ | Soldé.");
                        else
                            ShowAlert($"Paiement enregistré ✅ | Reste à payer : {balance:0}.");
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        ShowAlert("Erreur lors de l'enregistrement : " + ex.Message.Replace("'", "\\'"));
                    }
                }
            }

            // Recharge resume
            ChargerResumeActuel();
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormEnregistrerPaiementEtudiant.aspx");
        }
    }
}