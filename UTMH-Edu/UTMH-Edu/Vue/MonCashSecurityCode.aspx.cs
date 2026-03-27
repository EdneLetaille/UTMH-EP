using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Vue
{
    public partial class MonCashSecurityCode : System.Web.UI.Page
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // ⚠️ Mets à false en prod
        private const bool SHOW_OTP_FOR_TEST = false;

        // Helpers Session (anti cast invalid)
        private string S(string key) => Session[key]?.ToString() ?? "";

        private int I(string key)
        {
            string v = S(key);
            if (string.IsNullOrWhiteSpace(v)) return 0;
            return Convert.ToInt32(v, CultureInfo.InvariantCulture);
        }

        private decimal D(string key)
        {
            string v = S(key);
            if (string.IsNullOrWhiteSpace(v)) return 0m;

            v = v.Replace(",", "."); // sécurité
            return Convert.ToDecimal(v, CultureInfo.InvariantCulture);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["INS_nom"] == null || string.IsNullOrWhiteSpace(S("INS_email")))
                {
                    Show("Session expirée. Recommencez l'inscription.", isError: true);
                    btnSend.Enabled = false;
                    lnkResend.Enabled = false;
                    return;
                }

                // ✅ si pas encore d'OTP -> générer et envoyer par email
                if (Session["MC_OTP"] == null)
                {
                    string otp = new Random().Next(100000, 999999).ToString();
                    Session["MC_OTP"] = otp;

                    // Expiration (optionnel)
                    Session["MC_OTP_EXPIRES"] = DateTime.Now.AddMinutes(5);

                    SendOtpByEmail(otp);
                    Show("Un code de vérification a été envoyé à votre email.", isError: false);
                }

                // ❌ On n'affiche plus l'OTP
                lblDevOtp.Visible = false;
            }
        }


        protected void lnkResend_Click(object sender, EventArgs e)
        {
            string otp = new Random().Next(100000, 999999).ToString();
            Session["MC_OTP"] = otp;
            Session["MC_OTP_EXPIRES"] = DateTime.Now.AddMinutes(5);

            SendOtpByEmail(otp);
            Show("Nouveau code envoyé par email.", isError: false);

            lblDevOtp.Visible = false;
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (Session["MC_OTP_EXPIRES"] != null)
            {
                DateTime exp = (DateTime)Session["MC_OTP_EXPIRES"];
                if (DateTime.Now > exp)
                {
                    Show("Code expiré. Cliquez sur 'Resend code'.", true);
                    return;
                }
            }


            string otpUser = (txtOtp.Text ?? "").Trim();
            if (otpUser.Length != 6)
            {
                Show("Veuillez entrer un code à 6 chiffres.", isError: true);
                return;
            }

            string otpSession = S("MC_OTP");
            if (otpUser != otpSession)
            {
                Show("Code incorrect. Réessayez.", isError: true);
                return;
            }

            try
            {
                // 1) Générer mot de passe temporaire
                string mdpClair = GenererMotDePasse(8);
                string mdpHash = HashPassword(mdpClair);

                // 2) Enregistrer étudiant + paiement + motPasse (hash)
                int idEtudiant = SaveStudentAndPayment(mdpHash);

                Session.Remove("MC_OTP");
                Session.Remove("MC_OTP_EXPIRES");
                Session.Remove("MC_NUM");

                // 3) Envoyer email avec le mot de passe en clair
                // ✅ Utilise ton service si tu en as un
                ServiceTechnique.SendEmail(
                     S("INS_email"),
                     "Vos identifiants - UTMH",
                     $"Bonjour {S("INS_prenom")} {S("INS_nom")},<br><br>" +
                     $"Votre inscription est réussie.<br><br>" +
                     $"Identifiant (Code) : {S("INS_code")}<br>" +
                     $"Mot de passe temporaire : <b>{mdpClair}</b><br><br>" +
                     $"Veuillez changer votre mot de passe après connexion.<br><br>" +
                     $"Cordialement,<br>UTMH"
                 );

                Session["SuccessMessage"] = "Inscription effectuée avec succès. Mot de passe envoyé par email.";
                Response.Redirect("Default.aspx");



            }
            catch (Exception ex)
            {
                Show("Erreur lors de l'enregistrement : " + ex.Message, isError: true);
            }
        }

        private int SaveStudentAndPayment(string motPasseHash)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        // 1) INSERT ETUDIANT
                        string sqlEtu = @"
INSERT INTO dbo.etudiant
(idOption, code, nom, prenom, dateNaissance, sexe, idVille, departement, adresse,
 telephone, email, statut, personneRes, telephoneRes, dateInscription,
 cin, statutPaiement, anneeAcademique, modePaiement,motPasse, role)
VALUES
(@idOption, @code, @nom, @prenom, @dateNaissance, @sexe, @idVille, @departement, @adresse,
 @telephone, @email, @statut, @personneRes, @telephoneRes, @dateInscription,
 @cin, @statutPaiement, @anneeAcademique, @modePaiement, @motPasse, @role);

SELECT SCOPE_IDENTITY();
";

                        int idEtudiant;
                        using (SqlCommand cmd = new SqlCommand(sqlEtu, con, tx))
                        {
                            // ✅ idOption = int
                            cmd.Parameters.AddWithValue("@idOption", I("INS_idOption"));

                            cmd.Parameters.AddWithValue("@code", S("INS_code"));
                            cmd.Parameters.AddWithValue("@nom", S("INS_nom"));
                            cmd.Parameters.AddWithValue("@prenom", S("INS_prenom"));
                            cmd.Parameters.AddWithValue("@dateNaissance", S("INS_dateNaissance"));
                            cmd.Parameters.AddWithValue("@sexe", S("INS_sexe"));

                            // ✅ IMPORTANT: etudiant.idVille est VARCHAR(50) dans ta DB
                            cmd.Parameters.AddWithValue("@idVille", S("INS_idVille"));

                            cmd.Parameters.AddWithValue("@departement", S("INS_departement"));
                            cmd.Parameters.AddWithValue("@adresse", S("INS_adresse"));
                            cmd.Parameters.AddWithValue("@telephone", S("INS_telephone"));
                            cmd.Parameters.AddWithValue("@email", S("INS_email"));
                            cmd.Parameters.AddWithValue("@statut", "Actif");

                            cmd.Parameters.AddWithValue("@personneRes", S("INS_personneRes"));
                            cmd.Parameters.AddWithValue("@telephoneRes", S("INS_telephoneRes"));
                            cmd.Parameters.AddWithValue("@dateInscription", S("INS_dateInscription"));
                            cmd.Parameters.AddWithValue("@cin", S("INS_cin"));

                            cmd.Parameters.AddWithValue("@statutPaiement", "Payé");
                            cmd.Parameters.AddWithValue("@anneeAcademique", S("INS_anneeAcademique"));
                            cmd.Parameters.AddWithValue("@motPasse", motPasseHash);
                            cmd.Parameters.AddWithValue("@modePaiement", "MonCash");
                            cmd.Parameters.AddWithValue("@role", "Etudiant");

                            idEtudiant = Convert.ToInt32(Convert.ToDecimal(cmd.ExecuteScalar()));

                        }

                        // 2) INSERT paiementInscription
                        decimal montant = D("INS_montantInscription");
                        string annee = S("INS_anneeAcademique");

                        string sqlPay = @"
INSERT INTO dbo.paiementInscription (idEtudiant, anneeAcademique, montant, statut, dateCreation)
VALUES (@idEtudiant, @annee, @montant, 'PAYE', GETDATE());

SELECT SCOPE_IDENTITY();
";
                        int idPaiementInscription;
                        using (SqlCommand cmd = new SqlCommand(sqlPay, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                            cmd.Parameters.AddWithValue("@annee", annee);
                            cmd.Parameters.AddWithValue("@montant", montant);

                            idPaiementInscription = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 3) INSERT transactionInscription (SUCCESS) + référence (sans SEQUENCE)
                        string sqlTrxInsert = @"
INSERT INTO dbo.transactionInscription
(idPaiementInscription, provider, reference, montant, numeroClient, statut, message, created_at)
VALUES
(@idPay, 'MONCASH', 'TMP', @montant, @numero, 'SUCCESS', 'OTP validé', GETDATE());

SELECT SCOPE_IDENTITY();
";
                        int idTrx;
                        using (SqlCommand cmd = new SqlCommand(sqlTrxInsert, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@idPay", idPaiementInscription);
                            cmd.Parameters.AddWithValue("@montant", montant);
                            cmd.Parameters.AddWithValue("@numero", S("MC_NUM"));
                            idTrx = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string reference = "MC-" + DateTime.Now.ToString("yyyyMMdd") + "-" + idTrx.ToString("D6");

                        string sqlTrxUpdate = @"
UPDATE dbo.transactionInscription
SET reference = @ref, updated_at = GETDATE()
WHERE idTransactionInscription = @idTrx;
";
                        using (SqlCommand cmd = new SqlCommand(sqlTrxUpdate, con, tx))
                        {
                            cmd.Parameters.AddWithValue("@ref", reference);
                            cmd.Parameters.AddWithValue("@idTrx", idTrx);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return idEtudiant;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        private void SendOtpByEmail(string otp)
        {
            string email = S("INS_email");
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email introuvable. Recommencez l'inscription.");

            ServiceTechnique.SendEmail(
                email,
                "Code de vérification (OTP) - UTMH",
                $"Bonjour {S("INS_prenom")} {S("INS_nom")},\n\n" +
                $"Voici votre code de vérification : {otp}\n\n" +
                "Ce code est valable quelques minutes.\n\n" +
                "Cordialement,\nUTMH"
            );
        }


        private string GenererMotDePasse(int longueur = 8)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, longueur)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }
        private void Show(string message, bool isError)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = isError ? "mc-msg text-danger" : "mc-msg text-success";
        }
    }
}
