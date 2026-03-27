using System;
using System.Configuration;
using System.Data.SqlClient;

namespace UTMH_Edu.Model
{
    public class Utilisateur
    {
        static string strCon =
            ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        public static void ModifierMotPasse(int idUtilisateur, string motPasseHash)
        {
            string query = @"
                UPDATE Utilisateur
                SET motPasse = @motPasse
                WHERE idUtilisateur = @idUtilisateur";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@motPasse", motPasseHash);
                cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
