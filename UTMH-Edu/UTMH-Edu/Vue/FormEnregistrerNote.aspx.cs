using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UTMH_Edu.Vue
{
    public partial class FormEnregistrerNote : Page
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {          

            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                ChargerAnnees();
                ChargerOptionsProf();
                ViderGrille();
            }
        }

        private string CodeProf()
        {
            return Convert.ToString(Session["code"] ?? "").Trim();
        }

        // =====================================================
        // LOADERS
        // =====================================================
        private void ChargerAnnees()
        {
            ddlAnneeAcademique.Items.Clear();

            int y = DateTime.Now.Year;
            for (int i = 0; i < 6; i++)
            {
                string an = (y - i).ToString();
                ddlAnneeAcademique.Items.Add(new ListItem(an, an));
            }

            if (ddlAnneeAcademique.Items.FindByValue(y.ToString()) != null)
                ddlAnneeAcademique.SelectedValue = y.ToString();
        }

        private void ChargerOptionsProf()
        {
            ddlOption.Items.Clear();
            ddlOption.Items.Add(new ListItem("-- Option --", ""));

            int idProf = GetIdProfFromCode(CodeProf());

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT DISTINCT o.idOption, o.nom
                FROM dbo.cours c
                INNER JOIN dbo.optionChoisieCours oc ON oc.idCours = c.idCours
                INNER JOIN dbo.optionChoisie o ON o.idOption = oc.idOption
                WHERE c.idProf = @idProf
                ORDER BY o.nom ASC;
            ", con))
            {
                cmd.Parameters.Add("@idProf", SqlDbType.Int).Value = idProf;

                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        ddlOption.Items.Add(new ListItem(
                            r["nom"].ToString(),
                            r["idOption"].ToString()
                        ));
                    }
                }
            }
        }

        private void ChargerEtudiants()
        {
            lbMsg.Text = "";

            int idOption;
            if (!int.TryParse(ddlOption.SelectedValue, out idOption) || idOption <= 0)
            {
                ViderGrille();
                return;
            }

            int idProf = GetIdProfFromCode(CodeProf());

            int idCours;
            string nomCours;

            if (!GetCoursProfPourOption(idProf, idOption, out idCours, out nomCours))
            {
                ViderGrille();
                lbMsg.Text = "<span class='text-danger'>Aucun cours trouvé pour ce professeur dans cette option.</span>";
                return;
            }

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT
                    e.idEtudiant,
                    @idCours AS idCours,
                    @nomCours AS NomCours,
                    (LTRIM(RTRIM(ISNULL(e.nom, ''))) + ' ' + LTRIM(RTRIM(ISNULL(e.prenom, '')))) AS NomEtudiant
                FROM dbo.etudiant e
                WHERE e.idOption = @idOption
                ORDER BY e.nom ASC, e.prenom ASC;
            ", con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@idCours", SqlDbType.Int).Value = idCours;
                cmd.Parameters.Add("@nomCours", SqlDbType.NVarChar, 150).Value = nomCours;
                cmd.Parameters.Add("@idOption", SqlDbType.Int).Value = idOption;

                con.Open();
                da.Fill(dt);
            }

            gvNotes.DataSource = dt;
            gvNotes.DataBind();

            if (dt.Rows.Count == 0)
            {
                lbMsg.Text = "<span class='text-warning'>Aucun étudiant trouvé dans cette option.</span>";
            }
        }

        private void ViderGrille()
        {
            gvNotes.DataSource = null;
            gvNotes.DataBind();
        }

        // =====================================================
        // EVENTS
        // =====================================================
        protected void ddlAnneeAcademique_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerEtudiants();
        }

        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerEtudiants();
        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            if (ddlOption.Items.Count > 0)
                ddlOption.SelectedIndex = 0;

            lbMsg.Text = "";
            ViderGrille();
        }

        protected void gvNotes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EnregistrerNote")
                return;

            int rowIndex;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out rowIndex) ||
                rowIndex < 0 ||
                rowIndex >= gvNotes.Rows.Count)
            {
                lbMsg.Text = "<span class='text-danger'>Ligne invalide.</span>";
                return;
            }

            GridViewRow row = gvNotes.Rows[rowIndex];

            int idEtudiant = Convert.ToInt32(gvNotes.DataKeys[rowIndex].Values["idEtudiant"]);
            int idCours = Convert.ToInt32(gvNotes.DataKeys[rowIndex].Values["idCours"]);

            if (!CoursAppartientAuProf(idCours))
            {
                lbMsg.Text = "<span class='text-danger'>Vous ne pouvez enregistrer des notes que pour vos propres cours.</span>";
                return;
            }

            TextBox txtNote = row.FindControl("txtNote") as TextBox;
            DropDownList ddlType = row.FindControl("ddlTypeNote") as DropDownList;
            TextBox txtLib = row.FindControl("txtLibelleNote") as TextBox;

            string noteStr = (txtNote != null ? txtNote.Text : "").Trim();
            string typeNote = (ddlType != null ? ddlType.SelectedValue : "").Trim().ToUpperInvariant();
            string libelle = (txtLib != null ? txtLib.Text : "").Trim();

            if (string.IsNullOrWhiteSpace(noteStr))
            {
                lbMsg.Text = "<span class='text-danger'>Veuillez saisir la note.</span>";
                return;
            }

            decimal note;
            if (!decimal.TryParse(noteStr, NumberStyles.Any, CultureInfo.InvariantCulture, out note))
            {
                if (!decimal.TryParse(noteStr, NumberStyles.Any, new CultureInfo("fr-FR"), out note))
                {
                    lbMsg.Text = "<span class='text-danger'>Veuillez saisir une note valide.</span>";
                    return;
                }
            }

            if (note < 0 || note > 100)
            {
                lbMsg.Text = "<span class='text-danger'>La note doit être entre 0 et 100.</span>";
                return;
            }

            if (string.IsNullOrWhiteSpace(typeNote))
            {
                lbMsg.Text = "<span class='text-danger'>Veuillez choisir le type (Examen / Devoir).</span>";
                return;
            }

            if (string.IsNullOrWhiteSpace(libelle))
            {
                lbMsg.Text = "<span class='text-danger'>Veuillez saisir le libellé.</span>";
                return;
            }

            try
            {
                EnregistrerNote(idEtudiant, idCours, note, typeNote, libelle);

                lbMsg.Text = "<span class='text-success fw-bold'>Note enregistrée avec succès ✅</span>";

                if (txtNote != null) txtNote.Text = "";
                if (ddlType != null) ddlType.SelectedIndex = 0;
                if (txtLib != null) txtLib.Text = "";
            }
            catch (Exception ex)
            {
                lbMsg.Text = "<span class='text-danger'>Erreur : " + Server.HtmlEncode(ex.Message) + "</span>";
            }
        }

        // =====================================================
        // SAVE NOTE
        // =====================================================
        private void EnregistrerNote(int idEtudiant, int idCours, decimal note, string typeNote, string libelleNote )
        {
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
IF EXISTS (
    SELECT 1
    FROM dbo.note
    WHERE idEtudiant = @idEtudiant
      AND idCours = @idCours
      AND UPPER(LTRIM(RTRIM(ISNULL(typeNote,'')))) = @typeNote
      AND LTRIM(RTRIM(ISNULL(libelleNote,''))) = @libelle
      AND CAST(dateNote AS date) = CAST(GETDATE() AS date)
)
BEGIN
    UPDATE dbo.note
    SET noteObtenue = @noteObtenue,
        dateNote = GETDATE()
    WHERE idEtudiant = @idEtudiant
      AND idCours = @idCours
      AND UPPER(LTRIM(RTRIM(ISNULL(typeNote,'')))) = @typeNote
      AND LTRIM(RTRIM(ISNULL(libelleNote,''))) = @libelle
      AND CAST(dateNote AS date) = CAST(GETDATE() AS date)
END
ELSE
BEGIN
    INSERT INTO dbo.note (idEtudiant, idCours, noteObtenue, typeNote, libelleNote, dateNote)
    VALUES (@idEtudiant, @idCours, @noteObtenue, @typeNote, @libelle, GETDATE())
END
", con))
            {
                cmd.Parameters.Add("@idEtudiant", SqlDbType.Int).Value = idEtudiant;
                cmd.Parameters.Add("@idCours", SqlDbType.Int).Value = idCours;

                SqlParameter pNote = cmd.Parameters.Add("@noteObtenue", SqlDbType.Decimal);
                pNote.Precision = 5;
                pNote.Scale = 2;
                pNote.Value = note;

                cmd.Parameters.Add("@typeNote", SqlDbType.VarChar, 30).Value = typeNote;
                cmd.Parameters.Add("@libelle", SqlDbType.NVarChar, 150).Value = libelleNote;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =====================================================
        // HELPERS
        // =====================================================
        private int GetIdProfFromCode(string codeProf)
        {
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 idProf
                FROM dbo.professeur
                WHERE LTRIM(RTRIM(code)) = @code;
            ", con))
            {
                cmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value = codeProf;

                con.Open();
                object o = cmd.ExecuteScalar();

                if (o == null || o == DBNull.Value)
                    throw new Exception("Professeur introuvable.");

                return Convert.ToInt32(o);
            }
        }

        private bool GetCoursProfPourOption(int idProf, int idOption, out int idCours, out string nomCours)
        {
            idCours = 0;
            nomCours = "";

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 c.idCours, c.nom
                FROM dbo.cours c
                INNER JOIN dbo.optionChoisieCours oc ON oc.idCours = c.idCours
                WHERE c.idProf = @idProf
                  AND oc.idOption = @idOption
                ORDER BY c.nom ASC;
            ", con))
            {
                cmd.Parameters.Add("@idProf", SqlDbType.Int).Value = idProf;
                cmd.Parameters.Add("@idOption", SqlDbType.Int).Value = idOption;

                con.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        idCours = Convert.ToInt32(r["idCours"]);
                        nomCours = r["nom"].ToString();
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CoursAppartientAuProf(int idCours)
        {
            int idProf = GetIdProfFromCode(CodeProf());

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM dbo.cours
                WHERE idCours = @idCours
                  AND idProf = @idProf;
            ", con))
            {
                cmd.Parameters.Add("@idCours", SqlDbType.Int).Value = idCours;
                cmd.Parameters.Add("@idProf", SqlDbType.Int).Value = idProf;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}