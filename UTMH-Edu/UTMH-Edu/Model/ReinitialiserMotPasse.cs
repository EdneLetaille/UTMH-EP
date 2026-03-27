using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace UTMH_Edu.Model
{
    public class ReinitialiserMotPasse
    {
        static string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        public int idUtilisateur { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public bool dejaUtiliser { get; set; }
        public string typeUtilisateur { get; set; }

        public ReinitialiserMotPasse() { }

        public ReinitialiserMotPasse(
            int idUtilisateur,
            string token,
            DateTime expiration,
            string typeUtilisateur)
        {
            this.idUtilisateur = idUtilisateur;
            this.token = token;
            this.expiration = expiration;
            this.typeUtilisateur = typeUtilisateur;
            this.dejaUtiliser = false;
        }

        // INSÉRER LE TOKEN
        public void InsererToken()
        {
            string query = @"
                INSERT INTO ReinitialiserMotPasse
                (idUtilisateur, token, expiration, typeUtilisateur, dejaUtiliser)
                VALUES
                (@idUtilisateur, @token, @expiration, @typeUtilisateur, @dejaUtiliser)";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);
                cmd.Parameters.AddWithValue("@token", token);
                cmd.Parameters.AddWithValue("@expiration", expiration);
                cmd.Parameters.AddWithValue("@typeUtilisateur", typeUtilisateur);
                cmd.Parameters.Add("@dejaUtiliser", SqlDbType.Bit).Value = dejaUtiliser;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // TOKEN VALIDE ?
        public static bool TokenValide(string token)
        {
            string query = @"
                SELECT COUNT(*)
                FROM ReinitialiserMotPasse
                WHERE token = @token
                AND dejaUtiliser = 0
                AND expiration > GETUTCDATE()";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // ID UTILISATEUR
        public static int GetIdUtilisateurByToken(string token)
        {
            string query = @"
                SELECT idUtilisateur
                FROM ReinitialiserMotPasse
                WHERE token = @token";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // TYPE UTILISATEUR
        public static string GetTypeUtilisateurByToken(string token)
        {
            string query = @"
                SELECT typeUtilisateur
                FROM ReinitialiserMotPasse
                WHERE token = @token";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        // MARQUER UTILISÉ
        public static void MarquerTokenUtilise(string token)
        {
            string query = @"
                UPDATE ReinitialiserMotPasse
                SET dejaUtiliser = 1
                WHERE token = @token";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@token", token);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
