using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using iTextSharp.text;
using iTextSharp.text.pdf;

// ✅ Alias pou evite konfli ak iTextSharp.text.ListItem
using WebListItem = System.Web.UI.WebControls.ListItem;

namespace UTMH_Edu.Vue
{
    public partial class FormListePaiementEtudiant : System.Web.UI.Page
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
                ChargerMotifs();
                ChargerOptions();

                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "Sélectionnez un motif pour afficher la liste.";
            }
        }

        // =========================
        // FILTRES
        // =========================
        private void ChargerMotifs()
        {
            ddlMotif.Items.Clear();
            ddlMotif.Items.Add(new WebListItem("-- Motif --", "0"));

            ddlMotif.Items.Add(new WebListItem("Année Académique", "ANNEE_ACADEMIQUE"));
            ddlMotif.Items.Add(new WebListItem("Graduation", "GRADUATION"));
            ddlMotif.Items.Add(new WebListItem("Pratiquee", "PRATIQUEE"));
            ddlMotif.Items.Add(new WebListItem("Examen", "EXAMEN"));
            ddlMotif.Items.Add(new WebListItem("Attestation", "ATTESTATION"));
            ddlMotif.Items.Add(new WebListItem("Certificat", "CERTIFICAT"));
            ddlMotif.Items.Add(new WebListItem("Stage", "STAGE"));
        }

        private void ChargerOptions()
        {
            ddlOption.Items.Clear();
            ddlOption.Items.Add(new WebListItem("-- Option --", "0"));

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand("SELECT idOption, nom FROM dbo.optionChoisie ORDER BY nom", con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ddlOption.Items.Add(new WebListItem(
                            dr["nom"].ToString(),
                            dr["idOption"].ToString()
                        ));
                    }
                }
            }
        }

        protected void ddlMotif_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerListePaiements();
        }

        protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChargerListePaiements();
        }

        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            ddlMotif.SelectedIndex = 0;
            ddlOption.SelectedIndex = 0;

            GridView1.DataSource = null;
            GridView1.DataBind();
            Lb2.Text = "Sélectionnez un motif pour afficher la liste.";
        }

        // =========================
        // REGLE Motif -> MontantDu
        // =========================
        private decimal GetMontantDu(string motif)
        {
            string m = (motif ?? "").Trim().ToUpperInvariant();

            if (m == "ANNEE_ACADEMIQUE") return 20000m;
            if (m == "GRADUATION") return 25000m;

            return 0m;
        }

        // =========================
        // LISTE (Grid)
        // =========================
        private void ChargerListePaiements()
        {
            if (ddlMotif.SelectedValue == "0")
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "Sélectionnez un motif pour afficher la liste.";
                return;
            }

            string motif = ddlMotif.SelectedValue;
            decimal montantDu = GetMontantDu(motif);

            if (montantDu <= 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                Lb2.Text = "Ce motif n’a pas de montant fixe. Vous pouvez définir son prix dans les règles.";
                return;
            }

            int idOption = 0;
            if (ddlOption.SelectedValue != "0")
                idOption = Convert.ToInt32(ddlOption.SelectedValue);

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
SELECT
    e.idEtudiant,
    e.code,
    e.nom,
    e.prenom,
    o.nom AS nomOption,
    ISNULL(SUM(p.montantPaye),0) AS montantPayeTotal,
    CASE 
        WHEN (@montantDu - ISNULL(SUM(p.montantPaye),0)) < 0 THEN 0
        ELSE (@montantDu - ISNULL(SUM(p.montantPaye),0))
    END AS balanceReste
FROM dbo.etudiant e
INNER JOIN dbo.optionChoisie o ON o.idOption = e.idOption
LEFT JOIN dbo.Paiement p ON p.idEtudiant = e.idEtudiant AND p.motif = @motif
WHERE
    (@idOption = 0 OR e.idOption = @idOption)
GROUP BY e.idEtudiant, e.code, e.nom, e.prenom, o.nom
ORDER BY e.nom, e.prenom;";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@motif", motif);
                    cmd.Parameters.AddWithValue("@montantDu", montantDu);
                    cmd.Parameters.AddWithValue("@idOption", idOption);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Lb2.Text = dt.Rows.Count == 0 ? "Aucun étudiant trouvé." : "";
                }
            }
        }

        // =========================
        // ACTION: PRINT PDF
        // =========================
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrintPdf")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idEtudiant = Convert.ToInt32(GridView1.DataKeys[index].Values["idEtudiant"]);

                if (ddlMotif.SelectedValue == "0")
                {
                    Lb2.Text = "Sélectionnez un motif avant l’impression.";
                    return;
                }

                GenererRecuPdf(idEtudiant, ddlMotif.SelectedValue);
            }
        }

        // =========================
        // PDF
        // =========================
        private void GenererRecuPdf(int idEtudiant, string motif)
        {
            // Infos étudiant
            string code = "", nom = "", prenom = "", option = "", anneeAcademique = "";

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
SELECT e.code, e.nom, e.prenom, e.anneeAcademique, o.nom AS nomOption
FROM dbo.etudiant e
INNER JOIN dbo.optionChoisie o ON o.idOption = e.idOption
WHERE e.idEtudiant=@id;", con))
                {
                    cmd.Parameters.AddWithValue("@id", idEtudiant);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read()) return;

                        code = dr["code"].ToString();
                        nom = dr["nom"].ToString();
                        prenom = dr["prenom"].ToString();
                        option = dr["nomOption"].ToString();
                        anneeAcademique = dr["anneeAcademique"].ToString();
                    }
                }
            }

            // Montant dû
            decimal montantDu = GetMontantDu(motif);

            // Total payé
            decimal totalPaye = 0;
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT ISNULL(SUM(montantPaye),0)
FROM dbo.Paiement
WHERE idEtudiant=@id AND motif=@motif;", con))
            {
                cmd.Parameters.AddWithValue("@id", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);
                con.Open();
                totalPaye = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            // Balance (reste)
            decimal balance = montantDu - totalPaye;
            if (balance < 0) balance = 0;

            // Details versements
            DataTable dtDetails = new DataTable();
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT versement, datePaiement, montantPaye
FROM dbo.Paiement
WHERE idEtudiant=@id AND motif=@motif
ORDER BY versement ASC, datePaiement ASC;", con))
            {
                cmd.Parameters.AddWithValue("@id", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtDetails);
            }

            // =====================
            // PDF generation + watermark
            // =====================
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 35, 35, 40, 35);

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                // ✅ Filigran ki ranpli tout paj la
                string watermarkPath = Server.MapPath("~/assets/img/logo-ct-dark.png");
                writer.PageEvent = new ImageWatermarkEvent(watermarkPath, 0.08f);

                doc.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                // =====================
                // ENTÊTE: LOGO DEVAN NON LEKÒL LA
                // =====================
                PdfPTable headerTbl = new PdfPTable(2);
                headerTbl.WidthPercentage = 100;
                headerTbl.SetWidths(new float[] { 15f, 85f });

                PdfPCell logoCell = new PdfPCell();
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                string logoPath = Server.MapPath("~/assets/img/logo-ct-dark.png");
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(55f, 55f);
                    logoCell.AddElement(logo);
                }
                headerTbl.AddCell(logoCell);

                PdfPCell textCell = new PdfPCell();
                textCell.Border = Rectangle.NO_BORDER;
                textCell.HorizontalAlignment = Element.ALIGN_CENTER;
                textCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                Paragraph header = new Paragraph();
                header.Alignment = Element.ALIGN_CENTER;
                header.Add(new Phrase("Union des Techniciens Modernes d’Haïti – École Professionnelle\n", titleFont));
                header.Add(new Phrase("40, Delmas 95 Rue Promeyrac, Jacquet Tybull Impasse Louis Jeanty\n", normalFont));
                header.Add(new Phrase("Tel : (509) 4473-9494 | utm04haiti@gmail.com\n", normalFont));
                header.Add(new Phrase("\nReçu\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13)));

                textCell.AddElement(header);
                headerTbl.AddCell(textCell);

                doc.Add(headerTbl);
                doc.Add(new Paragraph(" ", normalFont));

                // =====================
                // TABLE INFOS
                // =====================
                PdfPTable t = new PdfPTable(2);
                t.WidthPercentage = 100;
                t.SetWidths(new float[] { 35, 65 });

                AddRow(t, "Code", code, boldFont, normalFont);
                AddRow(t, "Nom", nom, boldFont, normalFont);
                AddRow(t, "Prénom", prenom, boldFont, normalFont);
                AddRow(t, "Option", option, boldFont, normalFont);
                AddRow(t, "Année Académique", anneeAcademique, boldFont, normalFont);
                AddRow(t, "Motif", motif, boldFont, normalFont);
                AddRow(t, "Montant à Payer", montantDu.ToString("N0"), boldFont, normalFont);
                AddRow(t, "Montant payé (Total)", totalPaye.ToString("N0"), boldFont, normalFont);
                AddRow(t, "Balance (Reste)", balance.ToString("N0"), boldFont, normalFont);

                // ✅ Date san lè
                AddRow(t, "Date", DateTime.Now.ToString("dd/MM/yyyy"), boldFont, normalFont);

                doc.Add(t);
                doc.Add(new Paragraph(" ", normalFont));

                // =====================
                // DETAILS VERSEMENTS
                // =====================
                doc.Add(new Paragraph("DÉTAIL DES VERSEMENTS", boldFont));
                doc.Add(new Paragraph(" ", normalFont));

                PdfPTable td = new PdfPTable(3);
                td.WidthPercentage = 100;
                td.SetWidths(new float[] { 20, 40, 40 });

                AddHeader(td, "Versement", boldFont);
                AddHeader(td, "Date", boldFont);
                AddHeader(td, "Montant payé", boldFont);

                foreach (DataRow r in dtDetails.Rows)
                {
                    td.AddCell(new Phrase(r["versement"].ToString(), normalFont));
                    td.AddCell(new Phrase(Convert.ToDateTime(r["datePaiement"]).ToString("dd/MM/yyyy"), normalFont));
                    td.AddCell(new Phrase(Convert.ToDecimal(r["montantPaye"]).ToString("N0"), normalFont));
                }

                doc.Add(td);

                // =====================
                // SIGNATURE DIRECTION
                // =====================
                doc.Add(new Paragraph("\n\n", normalFont));

                PdfPTable sign = new PdfPTable(2);
                sign.WidthPercentage = 100;
                sign.SetWidths(new float[] { 60, 40 });

                PdfPCell left = new PdfPCell(new Phrase(""));
                left.Border = Rectangle.NO_BORDER;

                PdfPCell right = new PdfPCell(new Phrase(
                    "\n\n\n\n\n\n\n__________________________\n La Direction",
                    boldFont
                ));
                right.HorizontalAlignment = Element.ALIGN_CENTER;
                right.Border = Rectangle.NO_BORDER;

                sign.AddCell(left);
                sign.AddCell(right);

                doc.Add(sign);

                doc.Close();

                byte[] bytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=RecuPaiement_{code}_{motif}.pdf");
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }

        // =========================
        // Helpers PDF
        // =========================
        private void AddRow(PdfPTable t, string label, string value, Font labelFont, Font valueFont)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(label, labelFont));
            c1.Border = Rectangle.NO_BORDER;
            c1.Padding = 5;

            PdfPCell c2 = new PdfPCell(new Phrase(value ?? "", valueFont));
            c2.Border = Rectangle.NO_BORDER;
            c2.Padding = 5;

            t.AddCell(c1);
            t.AddCell(c2);
        }

        private void AddHeader(PdfPTable t, string text, Font font)
        {
            PdfPCell c = new PdfPCell(new Phrase(text, font));
            c.Padding = 6;
            t.AddCell(c);
        }
    }

    // =========================
    // WATERMARK EVENT (FILIGRAM)
    // =========================
    public class ImageWatermarkEvent : PdfPageEventHelper
    {
        private readonly string _imagePath;
        private readonly float _opacity;

        public ImageWatermarkEvent(string imagePath, float opacity = 0.08f)
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

            float x = (pageW - img.ScaledWidth) / 2;
            float y = (pageH - img.ScaledHeight) / 2;
            img.SetAbsolutePosition(x, y);

            PdfGState gs = new PdfGState { FillOpacity = _opacity, StrokeOpacity = _opacity };

            canvas.SaveState();
            canvas.SetGState(gs);
            canvas.AddImage(img);
            canvas.RestoreState();
        }
    }
}