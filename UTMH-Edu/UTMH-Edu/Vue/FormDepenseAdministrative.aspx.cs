using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;

namespace UTMH_Edu.Vue
{
    public partial class FormDepenseAdministrative : System.Web.UI.Page
    {
        private readonly ControlleurDepenseAdministrative ctrl = new ControlleurDepenseAdministrative();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                // Date par défaut
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ChargerListeFiltre();
            }
        }

        // =========================
        // ENREGISTRER (Ajout/Modification)
        // =========================
        protected void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                ShowMsg("");

                string motif = (txtMotif.Text ?? "").Trim();
                string description = (txtDescription.Text ?? "").Trim();
                string mode = (ddlModePaiement.SelectedValue ?? "").Trim();

                if (string.IsNullOrWhiteSpace(motif))
                    throw new Exception("Le motif est obligatoire.");

                // ✅ C# ancien: déclarer avant le TryParse
                DateTime dateDepense;
                if (!DateTime.TryParse(txtDate.Text, out dateDepense))
                    throw new Exception("Date invalide.");

                int quantite;
                if (!int.TryParse((txtQuantite.Text ?? "").Trim(), out quantite) || quantite <= 0)
                    throw new Exception("Quantité invalide.");

                if (string.IsNullOrWhiteSpace(mode))
                    throw new Exception("Veuillez sélectionner un mode de paiement.");

                decimal montant = ParseMontant(txtMontant.Text);
                if (montant <= 0)
                    throw new Exception("Le montant doit être supérieur à zéro.");

                int idAdm = GetIdAdm(); // adapte selon ton login/session
                int idDepense = 0;
                int.TryParse(hfIdDepense.Value, out idDepense);

                if (idDepense <= 0)
                {
                    ctrl.Ajouter(idAdm, motif, description, dateDepense, quantite, mode, montant);
                    ShowMsg("Dépense enregistrée avec succès.", false);
                }
                else
                {
                    ctrl.Modifier(idDepense, idAdm, motif, description, dateDepense, quantite, mode, montant);
                    ShowMsg("Dépense modifiée avec succès.", false);
                }

                ResetForm();
                ChargerListeFiltre();
            }
            catch (Exception ex)
            {
                ShowMsg("Erreur : " + ex.Message, true);
            }
        }

        // =========================
        // Charger (Filtre)
        // =========================
        protected void btnCharger_Click(object sender, EventArgs e)
        {
            try
            {
                ShowMsg("");
                ChargerListeFiltre();
            }
            catch (Exception ex)
            {
                ShowMsg("Erreur : " + ex.Message, true);
            }
        }

        protected void btnResetFiltre_Click(object sender, EventArgs e)
        {
            txtDu.Text = "";
            txtAu.Text = "";
            ShowMsg("");
            ChargerListeFiltre();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            ShowMsg("");
        }

        // =========================
        // Actions Grid
        // =========================
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ShowMsg("");

                int index = Convert.ToInt32(e.CommandArgument);
                if (index < 0 || index >= GridView1.Rows.Count) return;

                int idDepense = Convert.ToInt32(GridView1.DataKeys[index].Value);

                if (e.CommandName == "EDITER")
                {
                    DataRow row = ctrl.Lire(idDepense);
                    if (row == null) throw new Exception("Dépense introuvable.");

                    hfIdDepense.Value = idDepense.ToString();
                    txtMotif.Text = row["motif"].ToString();
                    txtDescription.Text = row["description"] == DBNull.Value ? "" : row["description"].ToString();

                    DateTime d = Convert.ToDateTime(row["dateDepense"]);
                    txtDate.Text = d.ToString("yyyy-MM-dd");

                    txtQuantite.Text = row["quantite"].ToString();
                    ddlModePaiement.SelectedValue = row["modePaiement"].ToString();

                    decimal m = Convert.ToDecimal(row["montant"]);
                    txtMontant.Text = m.ToString("0.##", CultureInfo.InvariantCulture);

                    ShowMsg("Mode modification activé. Modifiez puis cliquez « Enregistrer ».", false);
                    return;
                }

                if (e.CommandName == "SUPPRIMER")
                {
                    ctrl.Supprimer(idDepense);
                    ShowMsg("Dépense supprimée avec succès.", false);
                    ChargerListeFiltre();
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMsg("Erreur : " + ex.Message, true);
            }
        }

        // =========================
        // Helpers
        // =========================
        private void ChargerListeFiltre()
        {
            DateTime? du = ParseDateNullable(txtDu.Text);
            DateTime? au = ParseDateNullable(txtAu.Text);

            DataTable dt = ctrl.Lister(du, au);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private decimal ParseMontant(string input)
        {
            string s = (input ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s)) return 0m;

            s = s.Replace(" ", "");

            decimal v;

            // 1) culture actuelle
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out v))
                return v;

            // 2) invariant
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                return v;

            // 3) swap virgule -> point
            s = s.Replace(",", ".");
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
                return v;

            throw new Exception("Montant invalide.");
        }

        private DateTime? ParseDateNullable(string input)
        {
            string s = (input ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s)) return null;

            DateTime d;
            if (DateTime.TryParse(s, out d))
                return d.Date;

            return null;
        }

        private void ResetForm()
        {
            hfIdDepense.Value = "0";
            txtMotif.Text = "";
            txtDescription.Text = "";
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtQuantite.Text = "1";
            ddlModePaiement.SelectedIndex = 0;
            txtMontant.Text = "";
        }

        private void ShowMsg(string msg, bool isError)
        {
            Lb2.Text = msg ?? "";
            Lb2.CssClass = isError
                ? "text-danger fw-bold d-block mt-2"
                : "text-success fw-bold d-block mt-2";
        }

        private void ShowMsg(string msg)
        {
            ShowMsg(msg, true);
        }


        private int GetIdAdm()
        {

            return 1;
        }
    }
}