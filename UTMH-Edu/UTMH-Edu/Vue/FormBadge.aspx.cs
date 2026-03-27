using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UTMH_Edu.Vue
{
    public partial class FormBadge : System.Web.UI.Page
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormConnexion.aspx");
                    return;
                }
                this.ChargerEtudiants();
            }

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GenererBadge")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idEtudiant = Convert.ToInt32(
                    GridView1.DataKeys[index].Value);

                GenererBadge(idEtudiant);
            }
        }

       
        private void ChargerEtudiants()
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT idEtudiant, code, cin, nom, prenom
        FROM etudiant
        ORDER BY nom, prenom", con))
            {
                con.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void btnRecherche_Click(object sender, EventArgs e)
        {
            string recherche = txtRecherche.Text.Trim();
            rechercherEtudiant(recherche);
        }
        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            txtRecherche.Text = "";
            Lb2.Text = "";
            this.ChargerEtudiants();
        }

        void rechercherEtudiant(string recherche = "")
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                // Requête SQL avec filtre sur statut et recherche
                string chReq = "SELECT idEtudiant,code, nom,sexe, adresse, cin, prenom, email, telephone " +
                               "FROM etudiant " +
                               "WHERE (@rech = '' OR nom LIKE '%' + @rech + '%' " +
                               "OR prenom LIKE '%' + @rech + '%' OR code LIKE '%' + @rech + '%') " +
                               "ORDER BY code DESC";

                using (SqlCommand cmd = new SqlCommand(chReq, con))
                {
                    cmd.Parameters.AddWithValue("@rech", recherche);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Lb2.Text = dt.Rows.Count > 0 ? "" : "<span style='color:red;'>Aucun Etudiant trouvé.</span>";
                }
            }
        }


        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }


        private void GenererBadge(int idEtudiant)
        {
            string nom = "", prenom = "", code, cin = "", tel = "", nomOption, promotion = "";
            string photoDb = "";


            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT e.nom,e.code, e.prenom, e.cin, e.telephone, e.anneeAcademique, e.photo,
               ISNULL(o.nom,'') AS optionNom
        FROM etudiant e
        LEFT JOIN optionChoisie o ON o.idOption = e.idOption
        WHERE e.idEtudiant = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", idEtudiant);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        throw new Exception("Étudiant introuvable.");

                    nom = dr["nom"].ToString();
                    code = dr["code"].ToString();
                    prenom = dr["prenom"].ToString();
                    cin = dr["cin"].ToString();
                    tel = dr["telephone"].ToString();
                    promotion = dr["anneeAcademique"].ToString();
                    nomOption = dr["optionNom"].ToString();
                    photoDb = dr["photo"] == DBNull.Value ? "" : dr["photo"].ToString();
                }
                if (string.IsNullOrWhiteSpace(photoDb))
                {
                    //ShowAlert( "⚠ Cet étudiant n'a pas de photo. Veuillez en ajouter avant de générer le badge.");
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg",
 "showToast('warning','Cet étudiant n\\'a pas de photo. Veuillez en ajouter avant de générer le badge.');", true);
                    return;
                }
            }



            // ✅ Ici on résout le chemin photo (robuste)
            string photoPath = ResolvePhysicalPhotoPath(photoDb);

            // 📄 Badge size 640 x 1015
            iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(640f, 1015f);
            Document doc = new Document(pageSize, 0, 0, 0, 0);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                float W = 640f, H = 1015f, centerX = W / 2f;

                // 🖼 Background template
                string templatePath = Server.MapPath("~/Images/badge_template.jpg");
                if (File.Exists(templatePath))
                {
                    iTextSharp.text.Image bg = iTextSharp.text.Image.GetInstance(templatePath);
                    bg.ScaleAbsolute(W, H);
                    bg.SetAbsolutePosition(0, 0);
                    writer.DirectContentUnder.AddImage(bg);
                }

                PdfContentByte cb = writer.DirectContent;

                BaseFont bfBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);

                cb.BeginText();
                cb.SetColorFill(BaseColor.BLACK);

                cb.SetFontAndSize(bfBold, 34);
                cb.ShowTextAligned(Element.ALIGN_CENTER, (nom + " " + prenom).ToUpper(), centerX, 450f, 0);

                cb.SetFontAndSize(bf, 22);
                cb.ShowTextAligned(Element.ALIGN_CENTER, "Code : " + code, centerX, 395f, 0);
                cb.ShowTextAligned(Element.ALIGN_CENTER, "NIF/CIN : " + cin, centerX, 350f, 0);

                cb.ShowTextAligned(Element.ALIGN_CENTER, "Tél : " + tel, centerX, 305f, 0);

                cb.ShowTextAligned(Element.ALIGN_CENTER, "Option : " + nomOption, centerX, 250f, 0);


                cb.SetFontAndSize(bfBold, 26);
                cb.ShowTextAligned(Element.ALIGN_CENTER, "Promotion " + promotion, centerX, 190f, 0);

                cb.EndText();

                // 📷 PHOTO EN HEXAGON ✅ (ajiste pozisyon an si bezwen)
                float hx = 170f;
                float hy = 510f;
                float hw = 300f;
                float hh = 300f;

                // (OPTIONNEL) gade kontou a pou ajiste pi fasil
                // DrawHexGuide(cb, hx, hy, hw, hh);

                AddPhotoInHexagon(writer, photoPath, hx, hy, hw, hh,
                    new BaseColor(46, 49, 146), 6f); // border ble


                // 🔳 QR Code (robuste même si CIN vide)
                QRCodeGenerator qrGenerator = new QRCodeGenerator();

                string qrContent = cin;

                // si cin vide -> fallback
                if (string.IsNullOrWhiteSpace(qrContent))
                {
                    // ou ka mete nom+prenom oubyen idEtudiant
                    qrContent = $"Nom: {nom}; Prenom: {prenom}; Tel: {tel}; Promotion: {promotion}";
                }

                // si menm sa vid (ka rive), mete yon texte default
                if (string.IsNullOrWhiteSpace(qrContent))
                {
                    qrContent = "BADGE_ETUDIANT";
                }

                QRCodeData qrData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrData);
                Bitmap qrImg = qrCode.GetGraphic(6);

                using (MemoryStream msQr = new MemoryStream())
                {
                    qrImg.Save(msQr, System.Drawing.Imaging.ImageFormat.Png);
                    iTextSharp.text.Image qr = iTextSharp.text.Image.GetInstance(msQr.ToArray());
                    qr.ScaleToFit(160f, 160f);
                    qr.SetAbsolutePosition(240f, 35f);
                    doc.Add(qr);
                }
                doc.Close();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Badge_" + cin + ".pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();


            }
        }



        private string NormalizePhotoVirtualPath(string photoDb)
        {
            if (string.IsNullOrWhiteSpace(photoDb))
                return "~/Uploads/default-user.png";

            photoDb = photoDb.Trim().Replace("\\", "/");

            // deja virtuel
            if (photoDb.StartsWith("~/Uploads/PhotoProfil/")) return photoDb;
            if (photoDb.StartsWith("~/Uploads/PhotoProfil/")) return photoDb;
            if (photoDb.StartsWith("~/")) return photoDb;
            if (photoDb.StartsWith("/")) return "~" + photoDb;              // "/Uploads/x.jpg" -> "~/Uploads/x.jpg"
            if (photoDb.StartsWith("Uploads/", StringComparison.OrdinalIgnoreCase))
                return "~/" + photoDb;                                      // "Uploads/x.jpg" -> "~/Uploads/x.jpg"

            // filename sèlman
            return "~/Uploads/" + photoDb;                                  // "etu_1002.jpg" -> "~/Uploads/etu_1002.jpg"
        }

        private string ResolvePhysicalPhotoPath(string photoDb)
        {
            // 1) normal chemen virtuel
            string virtualPath = NormalizePhotoVirtualPath(photoDb);

            // 2) map vers chemin physique
            string physicalPath = Server.MapPath(virtualPath);

            // 3) si file la egziste, fini
            if (File.Exists(physicalPath))
                return physicalPath;

            // 4) Si pa gen extension, eseye ajoute (.jpg/.jpeg/.png)
            string ext = Path.GetExtension(physicalPath);
            if (string.IsNullOrEmpty(ext))
            {
                string[] exts = { ".jpg", ".jpeg", ".png" };
                foreach (var e in exts)
                {
                    string test = physicalPath + e;
                    if (File.Exists(test))
                        return test;
                }
            }

            // 5) Dènye fallback
            return Server.MapPath("~/Uploads/default-user.png");
        }

        private void AddPhotoInHexagon(PdfWriter writer, string photoPath,
    float x, float y, float w, float h,
    BaseColor borderColor = null, float borderWidth = 0f)
        {
            if (string.IsNullOrWhiteSpace(photoPath) || !File.Exists(photoPath))
                return;

            PdfContentByte cb = writer.DirectContent;

            float cx = x + (w / 2f);
            float cy = y + (h / 2f);
            float r = Math.Min(w, h) / 2f;

            // Angles: pointe en haut (hexagon "vertical")
            float[] angles = { 90f, 30f, -30f, -90f, -150f, 150f };

            cb.SaveState();

            // --- Clip path hexagon ---
            float x0 = cx + r * (float)Math.Cos(angles[0] * Math.PI / 180f);
            float y0 = cy + r * (float)Math.Sin(angles[0] * Math.PI / 180f);
            cb.MoveTo(x0, y0);

            for (int i = 1; i < angles.Length; i++)
            {
                float px = cx + r * (float)Math.Cos(angles[i] * Math.PI / 180f);
                float py = cy + r * (float)Math.Sin(angles[i] * Math.PI / 180f);
                cb.LineTo(px, py);
            }
            cb.ClosePath();
            cb.Clip();
            cb.NewPath();

            // --- Add image (cover) ---
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(photoPath);

            float imgW = img.Width;
            float imgH = img.Height;

            // "cover": ranpli bwat la san blan
            float scale = Math.Max(w / imgW, h / imgH);
            float drawW = imgW * scale;
            float drawH = imgH * scale;

            float drawX = x + (w - drawW) / 2f;
            float drawY = y + (h - drawH) / 2f;

            img.ScaleAbsolute(drawW, drawH);
            img.SetAbsolutePosition(drawX, drawY);

            cb.AddImage(img);

            cb.RestoreState();

            // --- Optional border ---
            if (borderColor != null && borderWidth > 0f)
            {
                cb.SaveState();
                cb.SetColorStroke(borderColor);
                cb.SetLineWidth(borderWidth);

                cb.MoveTo(x0, y0);
                for (int i = 1; i < angles.Length; i++)
                {
                    float px = cx + r * (float)Math.Cos(angles[i] * Math.PI / 180f);
                    float py = cy + r * (float)Math.Sin(angles[i] * Math.PI / 180f);
                    cb.LineTo(px, py);
                }
                cb.ClosePathStroke();
                cb.RestoreState();
            }
        }
        private void DrawHexGuide(PdfContentByte cb, float x, float y, float w, float h)
        {
            float cx = x + (w / 2f);
            float cy = y + (h / 2f);
            float r = Math.Min(w, h) / 2f;
            float[] angles = { 90f, 30f, -30f, -90f, -150f, 150f };

            cb.SaveState();
            cb.SetColorStroke(new BaseColor(255, 0, 0)); // rouge
            cb.SetLineWidth(3f);

            float x0 = cx + r * (float)Math.Cos(angles[0] * Math.PI / 180f);
            float y0 = cy + r * (float)Math.Sin(angles[0] * Math.PI / 180f);
            cb.MoveTo(x0, y0);

            for (int i = 1; i < angles.Length; i++)
            {
                float px = cx + r * (float)Math.Cos(angles[i] * Math.PI / 180f);
                float py = cy + r * (float)Math.Sin(angles[i] * Math.PI / 180f);
                cb.LineTo(px, py);
            }
            cb.ClosePathStroke();
            cb.RestoreState();
        }




    }
}