using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace UTMH_Edu.Model
{
    public class DepenseAdministrative
    {
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // =========================
        // INSERT
        // =========================
        public static int Insert(int idAdm, string motif, string description, DateTime dateDepense,
                                 int quantite, string modePaiement, decimal montant)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO dbo.DepenseAdministrative(idAdm, motif, description, dateDepense, quantite, modePaiement, montant)
OUTPUT INSERTED.idDepense
VALUES(@idAdm, @motif, @description, @dateDepense, @quantite, @mode, @montant);", con))
            {
                cmd.Parameters.Add("@idAdm", SqlDbType.Int).Value = idAdm;
                cmd.Parameters.Add("@motif", SqlDbType.VarChar, 150).Value = motif;
                cmd.Parameters.Add("@description", SqlDbType.VarChar, 500).Value =
                    string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description;

                cmd.Parameters.Add("@dateDepense", SqlDbType.Date).Value = dateDepense.Date;
                cmd.Parameters.Add("@quantite", SqlDbType.Int).Value = quantite;

                cmd.Parameters.Add("@mode", SqlDbType.VarChar, 30).Value = modePaiement;

                cmd.Parameters.Add("@montant", SqlDbType.Decimal).Value = montant;
                cmd.Parameters["@montant"].Precision = 18;
                cmd.Parameters["@montant"].Scale = 2;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // =========================
        // UPDATE
        // =========================
        public static void Update(int idDepense, int idAdm, string motif, string description, DateTime dateDepense,
                                  int quantite, string modePaiement, decimal montant)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
UPDATE dbo.DepenseAdministrative
SET idAdm=@idAdm,
    motif=@motif,
    description=@description,
    dateDepense=@dateDepense,
    quantite=@quantite,
    modePaiement=@mode,
    montant=@montant
WHERE idDepense=@idDepense;", con))
            {
                cmd.Parameters.Add("@idDepense", SqlDbType.Int).Value = idDepense;
                cmd.Parameters.Add("@idAdm", SqlDbType.Int).Value = idAdm;
                cmd.Parameters.Add("@motif", SqlDbType.VarChar, 150).Value = motif;
                cmd.Parameters.Add("@description", SqlDbType.VarChar, 500).Value =
                    string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description;

                cmd.Parameters.Add("@dateDepense", SqlDbType.Date).Value = dateDepense.Date;
                cmd.Parameters.Add("@quantite", SqlDbType.Int).Value = quantite;

                cmd.Parameters.Add("@mode", SqlDbType.VarChar, 30).Value = modePaiement;

                cmd.Parameters.Add("@montant", SqlDbType.Decimal).Value = montant;
                cmd.Parameters["@montant"].Precision = 18;
                cmd.Parameters["@montant"].Scale = 2;

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0) throw new Exception("Dépense introuvable.");
            }
        }

        // =========================
        // DELETE
        // =========================
        public static void Delete(int idDepense)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
DELETE FROM dbo.DepenseAdministrative WHERE idDepense=@id;", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idDepense;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // GET ONE
        // =========================
        public static DataRow GetById(int idDepense)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP 1 *
FROM dbo.DepenseAdministrative
WHERE idDepense=@id;", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idDepense;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);

                return dt.Rows.Count == 0 ? null : dt.Rows[0];
            }
        }

        // =========================
        // LIST (avec filtres date)
        // =========================
        public static DataTable Lister(DateTime? dateDu, DateTime? dateAu)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT
    d.idDepense,
    d.idAdm,
    d.motif,
    d.description,
    d.dateDepense,
    d.quantite,
    d.modePaiement,
    d.montant,
    d.dateCreation
FROM dbo.DepenseAdministrative d
WHERE (@du IS NULL OR d.dateDepense >= @du)
  AND (@au IS NULL OR d.dateDepense <= @au)
ORDER BY d.dateDepense DESC, d.idDepense DESC;", con))
            {
                cmd.Parameters.Add("@du", SqlDbType.Date).Value = (object)dateDu?.Date ?? DBNull.Value;
                cmd.Parameters.Add("@au", SqlDbType.Date).Value = (object)dateAu?.Date ?? DBNull.Value;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);

                return dt;
            }
        }
    }
}