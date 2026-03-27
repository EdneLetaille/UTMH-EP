using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UTMH_Edu.Vue
{
    public partial class FormBulletinEtudiant : Page
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // Moyenne pondérée
        private decimal _sumNoteCoeff = 0; // Σ(note*coeff)
        private decimal _sumCoeff = 0;     // Σ(coeff)

        // Total (somme de toutes les notes)
        private decimal _totalNotes = 0;   // Σ(note)
        private int _countNotes = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["code"] == null)
            {
                Response.Redirect("FormConnexion.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ChargerInfosEtudiant();
                ChargerBulletin();
            }
        }

        private string CodeEtudiant() => Session["code"].ToString();

        // ==========================
        // INFOS ETUDIANT (Nom/Prénom/Option)
        // ==========================
        private void ChargerInfosEtudiant()
        {
            int idEtudiant = GetIdEtudiantFromCode(CodeEtudiant());

            lbCodeEtudiant.Text = CodeEtudiant();

            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                SELECT TOP 1 e.nom, e.prenom, e.idOption
                FROM dbo.etudiant e
                WHERE e.idEtudiant = @idEtudiant
            ", con))
            {
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                con.Open();

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        string nom = r["nom"]?.ToString() ?? "";
                        string prenom = r["prenom"]?.ToString() ?? "";
                        lbNomEtudiant.Text = (nom + " " + prenom).Trim();

                        int idOpt = (r["idOption"] == DBNull.Value) ? 0 : Convert.ToInt32(r["idOption"]);

                        // ✅ FIX OPTION: detekte tab opsyon an otomatikman
                        string option = GetNomOptionSafe(idOpt);

                        // montre l sou paj la
                        lbOption.Text = string.IsNullOrWhiteSpace(option) ? "-" : option;
                    }
                }
            }
        }

        // ==========================
        // BULLETIN (Toutes les notes EXAMEN)
        // ==========================
        private void ChargerBulletin()
        {
            lbMsg.Text = "";
            ResetTotals();

            try
            {
                int idEtudiant = GetIdEtudiantFromCode(CodeEtudiant());

                string sql = @"
                    SELECT
                        c.nom AS Cours,
                        CAST(n.noteObtenue AS decimal(18,2)) AS NoteObtenue,
                        ISNULL(TRY_CONVERT(decimal(18,2), c.coefficient), 0) AS Coefficient
                    FROM dbo.note n
                    INNER JOIN dbo.cours c ON c.idCours = n.idCours
                    WHERE n.idEtudiant = @idEtudiant
                      AND UPPER(LTRIM(RTRIM(ISNULL(n.typeNote,'')))) = 'EXAMEN'
                    ORDER BY c.nom ASC
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

                gvBulletin.DataSource = dt;
                gvBulletin.DataBind();

                decimal moyenne = CalculerMoyenne();
                lbMoyenne.Text = moyenne.ToString("0.00", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                lbMsg.Text = "Erreur : " + ex.Message;
            }
        }

        private void ResetTotals()
        {
            _sumNoteCoeff = 0;
            _sumCoeff = 0;
            _totalNotes = 0;
            _countNotes = 0;
        }

        protected void gvBulletin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal note = 0;
                decimal coeff = 0;

                object oNote = DataBinder.Eval(e.Row.DataItem, "NoteObtenue");
                object oCoeff = DataBinder.Eval(e.Row.DataItem, "Coefficient");

                if (oNote != null && oNote != DBNull.Value) note = Convert.ToDecimal(oNote);
                if (oCoeff != null && oCoeff != DBNull.Value) coeff = Convert.ToDecimal(oCoeff);

                _countNotes++;
                _totalNotes += note;

                if (coeff > 0)
                {
                    _sumCoeff += coeff;
                    _sumNoteCoeff += (note * coeff);
                }

                // Center align all cells
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[0].Font.Bold = true;

                e.Row.Cells[1].Text = _totalNotes.ToString("0.00", CultureInfo.InvariantCulture);
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Font.Bold = true;

                e.Row.Cells[2].Text = _sumCoeff.ToString("0.##", CultureInfo.InvariantCulture);
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Font.Bold = true;
            }
        }

        private decimal CalculerMoyenne()
        {
            if (_sumCoeff > 0) return _sumNoteCoeff / _sumCoeff;
            if (_countNotes == 0) return 0;
            return _totalNotes / _countNotes;
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

        // ==========================
        // ✅ IMPRESSION PDF (Option + Signature)
        // ==========================
        protected void btnPrintAll_Click(object sender, EventArgs e)
        {
            try
            {
                int idEtudiant = GetIdEtudiantFromCode(CodeEtudiant());
                DataTable dt = GetBulletinDataExamens(idEtudiant);

                if (dt.Rows.Count == 0)
                {
                    lbMsg.Text = "Aucune note d’examen à imprimer.";
                    return;
                }

                // infos étudiant (et option)
                string nom = "", prenom = "", option = "-";
                int idOpt = 0;

                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(@"
                    SELECT TOP 1 e.nom, e.prenom, e.idOption
                    FROM dbo.etudiant e
                    WHERE e.idEtudiant = @idEtudiant
                ", con))
                {
                    cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            nom = r["nom"]?.ToString() ?? "";
                            prenom = r["prenom"]?.ToString() ?? "";
                            idOpt = (r["idOption"] == DBNull.Value) ? 0 : Convert.ToInt32(r["idOption"]);
                        }
                    }
                }

                option = GetNomOptionSafe(idOpt); // ✅ FIX OPTION PDF

                // totals
                decimal sumCoeff = 0, sumNoteCoeff = 0, totalNotes = 0;
                int count = 0;

                foreach (DataRow r in dt.Rows)
                {
                    decimal note = Convert.ToDecimal(r["NoteObtenue"]);
                    decimal coeff = Convert.ToDecimal(r["Coefficient"]);
                    totalNotes += note;
                    count++;

                    if (coeff > 0)
                    {
                        sumCoeff += coeff;
                        sumNoteCoeff += (note * coeff);
                    }
                }

                decimal moyenne = 0;
                if (sumCoeff > 0) moyenne = sumNoteCoeff / sumCoeff;
                else if (count > 0) moyenne = totalNotes / count;

                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.A4, 35, 35, 40, 35);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                    // watermark
                    string watermarkPath = Server.MapPath("~/assets/img/logo-ct-dark.png");
                    writer.PageEvent = new BulletinWatermarkEvent(watermarkPath, 0.08f);

                    doc.Open();

                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                    var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                    // Header (Logo + texte)
                    PdfPTable headerTbl = new PdfPTable(2);
                    headerTbl.WidthPercentage = 100;
                    headerTbl.SetWidths(new float[] { 15f, 85f });

                    PdfPCell logoCell = new PdfPCell { Border = Rectangle.NO_BORDER };
                    string logoPath = Server.MapPath("~/assets/img/logo-ct-dark.png");
                    if (File.Exists(logoPath))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.ScaleToFit(55f, 55f);
                        logoCell.AddElement(logo);
                    }
                    headerTbl.AddCell(logoCell);

                    PdfPCell textCell = new PdfPCell { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
                    Paragraph header = new Paragraph { Alignment = Element.ALIGN_CENTER };
                    header.Add(new Phrase("Union des Techniciens Modernes d’Haïti – École Professionnelle\n", titleFont));
                    header.Add(new Phrase("40, Delmas 95 Rue Promeyrac, Jacquet Tybull Impasse Louis Jeanty\n", normalFont));
                    header.Add(new Phrase("Tel : (509) 4473-9494 | utm04haiti@gmail.com\n", normalFont));
                    header.Add(new Phrase("\nBULLETIN DE NOTES\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)));
                    textCell.AddElement(header);
                    headerTbl.AddCell(textCell);

                    doc.Add(headerTbl);
                    doc.Add(new Paragraph(" ", normalFont));

                    // Infos étudiant
                    PdfPTable info = new PdfPTable(2);
                    info.WidthPercentage = 100;
                    info.SetWidths(new float[] { 30, 70 });

                    AddRow(info, "Code", CodeEtudiant(), boldFont, normalFont);
                    AddRow(info, "Nom", nom, boldFont, normalFont);
                    AddRow(info, "Prénom", prenom, boldFont, normalFont);
                    AddRow(info, "Option", string.IsNullOrWhiteSpace(option) ? "-" : option, boldFont, normalFont);
                    AddRow(info, "Date", DateTime.Now.ToString("dd/MM/yyyy"), boldFont, normalFont);

                    doc.Add(info);
                    doc.Add(new Paragraph(" ", normalFont));

                    // Table notes
                    PdfPTable tb = new PdfPTable(3);
                    tb.WidthPercentage = 100;
                    tb.SetWidths(new float[] { 55, 25, 20 });

                    AddHeader(tb, "NOM DU COURS", boldFont);
                    AddHeader(tb, "NOTE OBTENUE", boldFont);
                    AddHeader(tb, "COEFFICIENT", boldFont);

                    foreach (DataRow r in dt.Rows)
                    {
                        tb.AddCell(new PdfPCell(new Phrase(r["Cours"].ToString(), normalFont)) { Padding = 6 });
                        tb.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(r["NoteObtenue"]).ToString("0.00"), normalFont))
                        { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                        tb.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(r["Coefficient"]).ToString("0.##"), normalFont))
                        { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                    }

                    // Total + moyenne
                    tb.AddCell(new PdfPCell(new Phrase("TOTAL", boldFont)) { Padding = 6 });
                    tb.AddCell(new PdfPCell(new Phrase(totalNotes.ToString("0.00"), boldFont))
                    { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                    tb.AddCell(new PdfPCell(new Phrase(sumCoeff.ToString("0.##"), boldFont))
                    { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER });

                    tb.AddCell(new PdfPCell(new Phrase("MOYENNE", boldFont)) { Padding = 6 });
                    tb.AddCell(new PdfPCell(new Phrase(moyenne.ToString("0.00"), boldFont))
                    { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER });
                    tb.AddCell(new PdfPCell(new Phrase("", boldFont)) { Padding = 6 });

                    doc.Add(tb);

                    // ✅ Signature Direction (espas pou siyen)
                    doc.Add(new Paragraph("\n\n\n", normalFont));

                    PdfPTable sign = new PdfPTable(2);
                    sign.WidthPercentage = 100;
                    sign.SetWidths(new float[] { 60, 40 });

                    sign.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });

                    PdfPCell right = new PdfPCell(new Phrase(
                        "__________________________\nLa Direction",
                        boldFont
                    ));
                    right.Border = Rectangle.NO_BORDER;
                    right.HorizontalAlignment = Element.ALIGN_CENTER;

                    sign.AddCell(right);
                    doc.Add(sign);

                    doc.Close();

                    // download sans Response.End()
                    byte[] bytes = ms.ToArray();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment;filename=Bulletin_{CodeEtudiant()}.pdf");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                lbMsg.Text = "Erreur impression : " + ex.Message;
            }
        }

        private DataTable GetBulletinDataExamens(int idEtudiant)
        {
            string sql = @"
                SELECT
                    c.nom AS Cours,
                    CAST(n.noteObtenue AS decimal(18,2)) AS NoteObtenue,
                    ISNULL(TRY_CONVERT(decimal(18,2), c.coefficient), 0) AS Coefficient
                FROM dbo.note n
                INNER JOIN dbo.cours c ON c.idCours = n.idCours
                WHERE n.idEtudiant = @idEtudiant
                  AND UPPER(LTRIM(RTRIM(ISNULL(n.typeNote,'')))) = 'EXAMEN'
                ORDER BY c.nom ASC
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
            return dt;
        }

        // ======================================================
        // ✅ FIX OPTION: detecte table option + colonne texte
        // ======================================================
        private string GetNomOptionSafe(int idOption)
        {
            if (idOption <= 0) return "-";

            // 1) jwenn tab opsyon ki egziste (optionChoisie / option / options ...)
            string tableName = DetectOptionTableName();
            if (string.IsNullOrWhiteSpace(tableName)) return "-";

            // 2) jwenn kolòn non opsyon an
            string colName = DetectOptionNameColumn(tableName);
            if (string.IsNullOrWhiteSpace(colName)) return "-";

            // 3) li non opsyon an
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand($@"
                SELECT TOP 1 [{colName}]
                FROM dbo.[{tableName}]
                WHERE idOption = @idOption
            ", con))
            {
                cmd.Parameters.AddWithValue("@idOption", idOption);
                con.Open();
                object o = cmd.ExecuteScalar();
                return (o == null || o == DBNull.Value) ? "-" : o.ToString();
            }
        }

        private string DetectOptionTableName()
        {
            // Priyorite: optionChoisie -> option -> options
            var candidates = new[] { "optionChoisie", "option", "options" };

            var tables = new List<string>();
            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                SELECT TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = 'dbo'
            ", con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read()) tables.Add(r["TABLE_NAME"].ToString());
                }
            }

            // 1) match egzak sou candidates yo
            foreach (var c in candidates)
            {
                var found = tables.FirstOrDefault(t => t.Equals(c, StringComparison.OrdinalIgnoreCase));
                if (found != null) return found;
            }

            // 2) sinon pran premye tab ki gen "option" nan non li (men pa optionChoisieCours)
            var fallback = tables
                .FirstOrDefault(t =>
                    t.IndexOf("option", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    t.IndexOf("cours", StringComparison.OrdinalIgnoreCase) < 0);

            return fallback; // ka ret null si pa jwenn
        }

        private string DetectOptionNameColumn(string tableName)
        {
            var candidates = new[] { "nomOption", "nom", "libelle", "designation", "option", "intitule" };
            var cols = new List<string>();

            using (var con = new SqlConnection(cs))
            using (var cmd = new SqlCommand(@"
                SELECT COLUMN_NAME
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME=@t
            ", con))
            {
                cmd.Parameters.AddWithValue("@t", tableName);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read()) cols.Add(r["COLUMN_NAME"].ToString());
                }
            }

            foreach (var c in candidates)
            {
                var found = cols.FirstOrDefault(x => x.Equals(c, StringComparison.OrdinalIgnoreCase));
                if (found != null) return found;
            }

            // fallback: premye kolòn text ki pa id
            foreach (var col in cols)
            {
                string lc = col.ToLowerInvariant();
                if (lc.Contains("id")) continue;
                return col;
            }

            return null;
        }

        // PDF helpers
        private void AddRow(PdfPTable t, string label, string value, Font labelFont, Font valueFont)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(label, labelFont)) { Border = Rectangle.NO_BORDER, Padding = 5 };
            PdfPCell c2 = new PdfPCell(new Phrase(value ?? "", valueFont)) { Border = Rectangle.NO_BORDER, Padding = 5 };
            t.AddCell(c1);
            t.AddCell(c2);
        }

        private void AddHeader(PdfPTable t, string text, Font font)
        {
            PdfPCell c = new PdfPCell(new Phrase(text, font)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER };
            t.AddCell(c);
        }
    }

    // Watermark
    public class BulletinWatermarkEvent : PdfPageEventHelper
    {
        private readonly string _imagePath;
        private readonly float _opacity;

        public BulletinWatermarkEvent(string imagePath, float opacity)
        {
            _imagePath = imagePath;
            _opacity = opacity;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (string.IsNullOrWhiteSpace(_imagePath) || !File.Exists(_imagePath)) return;

            PdfContentByte canvas = writer.DirectContentUnder;
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(_imagePath);

            float pageW = document.PageSize.Width;
            float pageH = document.PageSize.Height;

            img.ScaleToFit(pageW, pageH);

            float x = (pageW - img.ScaledWidth) / 2f;
            float y = (pageH - img.ScaledHeight) / 2f;
            img.SetAbsolutePosition(x, y);

            PdfGState gs = new PdfGState { FillOpacity = _opacity, StrokeOpacity = _opacity };

            canvas.SaveState();
            canvas.SetGState(gs);
            canvas.AddImage(img);
            canvas.RestoreState();
        }
    }
}