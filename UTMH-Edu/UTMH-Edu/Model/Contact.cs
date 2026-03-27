using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UTMH_Edu.Model
{
    public class Contact
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        private string nomComplet { get; set; }
        private string telephone { get; set; }
        private string email { get; set; }
        private string message { get; set; }

        public Contact(string nomComplet, string telephone, string email, string message)
        {
            this.nomComplet = nomComplet;
            this.telephone = telephone;
            this.email = email;
            this.message = message;
        }

        public Contact() : this(null,null,null,null) { }






        public void enregistrerContact(string nomComplet, string telephone, string email, string message)
        {
            string query = @"INSERT INTO contact (nomComplet, telephone, email, message) 
                             VALUES (@nom, @telephone, @mail, @msg)";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@nom", nomComplet);
                cmd.Parameters.AddWithValue("@telephone", telephone);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@msg", message);
                try
                {

                    con.Open();
                    cmd.ExecuteNonQuery();
                }


                catch (Exception ex)
                {
                    throw new Exception("Erreur SQL : " + ex.Message);
                }
            }
        }
    }
}