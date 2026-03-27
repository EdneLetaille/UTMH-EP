using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace UTMH_Edu.Model
{
    public class Salaire
    {
        private static readonly string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        // =====================================================
        // UPSERT SALAIRE (INSERT ou UPDATE)
        // =====================================================
        public static void UpsertSalaire(string typePersonne, int idPersonne, decimal montantMensuel, string actif)
        {
            if (string.IsNullOrWhiteSpace(typePersonne))
                throw new Exception("Type personne obligatoire.");

            typePersonne = typePersonne.Trim().ToUpperInvariant();

            if (typePersonne != "ADM" && typePersonne != "PROF")
                throw new Exception("Type personne invalide (ADM/PROF).");

            if (idPersonne <= 0)
                throw new Exception("Id personne invalide.");

            if (montantMensuel <= 0)
                throw new Exception("Montant invalide.");

            if (string.IsNullOrWhiteSpace(actif))
                actif = "1";

            actif = actif.Trim();

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
IF (@type='ADM')
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.salaire WHERE typePersonne='ADM' AND idAdm=@id)
    BEGIN
        UPDATE dbo.salaire
        SET montantMensuel=@montant, actif=@actif
        WHERE typePersonne='ADM' AND idAdm=@id
    END
    ELSE
    BEGIN
        INSERT INTO dbo.salaire(typePersonne, idAdm, montantMensuel, actif)
        VALUES('ADM', @id, @montant, @actif)
    END
END
ELSE
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.salaire WHERE typePersonne='PROF' AND idProf=@id)
    BEGIN
        UPDATE dbo.salaire
        SET montantMensuel=@montant, actif=@actif
        WHERE typePersonne='PROF' AND idProf=@id
    END
    ELSE
    BEGIN
        INSERT INTO dbo.salaire(typePersonne, idProf, montantMensuel, actif)
        VALUES('PROF', @id, @montant, @actif)
    END
END
", con))
            {
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = typePersonne;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;

                cmd.Parameters.Add("@montant", SqlDbType.Decimal).Value = montantMensuel;
                cmd.Parameters["@montant"].Precision = 18;
                cmd.Parameters["@montant"].Scale = 2;

                cmd.Parameters.Add("@actif", SqlDbType.VarChar, 10).Value = actif;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =====================================================
        // LIRE SALAIRE
        // =====================================================
        public static decimal GetMontantMensuel(string typePersonne, int idPersonne)
        {
            if (string.IsNullOrWhiteSpace(typePersonne))
                return 0m;

            typePersonne = typePersonne.Trim().ToUpperInvariant();

            string sql = (typePersonne == "ADM")
                ? @"
SELECT TOP 1 montantMensuel
FROM dbo.salaire
WHERE typePersonne='ADM'
  AND idAdm=@id
  AND ISNULL(actif,'1')='1';"
                : @"
SELECT TOP 1 montantMensuel
FROM dbo.salaire
WHERE typePersonne='PROF'
  AND idProf=@id
  AND ISNULL(actif,'1')='1';";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = idPersonne;

                con.Open();
                object v = cmd.ExecuteScalar();
                if (v == null || v == DBNull.Value)
                    return 0m;

                return Convert.ToDecimal(v);
            }
        }

        // =====================================================
        // LISTE DES PERSONNES AVEC SALAIRE ACTIF (POUR PAYROLL TOUS)
        // Retourne: typePersonne + idPersonne (idAdm ou idProf)
        // =====================================================
        public static DataTable ListerPersonnesAvecSalaire(string type = "TOUS")
        {
            type = (type ?? "TOUS").Trim().ToUpperInvariant();

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(@"
;WITH x AS (
    SELECT 'ADM' AS typePersonne, a.idAdm AS idPersonne
    FROM dbo.administrateur a
    UNION ALL
    SELECT 'PROF' AS typePersonne, p.idProf AS idPersonne
    FROM dbo.professeur p
)
SELECT x.typePersonne, x.idPersonne
FROM x
INNER JOIN dbo.salaire s
    ON s.typePersonne = x.typePersonne
   AND (
        (x.typePersonne='ADM'  AND s.idAdm = x.idPersonne)
     OR (x.typePersonne='PROF' AND s.idProf = x.idPersonne)
   )
   AND ISNULL(s.actif,'1')='1'
WHERE (@type='TOUS' OR x.typePersonne=@type)
ORDER BY x.typePersonne, x.idPersonne;
", con))
            {
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 10).Value = type;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);

                return dt;
            }
        }
    }
}