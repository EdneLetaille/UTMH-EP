using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail; // fallback SMTP
using System.Reflection;
using System.Web.UI;

namespace UTMH_Edu.Vue
{
    public partial class FormEnvoyerMessage : System.Web.UI.Page
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
                Lb2.Text = "";
                Lb2.CssClass = "fw-bold text-muted";
            }
        }

        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            // Ou ka chanje sa pou redirect kote ou vle (dashboard, etc.)
            Response.Redirect("FormAccueilAdm.aspx");
        }

        protected void btnEnvoyer_Click(object sender, EventArgs e)
        {
            try
            {
                string type = (ddlTypeDestinataire.SelectedValue ?? "TOUS").Trim().ToUpperInvariant();
                string objet = (txtObjet.Text ?? "").Trim();
                string message = (txtMessage.Text ?? "").Trim();

                if (string.IsNullOrWhiteSpace(objet))
                    throw new Exception("Veuillez saisir l'objet.");

                if (string.IsNullOrWhiteSpace(message))
                    throw new Exception("Veuillez saisir le message.");

                List<string> emails = GetEmailsByType(type);

                if (emails.Count == 0)
                    throw new Exception("Aucun email trouvé pour ce type de destinataire.");

                // Evite doublon
                emails = emails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                int ok = 0;
                int ko = 0;
                List<string> erreurs = new List<string>();

                foreach (string to in emails)
                {
                    try
                    {
                        SendEmailSmart(to, objet, message);
                        ok++;
                    }
                    catch (Exception exOne)
                    {
                        ko++;
                        // Pa mete twòp detay nan UI, men nou kenbe yon ti rezime
                        erreurs.Add($"{to} : {exOne.Message}");
                    }
                }

                if (ko == 0)
                {
                    Lb2.CssClass = "fw-bold text-success";
                    Lb2.Text = $"Message envoyé avec succès. Total destinataires: {ok}.";
                }
                else
                {
                    Lb2.CssClass = "fw-bold text-warning";
                    // Afichaj kout pou pa chaje paj la
                    Lb2.Text = $"Envoi terminé. Succès: {ok} | Échecs: {ko}. " +
                               $"(Vérifiez la configuration email ou les adresses invalides.)";
                }

                // Optional: reset champs
                txtObjet.Text = "";
                txtMessage.Text = "";
            }
            catch (Exception ex)
            {
                Lb2.CssClass = "fw-bold text-danger";
                Lb2.Text = "Erreur : " + ex.Message;
            }
        }

        // =========================================================
        // 1) Récupération emails selon type
        // =========================================================
        private List<string> GetEmailsByType(string type)
        {
            // ⚠️ Adapte non kolòn yo si nan BD ou se "Email" olye de "email"
            // Mwen mete "email" paske sa pi komen nan tab yo.
            // Si ou gen "Email", chanje SELECT yo: SELECT Email AS email

            string sql = "";

            if (type == "ADM")
            {
                sql = @"
SELECT DISTINCT LTRIM(RTRIM(ISNULL(email,''))) AS email
FROM dbo.administrateur
WHERE ISNULL(email,'') <> '';";
            }
            else if (type == "PROF")
            {
                sql = @"
SELECT DISTINCT LTRIM(RTRIM(ISNULL(email,''))) AS email
FROM dbo.professeur
WHERE ISNULL(email,'') <> '';";
            }
            else if (type == "ETUD")
            {
                sql = @"
SELECT DISTINCT LTRIM(RTRIM(ISNULL(email,''))) AS email
FROM dbo.etudiant
WHERE ISNULL(email,'') <> '';";
            }
            else // TOUS
            {
                sql = @"
SELECT DISTINCT email FROM
(
    SELECT LTRIM(RTRIM(ISNULL(email,''))) AS email
    FROM dbo.administrateur
    WHERE ISNULL(email,'') <> ''

    UNION ALL

    SELECT LTRIM(RTRIM(ISNULL(email,''))) AS email
    FROM dbo.professeur
    WHERE ISNULL(email,'') <> ''

    UNION ALL

    SELECT LTRIM(RTRIM(ISNULL(email,''))) AS email
    FROM dbo.etudiant
    WHERE ISNULL(email,'') <> ''
) x
WHERE ISNULL(email,'') <> '';";
            }

            List<string> emails = new List<string>();

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string email = dr["email"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(email))
                            emails.Add(email.Trim());
                    }
                }
            }

            return emails;
        }

        // =========================================================
        // 2) Envoi email: tente ServiceTechnique (Reflection), sinon SMTP
        // =========================================================
        private void SendEmailSmart(string to, string subject, string body)
        {
            // A) Tente ServiceTechnique si li egziste nan pwojè a
            bool sentByService = TrySendUsingServiceTechnique(to, subject, body);

            if (sentByService) return;

            // B) Fallback SMTP (mailSettings web.config)
            SendBySmtpFallback(to, subject, body);
        }

        private bool TrySendUsingServiceTechnique(string to, string subject, string body)
        {
            // Chèche class la san obligasyon compile sou yon metòd presi
            // Non class la ka: UTMH_Edu.ServiceTech.ServiceTechnique
            // Si nan pwojè ou li diferan, chanje non an la.
            Type t = Type.GetType("UTMH_Edu.ServiceTech.ServiceTechnique");

            // Si Type.GetType pa jwenn (pafwa), eseye nan assembly aktyèl la
            if (t == null)
            {
                // chèche nan tout assemblies chaje
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    t = asm.GetType("UTMH_Edu.ServiceTech.ServiceTechnique");
                    if (t != null) break;
                }
            }

            if (t == null) return false;

            // Lis non metòd posib
            string[] methodNames = new[]
            {
                "EnvoyerEmail", "EnvoyerMail", "SendEmail", "SendMail", "EnvoyerMessage", "SendMessage"
            };

            MethodInfo found = null;

            foreach (string name in methodNames)
            {
                // public static ??? (...)
                found = t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                         .FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (found != null) break;
            }

            if (found == null) return false;

            // Eseye adapte paramèt yo otomatikman
            // Nou sipòte signatures tipik:
            // (string to, string subject, string body)
            // (string to, string body)
            // (string to, string subject, string body, bool isHtml)
            // (string to, string subject, string body, string cc, string bcc) etc.
            var prms = found.GetParameters();

            object[] args = BuildArgsForServiceTechnique(prms, to, subject, body);

            if (args == null) return false;

            found.Invoke(null, args);
            return true;
        }

        private object[] BuildArgsForServiceTechnique(ParameterInfo[] prms, string to, string subject, string body)
        {
            // Nou fè maping entèlijan: string params yo resevwa to/subject/body selon non paramèt la
            // Si pa gen non, nou itilize lòd klasik: to, subject, body

            object[] args = new object[prms.Length];

            for (int i = 0; i < prms.Length; i++)
            {
                var p = prms[i];

                // string
                if (p.ParameterType == typeof(string))
                {
                    string pn = (p.Name ?? "").ToLowerInvariant();

                    if (pn.Contains("to") || pn.Contains("dest") || pn.Contains("email"))
                        args[i] = to;
                    else if (pn.Contains("subj") || pn.Contains("objet") || pn.Contains("title"))
                        args[i] = subject;
                    else if (pn.Contains("body") || pn.Contains("message") || pn.Contains("contenu") || pn.Contains("text"))
                        args[i] = body;
                    else
                    {
                        // fallback lòd
                        if (i == 0) args[i] = to;
                        else if (i == 1) args[i] = subject;
                        else args[i] = body;
                    }
                }
                // bool (souvan isHtml)
                else if (p.ParameterType == typeof(bool))
                {
                    args[i] = false; // default
                }
                // lòt tip: nou pa sipòte otomatik
                else
                {
                    // si param la optional, mete default
                    if (p.IsOptional) args[i] = Type.Missing;
                    else return null;
                }
            }

            return args;
        }

        private void SendBySmtpFallback(string to, string subject, string body)
        {
            // Sa ap itilize config SMTP nan web.config (<system.net><mailSettings>...)
            // Si SMTP pa configure, li pral bay erè klè.

            using (MailMessage msg = new MailMessage())
            {
                msg.To.Add(to);
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = false;

                // Si ou gen From nan web.config mailSettings, SmtpClient ka mete l.
                // Men kèk config mande From obligatwa, konsa n ap mete li si appSetting egziste:
                string from = ConfigurationManager.AppSettings["MailFrom"];
                if (!string.IsNullOrWhiteSpace(from))
                    msg.From = new MailAddress(from.Trim());

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Send(msg);
                }
            }
        }
    }
}