using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace UTMH_Edu.Model
{
    public class Log
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        public void AjouterLog(
            string code,
            string page,
            string action,
            string role
        )
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
                    INSERT INTO dbo.log
                    (code, page, action, dateConnexion, role)
                    VALUES
                    (@code, @page, @action, @dateConnexion, @role)
                ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@dateConnexion",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@role", role);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void MettreAJourDeconnexion(string code)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            UPDATE dbo.log
            SET dateDeconnexion = @dateDeconnexion
            WHERE code = @code
              AND dateDeconnexion IS NULL
        ";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@dateDeconnexion",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                );

                cmd.Parameters.AddWithValue("@code", code);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public DataTable GetLogs(string code, DateTime? date)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
                SELECT
                    idLog,
                    code,
                    page,
                    action,
                    role,
                    dateConnexion,
                    dateDeconnexion
                FROM dbo.log
                WHERE
                    (@code IS NULL OR code LIKE '%' + @code + '%')
                AND (
                    @date IS NULL
                    OR (dateConnexion >= @date
                        AND dateConnexion < DATEADD(day, 1, @date))
                )
                ORDER BY dateConnexion DESC";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@code",
                    string.IsNullOrWhiteSpace(code)
                    ? (object)DBNull.Value
                    : code.Trim());

                cmd.Parameters.AddWithValue("@date",
                    date.HasValue
                    ? (object)date.Value
                    : DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }




    }
}
