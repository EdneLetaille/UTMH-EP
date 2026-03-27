using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace UTMH_Edu.Model
{
    public class Payroll
    {
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // =========================================================
        // GENERER PAYROLL (1 personne)
        // datePayroll = GETDATE()
        // dateCreation = GETDATE()
        // =========================================================
        public static int GenererPayroll(string typePersonne, int idPersonne, int mois, int annee)
        {
            if (string.IsNullOrWhiteSpace(typePersonne))
                throw new Exception("Type personne invalide.");

            typePersonne = typePersonne.Trim().ToUpperInvariant();

            if (typePersonne != "ADM" && typePersonne != "PROF")
                throw new Exception("Type personne doit être ADM ou PROF.");

            if (idPersonne <= 0) throw new Exception("Personne invalide.");
            if (mois < 1 || mois > 12) throw new Exception("Mois invalide.");
            if (annee < 2000 || annee > 2100) throw new Exception("Année invalide.");

            // Salaire mensuel (table salaire toujou itilize idPersonne)
            decimal montant = Salaire.GetMontantMensuel(typePersonne, idPersonne);
            if (montant <= 0)
                throw new Exception("Salaire non défini pour cette personne.");

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                using (SqlTransaction tr = con.BeginTransaction())
                {
                    try
                    {
                        // Anti-doublon (selon type -> idAdm / idProfesseur)
                        using (SqlCommand chk = new SqlCommand(@"
SELECT TOP 1 idPayroll
FROM dbo.payroll
WHERE typePersonne=@type
  AND mois=@mois AND annee=@annee
  AND (
        (@type='ADM'  AND idAdm=@id)
     OR (@type='PROF' AND idProfesseur=@id)
  );", con, tr))
                        {
                            chk.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = typePersonne;
                            chk.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;
                            chk.Parameters.Add("@mois", SqlDbType.Int).Value = mois;
                            chk.Parameters.Add("@annee", SqlDbType.Int).Value = annee;

                            object exist = chk.ExecuteScalar();
                            if (exist != null && exist != DBNull.Value)
                                throw new Exception("Payroll déjà généré pour ce mois.");
                        }

                        // Insert payroll (selon type)
                        using (SqlCommand ins = new SqlCommand(@"
INSERT INTO dbo.payroll
(typePersonne, idAdm, idProfesseur, mois, annee, montant, statut, dateCreation, datePayroll)
OUTPUT INSERTED.idPayroll
VALUES
(@type,
 CASE WHEN @type='ADM' THEN @id ELSE NULL END,
 CASE WHEN @type='PROF' THEN @id ELSE NULL END,
 @mois, @annee, @montant, 'EN_ATTENTE', GETDATE(), GETDATE());", con, tr))
                        {
                            ins.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = typePersonne;
                            ins.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;
                            ins.Parameters.Add("@mois", SqlDbType.Int).Value = mois;
                            ins.Parameters.Add("@annee", SqlDbType.Int).Value = annee;

                            ins.Parameters.Add("@montant", SqlDbType.Decimal).Value = montant;
                            ins.Parameters["@montant"].Precision = 18;
                            ins.Parameters["@montant"].Scale = 2;

                            int idPayroll = Convert.ToInt32(ins.ExecuteScalar());
                            tr.Commit();
                            return idPayroll;
                        }
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        // =========================================================
        // VALIDER PAIEMENT (datePaiement = GETDATE)
        // =========================================================
        public static void ValiderPaiement(int idPayroll, string noCheque, int idAdmin)
        {
            if (idPayroll <= 0) throw new Exception("Payroll invalide.");
            if (idAdmin <= 0) throw new Exception("Admin invalide.");

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
UPDATE dbo.payroll
SET statut='PAYE',
    datePaiement=GETDATE(),
    noCheque=@noCheque,
    idUserPaiement=@idAdmin
WHERE idPayroll=@idPayroll AND ISNULL(statut,'') <> 'PAYE';", con))
            {
                cmd.Parameters.Add("@idPayroll", SqlDbType.Int).Value = idPayroll;

                if (string.IsNullOrWhiteSpace(noCheque))
                    cmd.Parameters.Add("@noCheque", SqlDbType.VarChar, 50).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@noCheque", SqlDbType.VarChar, 50).Value = noCheque.Trim();

                cmd.Parameters.Add("@idAdmin", SqlDbType.Int).Value = idAdmin;

                con.Open();
                if (cmd.ExecuteNonQuery() == 0)
                    throw new Exception("Payroll introuvable ou déjà payé.");
            }
        }

        // =========================================================
        // LISTER PAYROLL
        // =========================================================
        public static DataTable ListerPayroll(int mois, int annee, string statut = "0", string typePersonne = "0")
        {
            statut = (statut ?? "0").Trim().ToUpperInvariant();
            typePersonne = (typePersonne ?? "0").Trim().ToUpperInvariant();

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
SELECT 
    idPayroll,
    typePersonne,
    idAdm,
    idProfesseur,
    mois,
    annee,
    montant,
    statut,
    dateCreation,
    datePayroll,
    datePaiement,
    noCheque,
    idUserPaiement
FROM dbo.payroll
WHERE (@mois = 0 OR mois = @mois)
  AND (@annee = 0 OR annee = @annee)
  AND (@statut = '0' OR statut = @statut)
  AND (@type = '0' OR typePersonne = @type)
ORDER BY datePayroll DESC;", con))
            {
                cmd.Parameters.Add("@mois", SqlDbType.Int).Value = mois;
                cmd.Parameters.Add("@annee", SqlDbType.Int).Value = annee;
                cmd.Parameters.Add("@statut", SqlDbType.VarChar, 20).Value = statut;
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = typePersonne;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}