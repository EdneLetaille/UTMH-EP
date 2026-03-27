using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTMH_Edu.Model;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using UTMH_Edu.ServiceTech;

namespace UTMH_Edu.Controlleur
{
    public class ControlleurPaiement
    {
        // ⚠️ Mete non connectionString ou a nan Web.config
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // =========================================================
        // ENREGISTRER UN PAIEMENT (1 ligne = 1 versement)
        // - calcule montantDu selon motif
        // - calcule versement (1,2,3...)
        // - calcule total payé + balance
        // - refuse si paiement insuffisant (total < montantDu)
        // =========================================================
        public void EnregistrerPaiement(
            int idEtudiant,
            DateTime datePaiement,
            string moyenPaiement,
            string motif,
            decimal montantPaye)
        {
            // -------------------------
            // VALIDATIONS
            // -------------------------
            if (idEtudiant <= 0) throw new Exception("IdEtudiant invalide.");
            if (string.IsNullOrWhiteSpace(motif)) throw new Exception("Motif invalide.");
            if (montantPaye <= 0) throw new Exception("Le montant payé doit être supérieur à zéro.");
            if (datePaiement == default(DateTime)) datePaiement = DateTime.Now;

            // -------------------------
            // MONTANT DÛ selon MOTIF
            // -------------------------
            decimal montantDu = GetMontantDu(motif);

            if (montantDu <= 0)
                throw new Exception("Motif non reconnu ou montant dû non défini.");

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();

                // 1) Total déjà payé (par étudiant + motif)
                decimal totalDejaPaye = 0;
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT ISNULL(SUM(montantPaye),0)
                    FROM Paiement
                    WHERE idEtudiant=@id AND motif=@motif", con))
                {
                    cmd.Parameters.AddWithValue("@id", idEtudiant);
                    cmd.Parameters.AddWithValue("@motif", motif);
                    totalDejaPaye = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // Si déjà soldé, bloquer
                if (totalDejaPaye >= montantDu)
                    throw new Exception("Paiement déjà soldé. Aucun nouveau versement n'est nécessaire.");

                // 2) Prochain numéro de versement
                int prochainVersement = 1;
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT ISNULL(MAX(versement),0) + 1
                    FROM Paiement
                    WHERE idEtudiant=@id AND motif=@motif", con))
                {
                    cmd.Parameters.AddWithValue("@id", idEtudiant);
                    cmd.Parameters.AddWithValue("@motif", motif);
                    prochainVersement = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 3) Nouveau total
                decimal nouveauTotal = totalDejaPaye + montantPaye;

                // 4) Règle: si total < montantDu => ERREUR (comme tu as demandé)
                if (nouveauTotal < montantDu)
                {
                    decimal reste = montantDu - nouveauTotal;
                    throw new Exception($"Montant insuffisant. Reste à payer : {reste:N0}.");
                }

                // 5) Balance (surplus si > 0)
                decimal balance = nouveauTotal - montantDu;

                // 6) INSERT 1 ligne Paiement
                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Paiement
                    (idEtudiant, datePaiement, moyenPaiement, motif, versement, montantDu, balance, montantPaye)
                    VALUES
                    (@id, @date, @moyen, @motif, @versement, @montantDu, @balance, @montantPaye)", con))
                {
                    cmd.Parameters.AddWithValue("@id", idEtudiant);
                    cmd.Parameters.AddWithValue("@date", datePaiement);
                    cmd.Parameters.AddWithValue("@moyen", (object)moyenPaiement ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@motif", motif);
                    cmd.Parameters.AddWithValue("@versement", prochainVersement);
                    cmd.Parameters.AddWithValue("@montantDu", montantDu);
                    cmd.Parameters.AddWithValue("@balance", balance);
                    cmd.Parameters.AddWithValue("@montantPaye", montantPaye);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================================================
        // RESUME ETUDIANT (Total payé / Montant dû / Balance)
        // =========================================================
        public DataTable LireResumeEtudiant(int idEtudiant, string motif)
        {
            if (idEtudiant <= 0) throw new Exception("IdEtudiant invalide.");
            if (string.IsNullOrWhiteSpace(motif)) throw new Exception("Motif invalide.");

            decimal montantDu = GetMontantDu(motif);

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT
                    @idEtudiant AS idEtudiant,
                    @motif AS motif,
                    ISNULL(SUM(montantPaye),0) AS totalPaye,
                    @montantDu AS montantDu,
                    (ISNULL(SUM(montantPaye),0) - @montantDu) AS balance
                FROM Paiement
                WHERE idEtudiant=@idEtudiant AND motif=@motif;", con))
            {
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);
                cmd.Parameters.AddWithValue("@montantDu", montantDu);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // =========================================================
        // LISTE HISTORIQUE (toutes les lignes Paiement)
        // =========================================================
        public DataTable ListerHistoriqueEtudiant(int idEtudiant, string motif)
        {
            if (idEtudiant <= 0) throw new Exception("IdEtudiant invalide.");
            if (string.IsNullOrWhiteSpace(motif)) throw new Exception("Motif invalide.");

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT
                    idPaiement,
                    datePaiement,
                    moyenPaiement,
                    motif,
                    versement,
                    montantPaye,
                    montantDu,
                    balance
                FROM Paiement
                WHERE idEtudiant=@idEtudiant AND motif=@motif
                ORDER BY versement ASC, datePaiement ASC;", con))
            {
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // =========================================================
        // SUPPRIMER TOUS LES PAIEMENTS D'UN ETUDIANT (par motif)
        // =========================================================
        public string SupprimerPaiementEtudiant(int idEtudiant, string motif)
        {
            if (idEtudiant <= 0) return "IdEtudiant invalide.";
            if (string.IsNullOrWhiteSpace(motif)) return "Motif invalide.";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
                DELETE FROM Paiement
                WHERE idEtudiant=@id AND motif=@motif;", con))
            {
                cmd.Parameters.AddWithValue("@id", idEtudiant);
                cmd.Parameters.AddWithValue("@motif", motif);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0 ? "Paiements supprimés avec succès." : "Aucun paiement à supprimer.";
            }
        }

        // =========================================================
        // REGLE MOTIF -> MONTANT DÛ
        // (ajoute stage si ou vle)
        // =========================================================
        private decimal GetMontantDu(string motif)
        {
            // Normalize (evite casse)
            string m = (motif ?? "").Trim().ToUpperInvariant();

            if (m == "ANNEE_ACADEMIQUE" || m == "ANNEE ACADEMIQUE") return 20000;
            if (m == "GRADUATION") return 25000;

            // Si stage gen pri fiks, mete li la
            // if (m == "STAGE") return 15000;

            return 0; // motif pa gen montantDu defini
        }
    }
}