using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using UTMH_Edu.Model;
using UTMH_Edu.ServiceTech;


namespace UTMH_Edu.Model
{
    public class Paiement
    {
        public static string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // =========================
        // PROPRIETES (colonne table Paiement)
        // =========================
        public int IdPaiement { get; private set; }
        public int IdEtudiant { get; set; }
        public DateTime DatePaiement { get; set; }
        public string MoyenPaiement { get; set; }
        public string Motif { get; set; }

        // versement = 1,2,3...
        public int Versement { get; private set; }

        // montantPaye = montant du versement
        public decimal MontantPaye { get; set; }

        // montantDu = total à payer (20000 / 25000)
        public decimal MontantDu { get; private set; }

        // balance = (total payé - montantDu)
        public decimal Balance { get; private set; }

        public Paiement() { }

        // =========================================================
        // ENREGISTRER UN PAIEMENT (1 ligne = 1 versement)
        // =========================================================
        public void EnregistrerPaiement()
        {
            if (IdEtudiant <= 0) throw new Exception("IdEtudiant invalide.");
            if (string.IsNullOrWhiteSpace(Motif)) throw new Exception("Motif invalide.");
            if (MontantPaye <= 0) throw new Exception("Le montant payé doit être supérieur à zéro.");
            if (DatePaiement == default(DateTime)) DatePaiement = DateTime.Now;

            // 1) Montant dû selon motif
            MontantDu = GetMontantDu(Motif);
            if (MontantDu <= 0)
                throw new Exception("Motif non reconnu ou montant dû non défini.");

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlTransaction tr = con.BeginTransaction())
                {
                    try
                    {
                        // 2) Total déjà payé (idEtudiant + motif)
                        decimal totalDejaPaye = 0;
                        using (SqlCommand cmd = new SqlCommand(@"
                            SELECT ISNULL(SUM(montantPaye),0)
                            FROM dbo.Paiement
                            WHERE idEtudiant=@id AND motif=@motif;", con, tr))
                        {
                            cmd.Parameters.AddWithValue("@id", IdEtudiant);
                            cmd.Parameters.AddWithValue("@motif", Motif);
                            totalDejaPaye = Convert.ToDecimal(cmd.ExecuteScalar());
                        }

                        // Si déjà soldé => bloquer
                        if (totalDejaPaye >= MontantDu)
                            throw new Exception("Paiement déjà soldé. Aucun nouveau versement n'est nécessaire.");

                        // 3) Prochain numéro de versement
                        int prochainVersement = 1;
                        using (SqlCommand cmd = new SqlCommand(@"
                            SELECT ISNULL(MAX(versement),0) + 1
                            FROM dbo.Paiement
                            WHERE idEtudiant=@id AND motif=@motif;", con, tr))
                        {
                            cmd.Parameters.AddWithValue("@id", IdEtudiant);
                            cmd.Parameters.AddWithValue("@motif", Motif);
                            prochainVersement = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 4) Nouveau total
                        decimal nouveauTotal = totalDejaPaye + MontantPaye;

                        // ✅ Règle ou te mande: si total < montantDu => erreur
                        if (nouveauTotal < MontantDu)
                        {
                            decimal reste = MontantDu - nouveauTotal;
                            throw new Exception($"Montant insuffisant. Reste à payer : {reste:N0}.");
                        }

                        // 5) Balance (surplus si > 0)
                        Balance = nouveauTotal - MontantDu;
                        Versement = prochainVersement;

                        // 6) Insert ligne Paiement
                        using (SqlCommand cmd = new SqlCommand(@"
                            INSERT INTO dbo.Paiement
                            (idEtudiant, datePaiement, moyenPaiement, motif, versement, montantDu, balance, montantPaye)
                            OUTPUT INSERTED.idPaiement
                            VALUES
                            (@idEtudiant, @datePaiement, @moyenPaiement, @motif, @versement, @montantDu, @balance, @montantPaye);", con, tr))
                        {
                            cmd.Parameters.AddWithValue("@idEtudiant", IdEtudiant);
                            cmd.Parameters.AddWithValue("@datePaiement", DatePaiement);
                            cmd.Parameters.AddWithValue("@moyenPaiement", (object)MoyenPaiement ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@motif", Motif);
                            cmd.Parameters.AddWithValue("@versement", Versement);
                            cmd.Parameters.AddWithValue("@montantDu", MontantDu);
                            cmd.Parameters.AddWithValue("@balance", Balance);
                            cmd.Parameters.AddWithValue("@montantPaye", MontantPaye);

                            IdPaiement = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Rollback();
                        throw new Exception("Erreur SQL : " + ex.Message);
                    }
                }
            }
        }

        // =========================================================
        // LISTER HISTORIQUE (tous les versements)
        // =========================================================
        public static DataTable ListerPaiementsEtudiant(int idEtudiant, string motif)
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
                FROM dbo.Paiement
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
        // LIRE RESUME (total payé / montant dû / balance)
        // =========================================================
        public static DataTable LireResumeEtudiant(int idEtudiant, string motif)
        {
            if (idEtudiant <= 0) throw new Exception("IdEtudiant invalide.");
            if (string.IsNullOrWhiteSpace(motif)) throw new Exception("Motif invalide.");

            decimal montantDu = GetMontantDuStatic(motif);
            if (montantDu <= 0) throw new Exception("Motif non reconnu ou montant dû non défini.");

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT
                    @idEtudiant AS idEtudiant,
                    @motif AS motif,
                    ISNULL(SUM(montantPaye),0) AS totalPaye,
                    @montantDu AS montantDu,
                    (ISNULL(SUM(montantPaye),0) - @montantDu) AS balance
                FROM dbo.Paiement
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
        // SUPPRIMER TOUT (par étudiant + motif)
        // =========================================================
        public static string SupprimerPaiementEtudiant(int idEtudiant, string motif)
        {
            try
            {
                if (idEtudiant <= 0) return "IdEtudiant invalide.";
                if (string.IsNullOrWhiteSpace(motif)) return "Motif invalide.";

                using (SqlConnection con = new SqlConnection(strCon))
                using (SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM dbo.Paiement
                    WHERE idEtudiant=@idEtudiant AND motif=@motif;", con))
                {
                    cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                    cmd.Parameters.AddWithValue("@motif", motif);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0 ? "Suppression réussie." : "Aucun paiement à supprimer.";
                }
            }
            catch
            {
                return "Suppression non réussie.";
            }
        }

        // =========================================================
        // REGLE MOTIF -> MONTANT DÛ
        // =========================================================
        private decimal GetMontantDu(string motif)
        {
            return GetMontantDuStatic(motif);
        }

        private static decimal GetMontantDuStatic(string motif)
        {
            string m = (motif ?? "").Trim().ToUpperInvariant();

            if (m == "ANNEE_ACADEMIQUE" || m == "ANNEE ACADEMIQUE") return 20000;
            if (m == "GRADUATION") return 25000;

            // Ajoute lòt motif si yo gen pri fiks:
            // if (m == "STAGE") return 15000;

            return 0;
        }
    }
}